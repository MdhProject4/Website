using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ProjectFlight.Managers
{
	/// <summary>
	/// Delivers messages/notifications over web socket to users
	/// </summary>
	public static class WebSocketDeliverer
	{
		/// <summary>
		/// Users and their assigned WebSocket
		/// </summary>
		private static Dictionary<string, WebSocket> users;

		/// <summary>
		/// Sets everything up, like a constructor sort of
		/// </summary>
		public static void Create() => users = new Dictionary<string, WebSocket>();

		/// <summary>
		/// Handles incoming requests and checks if it's a WebSocket request
		/// </summary>
		public static async Task Handle(HttpContext context, Func<Task> next)
		{
			// Check if valid WebSocket request
			if (context.Request.Path == "/ws" && context.WebSockets.IsWebSocketRequest)
			{
				// Respond to it (upgrade tcp to web socket)
				var webSocket = await context.WebSockets.AcceptWebSocketAsync();
				await Respond(context, webSocket);
			}
			else
				await next();
		}

		/// <summary>
		/// Respond to a WebSocket request
		/// </summary>
		/// <param name="context">Current HTTP context</param>
		/// <param name="webSocket">Incoming web socket</param>
		private static async Task Respond(HttpContext context, WebSocket webSocket)
		{
			// Get user
			var username = SessionManager.Get(context);

			// Check if user was found
			if (username == null)
				return;

			// Generous 16 byte buffer
			var buffer = new byte[16];

			// Save client
			Console.WriteLine($"WebSocket Connection: {username}");
			users[username] = webSocket;

			// Get response from client
			var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

			// TODO: For now, always respond with OK
			var ok = Encoding.ASCII.GetBytes("OK");

			while (!result.CloseStatus.HasValue)
			{
				await webSocket.SendAsync(
					new ArraySegment<byte>(ok),
					result.MessageType,
					result.EndOfMessage,
					CancellationToken.None);

				result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}

			// Close handshake
			await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);

			// Log
			Console.WriteLine($"WebSocket Disconnection: {username}");
		}

		/// <summary>
		/// Send a message to the specified user
		/// </summary>
		/// <param name="username">User to send the message to</param>
		/// <param name="message">The message to send</param>
		public static async void Send(string username, string message)
		{
			if (!users.ContainsKey(username))
				return;

			await users[username].SendAsync(
				new ArraySegment<byte>(Encoding.ASCII.GetBytes(message)),
				WebSocketMessageType.Text,
				true,
				CancellationToken.None);
		}
	}
}
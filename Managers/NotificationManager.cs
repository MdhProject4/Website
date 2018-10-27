using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ProjectFlight.Data;

namespace ProjectFlight.Managers
{
	public static class NotificationManager
	{
		/// <summary>
		/// Users and their assigned WebSocket
		/// </summary>
		private static Dictionary<string, WebSocket> users;

		/// <summary>
		/// Update and check if any notifications should be sent
		/// </summary>
		/// <param name="infos"></param>
		public static void Update(IEnumerable<FlightInfo> infos)
		{
			// Get saved notifications
			FlightNotification[] notifications;
			using (var context = new ApplicationDbContext())
				notifications = context.FlightNotifications.ToArray();

			// If it's empty, just return
			if (!notifications.Any())
				return;

			// Loop through notifications to try and find matches
			foreach (var notification in notifications)
			{
				// TODO
			}
		}

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
			// Temporary buffer to store messages in
			var buffer = new byte[1024 * 4];

			// Get the result from the client
			var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);


			while (!result.CloseStatus.HasValue)
			{
				await webSocket.SendAsync(
					new ArraySegment<byte>(buffer, 0, result.Count),
					result.MessageType,
					result.EndOfMessage,
					CancellationToken.None);

				result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
			}

			await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
		}
	}
}
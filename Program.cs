using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ProjectFlight
{
	public class Program
	{
		public static void Main(string[] args) => 
			CreateWebHostBuilder(args).Build().Run();

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, builder) => { builder.AddJsonFile("connection.json"); })
				.UseStartup<Startup>();
	}
}
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectFlight.Data;

namespace ProjectFlight
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			// Ensure database and tables are created
			using (var scope = host.Services.CreateScope())
				scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.EnsureCreated();

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.ConfigureAppConfiguration((context, builder) => { builder.AddJsonFile("connection.json"); })
				.UseStartup<Startup>();
	}
}
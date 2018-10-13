using Microsoft.EntityFrameworkCore;

namespace ProjectFlight.Data
{
	/// <summary>
	/// Representation of our database
	/// </summary>
	public class ApplicationDbContext : DbContext
	{
		public DbSet<FlightInfo> FlightInfos { get; set; }

		public static string ConnectionString { private get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlServer(ConnectionString);
		}
	}
}
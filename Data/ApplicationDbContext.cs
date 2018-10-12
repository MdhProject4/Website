using Microsoft.EntityFrameworkCore;

namespace ProjectFlight.Data
{
	/// <summary>
	/// Representation of our database
	/// </summary>
	public class ApplicationDbContext : DbContext
	{
		public DbSet<FlightInfo> FlightInfos { get; set; }

		public ApplicationDbContext(DbContextOptions options) : base(options)
		{
		}
	}
}
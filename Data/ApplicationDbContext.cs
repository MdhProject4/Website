using Microsoft.EntityFrameworkCore;

namespace ProjectFlight.Data
{
	/// <summary>
	/// Representation of our database
	/// </summary>
	public class ApplicationDbContext : DbContext
	{
		#region Database tables

		public DbSet<FlightInfo> FlightInfos { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<FlightBookmark> FlightBookmarks { get; set; }

		public DbSet<FlightNotification> FlightNotifications { get; set; }

		#endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
			optionsBuilder.UseSqlite("Data Source=cats.db");
		}
	}
}
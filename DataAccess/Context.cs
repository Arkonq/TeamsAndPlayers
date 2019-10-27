using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class Context : DbContext
	{
		string connectionString;

		public Context(string serverName)
		{
			this.connectionString = $"Server={serverName};Database=TeamsAndPlayers_;Trusted_Connection=true;";
			Database.EnsureCreated();
		}

		public DbSet<Team> Teams { get; set; }
		public DbSet<Player> Players { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseLazyLoadingProxies().UseSqlServer(connectionString);
		}
	}
}

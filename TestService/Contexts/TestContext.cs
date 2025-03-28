using Microsoft.EntityFrameworkCore;
using TestService.Models;

namespace TestService.Contexts
{
	public class TestContext : DbContext
	{
		private readonly string _connectionString;

		public DbSet<BasicClass> BasicClasses { get; set; }



		public TestContext(IConfiguration configuration)
		{
			_connectionString = configuration["ConnectionStrings:DbContext"]!;

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite(_connectionString);
			//optionsBuilder.UseSqlServer(_connectionString);
			base.OnConfiguring(optionsBuilder);
		}
	}
}

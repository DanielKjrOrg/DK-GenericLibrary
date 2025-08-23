using Microsoft.EntityFrameworkCore;


namespace RepoUnitTests.Fakes
{
	public class FakeContext : DbContext
	{
		public DbSet<BasicClass> BasicClasses { get; set; }

		public FakeContext(DbContextOptions<FakeContext> options)
			: base(options)
		{
		}



		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{

		}
	}

}

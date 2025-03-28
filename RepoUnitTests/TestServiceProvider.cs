using DK.GenericLibrary;
using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepoUnitTests.Fakes;


namespace RepoUnitTests
{

	/// <summary>
	/// Static class to provide a service provider for testing
	/// </summary>
	public static class TestServiceProvider
	{

		/// <summary>
		/// Returns a ServiceProvider with the necessary services for testing
		/// </summary>
		/// <returns></returns>
		public static ServiceProvider GetServiceProvider()
		{
			var services = new ServiceCollection();
			var random = new Random();

			services.AddTransient(typeof(IAsyncRepository<>), typeof(AsyncRepository<>));
			//services.AddTransient<FakeContext>();
			//Datetime wasn't enough to make sure they had different names, so random was added
			services.AddDbContextFactory<FakeContext>(options =>
				options.UseInMemoryDatabase("TestDatabase"+ DateTime.Now + random.Next(1,500)));
			 return services.BuildServiceProvider();
		}
	
	}

}

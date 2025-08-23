using DK.GenericLibrary.ServiceCollection;
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
		public static ServiceProvider GetTransientServiceProvider(bool asyncProvider = true)
		{
			var services = new ServiceCollection();
			var random = new Random();
			if (asyncProvider)
				services.AddTransientAsyncRepository<FakeContext>();
			else
				services.AddTransientRepository<FakeContext>();


			//Datetime wasn't enough to make sure they had different names, so random was added
			services.AddDbContextFactory<FakeContext>(options =>
				options.UseInMemoryDatabase("TestDatabase" + DateTime.Now + random.Next(1, 500)));
			return services.BuildServiceProvider();
		}

		/// <summary>
		/// Returns a ServiceProvider with the necessary services for testing
		/// </summary>
		/// <returns></returns>
		public static ServiceProvider GetScopedAsyncServiceProvider(bool asyncProvider = true)
		{
			var services = new ServiceCollection();
			var random = new Random();
			if (asyncProvider)
				services.AddScopedAsyncRepository<FakeContext>();
			else
				services.AddScopedRepository<FakeContext>();
			//Datetime wasn't enough to make sure they had different names, so random was added
			services.AddDbContextFactory<FakeContext>(options =>
				options.UseInMemoryDatabase("TestDatabase" + DateTime.Now + random.Next(1, 500)));
			return services.BuildServiceProvider();
		}


		/// <summary>
		/// Returns a ServiceProvider with the necessary services for testing
		/// </summary>
		/// <returns></returns>
		public static ServiceProvider GetSingletonAsyncServiceProvider(bool asyncProvider = true)
		{
			var services = new ServiceCollection();
			var random = new Random();
			if (asyncProvider)
				services.AddSingletonAsyncRepository<FakeContext>();
			else
				services.AddSingletonRepository<FakeContext>();
			//Datetime wasn't enough to make sure they had different names, so random was added
			services.AddDbContextFactory<FakeContext>(options =>
				options.UseInMemoryDatabase("TestDatabase" + DateTime.Now + random.Next(1, 500)));
			return services.BuildServiceProvider();
		}

	}

}

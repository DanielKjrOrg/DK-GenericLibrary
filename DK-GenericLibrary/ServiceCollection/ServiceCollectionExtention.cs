using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DK.GenericLibrary.ServiceCollection
{
	/// <summary>
	/// Methods for adding the AsyncRepository to the IServiceCollection
	/// </summary>
	public static class ServiceCollectionExtention
	{
		/// <summary>
		/// Adds the AsyncRepository as a scoped service
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddScopedAsyncRepository<TContext>(this IServiceCollection services) where TContext : DbContext
		{
			services.AddScoped<IAsyncRepository<TContext>, AsyncRepository<TContext>>();
			return services;
		}


		/// <summary>
		/// Adds the AsyncRepository as a transient service
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddTransientAsyncRepository<TContext>(this IServiceCollection services) where TContext : DbContext
		{
			services.AddTransient<IAsyncRepository<TContext>, AsyncRepository<TContext>>();
			return services;
		}

		/// <summary>
		/// Adds the AsyncRepository as a singleton service
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddSingletonAsyncRepository<TContext>(this IServiceCollection services) where TContext : DbContext
		{
			services.AddSingleton<IAsyncRepository<TContext>, AsyncRepository<TContext>>();
			return services;
		}



		/// <summary>
		/// Adds the Repository as a scoped service
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddScopedRepository<TContext>(this IServiceCollection services) where TContext : DbContext
		{
			services.AddScoped<IRepository<TContext>, Repository<TContext>>();
			return services;
		}


		/// <summary>
		/// Adds the Repository as a transient service
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddTransientRepository<TContext>(this IServiceCollection services) where TContext : DbContext
		{
			services.AddTransient<IRepository<TContext>, Repository<TContext>>();
			return services;
		}

		/// <summary>
		/// Adds the Repository as a singleton service
		/// </summary>
		/// <typeparam name="TContext"></typeparam>
		/// <param name="services"></param>
		/// <returns></returns>
		public static IServiceCollection AddSingletonRepository<TContext>(this IServiceCollection services) where TContext : DbContext
		{
			services.AddSingleton<IRepository<TContext>, Repository<TContext>>();
			return services;
		}
	}
}

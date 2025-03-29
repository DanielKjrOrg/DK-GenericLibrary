using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DK.GenericLibrary.Interfaces
{
	/// <summary>
	/// Interface providing generic async methods for interacting with EF Core's DbContext.
	/// <para>
	/// Must be registered as a service in Startup.cs using one of the provided AddTransient, AddScoped, AddSingleton versions Example:
	/// <![CDATA[
	/// builder.Services.AddTransientAsyncRepository<TestContext>();
	/// ]]>
	/// </para>
	/// <para>
	/// Alongside a DbContext factory:
	/// <![CDATA[
	/// builder.Services.AddDbContextFactory<YourContext>();
	/// ]]>
	/// </para>
	/// </summary>
	/// <typeparam name="TContext">The DbContext type</typeparam>
	public interface IAsyncRepository<TContext> where TContext : DbContext
	{
		/// <summary>
		/// Adds the TEntity to DbSet matching the type parameter
		/// </summary>
		/// <typeparam name="TEntity">TEntity that exists in the DbContext</typeparam>
		/// <param name="entity">TEntity reference</param>
		/// <returns></returns>
		Task AddItem<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Adds the TEntities to the DbSet matching the type parameter
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entities">List of TEntity to add to DbSet</param>
		/// <returns></returns>
		Task AddItems<TEntity>(List<TEntity> entities) where TEntity : class;

		/// <summary>
		/// Removes a TEntity from the DbSet matching type parameter
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		Task RemoveItem<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Removes a TEntity matching the provided searchExpression from the DbSet matching type parameter
		/// <para>
		/// <![CDATA[Example: _repository.RemoveItem<TEntity>(x => x.Id == 222);]]>
		/// </para>
		/// </summary>
		/// <typeparam name="TEntity">Entity that exists in a DbSet</typeparam>
		/// <param name="searchExpression">Lambda expression</param>
		/// <returns></returns>
		Task RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class;

		/// <summary>
		/// Removes all TEntities that match the provided queryOperation from the DbSet matching type parameter
		/// <para>
		/// <![CDATA[Example: _repository.RemoveItems<TEntity>(query => query.Where(x => x.Age == 5));]]>
		/// </para>
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryOperation">IQueryable expression</param>
		/// <returns></returns>
		Task RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class;

		/// <summary>
		/// Removes the TEntities from the DbSet matching type parameters
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="itemsToRemove">List of TEntities to remove from DbSet</param>
		/// <returns></returns>
		Task RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class;

		/// <summary>
		/// Returns the TEntity matching the queryOperation from the DbSet matching type parameters
		/// <para>
		/// <![CDATA[Example: _repository.GetItem<TEntity>(query => query.Where(x => x.ID == 201023));]]>
		/// </para>
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryOperation">IQueryable expression</param>
		/// <returns>FirstOrDefault TEntity</returns>
		Task<TEntity> GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class;

		/// <summary>
		/// Returns TEntity list of DbSet matching type parameters by default.
		/// <para>
		/// Returns TEntity list of entries matching queryOperation when optional parameter is used.
		/// </para>
		/// <para>
		/// <![CDATA[Example: _repository.GetAllItems<TEntity>(query => query.Where(x=> x.Age == 4));]]>
		/// </para>
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryOperation">Optional IQueryable expression</param>
		/// <returns>List{TEntity}</returns>
		Task<List<TEntity>> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class;

		/// <summary>
		/// Returns T from DbSet matching type parameter.
		/// <para>Used to retrieve a specific column</para>
		/// <para>
		/// <![CDATA[Example: _repository.GetAllForColumn<TEntity, string>(q => q.Select(x => x.PropertyName))]]>
		/// </para>
		/// </summary>
		/// <typeparam name="TEntity">TEntity that is in a DbSet</typeparam>
		/// <typeparam name="T">Expected return type</typeparam>
		/// <param name="queryOperation">IQueryable expression</param>
		/// <returns>List{T}</returns>
		Task<List<T>> GetAllItems<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class;

		/// <summary>
		/// Returns T from DbSet matching type parameter.
		/// <para>Used to retrieve struct data types</para>
		/// <para>
		/// <![CDATA[Example: _repository.GetAllForColumn<TEntity, int>(q => q.Select(x => x.PropertyName))]]>
		/// </para>
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOperation"></param>
		/// <returns></returns>
		Task<List<T>> GetAllItemsStruct<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : struct;

		/// <summary>
		/// Changes TEntity reference and its' collections EntityState to Modified. Note that it will only catch 1 nested collection, anything past that can be put directly as a parameter as ut us a TEntity
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item">Tracked TEntity</param>
		/// <returns></returns>
		Task UpdateItem<TEntity>(TEntity item) where TEntity : class;

		/// <summary>
		/// Changes TEntity references and their collections EntityState to Modified. Note that it will only catch 1 nested collection, anything past that can be put directly as a parameter as it is a TEntity
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="items">List of tracked TEntities</param>
		/// <returns></returns>
		Task UpdateItems<TEntity>(List<TEntity> items) where TEntity : class;


	}
}


using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DK_NuGet_Library.Interfaces
{
	/// <summary>
	/// Injectable generic interface that provides basic CRUD functionality and universal functions.
	/// </summary>
	/// <typeparam name="TContext">Class that derives from DbContext</typeparam>
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
		/// </summary>
		/// <example>
		/// _repository.RemoveItem{TEntity}(x => x.Id == 222);
		/// </example>
		/// <typeparam name="TEntity">Entity that exists in a DbSet</typeparam>
		/// <param name="searchExpression">Lambda expression</param>
		/// <returns></returns>
		Task RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class;

		/// <summary>
		/// Removes all TEntities that match the provided queryOperation from the DbSet matching type parameter
		/// </summary>
		/// <example>
		/// _repository.RemoveItems{TEntity}(query => query.Where(x => x.Age == 5));
		/// </example>
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
		/// </summary>
		/// <example>
		/// _repository.GetAllItems{TEntity}(query => query.Where(x=> x.Age == 4))
		/// </example>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryOperation">Optional IQueryable expression</param>
		/// <returns>List{TEntity}</returns>
		Task<List<TEntity>> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class;

		/// <summary>
		/// Returns T from DbSet matching type parameter.
		/// <para>Used to retrieve a specific column</para>
		/// </summary>
		/// <example>
		/// _repository.GetAllForColumn{TEntity, string}(q => q.Select(x => x.Comment))
		/// </example>
		/// <typeparam name="TEntity">TEntity that is in a DbSet</typeparam>
		/// <typeparam name="T">Expected return type</typeparam>
		/// <param name="queryOperation">IQueryable expression</param>
		/// <returns>List{T}</returns>
		Task<List<T>> GetAllForColumn<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class;

		/// <summary>
		/// Changes TEntity reference and its' collections EntityState to Modified
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item">Tracked TEntity</param>
		/// <returns></returns>
		Task UpdateItem<TEntity>(TEntity item) where TEntity : class;

		/// <summary>
		/// Changes TEntity references and their collections EntityState to Modified
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="items">List of tracked TEntities</param>
		/// <returns></returns>
		Task UpdateItems<TEntity>(List<TEntity> items) where TEntity : class;
	}
}

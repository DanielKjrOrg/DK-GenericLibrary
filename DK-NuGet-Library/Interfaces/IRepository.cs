using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DK.GenericLibrary.Interfaces
{
	/// <summary>
	/// Injectable generic interface that provides basic CRUD functionality and universal functions.
	/// </summary>
	/// <typeparam name="TContext">Class that derives from DbContext</typeparam>
	public interface IRepository<TContext> where TContext : DbContext
	{
		/// <summary>
		/// Adds the TEntity to DbSet matching the type parameter
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		void AddItem<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Adds the TEntities to the DbSet matching the type parameter
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entities"></param>
		/// <returns></returns>
		void AddItems<TEntity>(List<TEntity> entities) where TEntity : class;

		/// <summary>
		/// Removes a TEntity from the DbSet matching type parameter
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		void RemoveItem<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Removes a TEntity matching the provided searchExpression from the DbSet matching type parameter
		/// </summary>
		/// <example>
		/// _repository.RemoveItem{TEntity}(x => x.Id == 222);
		/// </example>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="searchExpression"></param>
		/// <returns></returns>
		void RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class;

		/// <summary>
		/// Removes all TEntities that match the provided queryOperation from the DbSet matching type parameter
		/// </summary>
		/// <example>
		/// _repository.RemoveItems{TEntity}(query => query.Where(x => x.Age == 5));
		/// </example>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryOperation"></param>
		/// <returns></returns>
		void RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class;

		/// <summary>
		/// Removes the TEntities from the DbSet matching type parameters
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="itemsToRemove"></param>
		/// <returns></returns>
		void RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class;

		/// <summary>
		/// Returns the TEntity matching the queryOperation from the DbSet matching type parameters
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="queryOperation"></param>
		/// <returns>FirstOrDefault TEntity</returns>
		TEntity GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class;

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
		/// <param name="queryOperation"></param>
		/// <returns>List{TEntity}</returns>
		List<TEntity> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class;

		/// <summary>
		/// Returns T from DbSet matching type parameter.
		/// <para>Used to retrieve a specific column</para>
		/// </summary>
		/// <example>
		/// _repository.GetAllForColumn{TEntity, string}(q => q.Select(x => x.Comment))
		/// </example>
		/// <typeparam name="TEntity"></typeparam>
		/// <typeparam name="T"></typeparam>
		/// <param name="queryOperation"></param>
		/// <returns>List{T}</returns>
		List<T> GetAllForColumn<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class;

		/// <summary>
		/// Changes TEntity reference and its' collections EntityState to Modified
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="item"></param>
		/// <returns></returns>
		void UpdateItem<TEntity>(TEntity item) where TEntity : class;

		/// <summary>
		/// Changes TEntity references and their collections EntityState to Modified
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="items"></param>
		/// <returns></returns>
		void UpdateItems<TEntity>(List<TEntity> items) where TEntity : class;
	}
}

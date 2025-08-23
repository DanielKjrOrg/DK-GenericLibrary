using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DK.GenericLibrary
{

	/// <summary>
	/// Provides the implementation of the IRepository interface
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public class Repository<TContext>(IDbContextFactory<TContext> dbContextFactory) : IRepository<TContext> where TContext : DbContext
	{




		/// <inheritdoc/>
		public void AddItem<TEntity>(TEntity entity) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			context.Set<TEntity>().Add(entity);
			context.SaveChanges();
		}
		/// <inheritdoc/>
		public void AddItems<TEntity>(List<TEntity> entities) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			entities.ForEach(i => context.Set<TEntity>().Add(i));
			context.SaveChanges();
		}

		/// <inheritdoc/>
		public void RemoveItem<TEntity>(TEntity entity) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			context.Set<TEntity>().Remove(entity);
			context.SaveChanges();
		}
		/// <inheritdoc/>
		public void RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			TEntity entityToRemove = context.Set<TEntity>().Where(searchExpression).FirstOrDefault()!;
			if (entityToRemove != null)
			{
				context.Set<TEntity>().Remove(entityToRemove);
				context.SaveChanges();
			}
		}
		/// <inheritdoc/>
		public void RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			List<TEntity> entitiesToRemove = queryOperation(context.Set<TEntity>()).ToList();
			entitiesToRemove.ForEach(i => context.Set<TEntity>().Remove(i));
			context.SaveChanges();
		}
		/// <inheritdoc/>
		public void RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			foreach (TEntity item in itemsToRemove)
			{
				context.Set<TEntity>().Remove(item);
			}
			context.SaveChanges();
		}
		/// <inheritdoc/>
		public TEntity GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			return queryOperation(context.Set<TEntity>().AsNoTracking()).FirstOrDefault()!;
		}
		/// <inheritdoc/>
		public List<TEntity> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			return queryOperation != null
				? queryOperation(context.Set<TEntity>().AsNoTracking()).ToList()
				: context.Set<TEntity>().AsNoTracking().ToList();
		}
		/// <inheritdoc/>
		public List<T> GetAllItems<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class
		{
			using var context = dbContextFactory.CreateDbContext();
			return queryOperation(context.Set<TEntity>().AsNoTracking()).ToList();
		}
		/// <inheritdoc/>
		public List<T> GetAllItemsStruct<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : struct
		{
			using var context = dbContextFactory.CreateDbContext();
			return queryOperation(context.Set<TEntity>().AsNoTracking()).ToList();
		}
		/// <inheritdoc/>
		public void UpdateItem<TEntity>(TEntity item) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			context.Entry(item).State = EntityState.Modified;
			foreach (NavigationEntry navigationEntry in context.Entry(item).Navigations)
			{
				if (navigationEntry.Metadata.IsCollection &&
					navigationEntry.CurrentValue is IEnumerable<object> collection)
				{
					foreach (object entity in collection)
					{
						context.Entry(entity).State = EntityState.Modified;
					}
				}
			}
			context.SaveChanges();
		}

		/// <inheritdoc/>
		public void UpdateItems<TEntity>(List<TEntity> items) where TEntity : class
		{
			using var context = dbContextFactory.CreateDbContext();
			foreach (TEntity item in items)
			{
				context.Entry(item).State = EntityState.Modified;
				foreach (NavigationEntry navigationEntry in context.Entry(item).Navigations)
				{
					if (navigationEntry.Metadata.IsCollection &&
						navigationEntry.CurrentValue is IEnumerable<object> collection)
					{
						foreach (object entity in collection)
						{
							context.Entry(entity).State = EntityState.Modified;
						}
					}
				}
			}
			context.SaveChanges();
		}

	}
}
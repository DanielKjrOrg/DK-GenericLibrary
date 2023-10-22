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
	public class Repository<TContext> : IRepository<TContext> where TContext : DbContext
	{
		private readonly IDbContextFactory<TContext> _dbContextFactory;

		/// <summary>
		/// Uses IDbContextFactory to create context instances
		/// </summary>
		/// <param name="dbContextFactory"></param>
		public Repository(IDbContextFactory<TContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		/// <inheritdoc/>
		public void AddItem<TEntity>(TEntity entity) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Set<TEntity>().Add(entity);
				context.SaveChanges();
			}

		}
		/// <inheritdoc/>
		public void AddItems<TEntity>(List<TEntity> entities) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				entities.ForEach(i => context.Set<TEntity>().Add(i));
				context.SaveChanges();
			}

		}

		/// <inheritdoc/>
		public void RemoveItem<TEntity>(TEntity entity) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				context.Set<TEntity>().Remove(entity);
				context.SaveChanges();
			}

		}
		/// <inheritdoc/>
		public void RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				TEntity entityToRemove = context.Set<TEntity>().Where(searchExpression).FirstOrDefault()!;
				if (entityToRemove != null)
				{
					context.Set<TEntity>().Remove(entityToRemove);
					context.SaveChanges();
				}

			}

		}
		/// <inheritdoc/>
		public void RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				List<TEntity> entitiesToRemove = queryOperation(context.Set<TEntity>()).ToList();
				entitiesToRemove.ForEach(i => context.Set<TEntity>().Remove(i));
				context.SaveChanges();
			}

		}
		/// <inheritdoc/>
		public void RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				foreach (TEntity item in itemsToRemove)
				{
					context.Set<TEntity>().Remove(item);
				}
				context.SaveChanges();
			}
		}
		/// <inheritdoc/>
		public TEntity GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return queryOperation(context.Set<TEntity>()).FirstOrDefault()!;
			}
		}
		/// <inheritdoc/>
		public List<TEntity> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return queryOperation != null
					? queryOperation(context.Set<TEntity>()).ToList()
					: context.Set<TEntity>().ToList();
			}
		}
		/// <inheritdoc/>
		public List<T> GetAllForColumn<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return queryOperation(context.Set<TEntity>()).ToList();
			}
		}
		/// <inheritdoc/>
		public void UpdateItem<TEntity>(TEntity item) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
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
				context.SaveChanges();
			}
		}

		/// <inheritdoc/>
		public void UpdateItems<TEntity>(List<TEntity> items) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
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



		private void UpdateCollection<TEntity>(List<TEntity> oldList, List<TEntity> newList, Comparer<TEntity> comparer) where TEntity : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				DeleteOldEntries(context, oldList, newList, comparer);
				AddOrUpdateNewEntries(context, oldList, newList, comparer);
				context.SaveChanges();
			}
		}


		private static void DeleteOldEntries<T>(DbContext context, List<T> oldList, List<T> newList, Comparer<T> comparer) where T : class
		{
			// Delete entries from database that are not in the new collection
			foreach (T oldItem in oldList)
			{
				if (newList == null || !newList.Any(item => comparer.Compare(item, oldItem) == 0))
				{
					context.Set<T>().Remove(oldItem);
				}
			}
		}

		private static void AddOrUpdateNewEntries<T>(DbContext context, List<T> oldList, List<T> newList,
			Comparer<T> comparer) where T : class
		{
			if (newList == null)
			{
				return;
			}
			foreach (T newItem in newList)
			{
				T oldItem = oldList.SingleOrDefault(item => comparer.Compare(item, newItem) == 0)!;
				if (oldItem != null)
				{
					context.Set<T>().Entry(oldItem).CurrentValues.SetValues(newItem);
					context.Set<T>().Attach(oldItem).State = EntityState.Modified;
				}
				else
				{
					context.Set<T>().Add(newItem);
				}
			}
		}
	}
}
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DK_NuGet_Library.Interfaces;

namespace DK_NuGet_Library
{
	/// <summary>
	/// Provides the implementation of the IAsyncRepository interface
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public class AsyncRepository<TContext> : IAsyncRepository<TContext> where TContext : DbContext
	{
		private readonly IDbContextFactory<TContext> _dbContextFactory;

	
		/// <summary>
		/// Provides the implementation of IAsyncRepository
		/// </summary>
		/// <param name="dbContextFactory"></param>
		public AsyncRepository(IDbContextFactory<TContext> dbContextFactory)
		{
			_dbContextFactory = dbContextFactory;
		}

		/// <inheritdoc/>
		public async Task AddItem<TEntity>(TEntity entity) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				await context.Set<TEntity>().AddAsync(entity);
				await context.SaveChangesAsync();
			}
			
		}
		/// <inheritdoc/>
		public async Task AddItems<TEntity>(List<TEntity> entities) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				foreach (TEntity entity in entities)
				{
					await context.Set<TEntity>().AddAsync(entity);
				}
				await context.SaveChangesAsync();
			}

		}

		/// <inheritdoc/>
		public async Task RemoveItem<TEntity>(TEntity entity) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				context.Set<TEntity>().Remove(entity);
				await context.SaveChangesAsync();
			}

		}
		/// <inheritdoc/>
		public async Task RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				TEntity entityToRemove = context.Set<TEntity>().Where(searchExpression).FirstOrDefault()!;
				if (entityToRemove != null)
				{
					context.Set<TEntity>().Remove(entityToRemove);
					await context.SaveChangesAsync();
				}

			}

		}
		/// <inheritdoc/>
		public async Task RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				List<TEntity> entitiesToRemove = queryOperation(context.Set<TEntity>()).ToList();
				foreach (var entityToRemove in entitiesToRemove)
				{
					context.Set<TEntity>().Remove(entityToRemove);
				}
				await context.SaveChangesAsync();
			}

		}
		/// <inheritdoc/>
		public async Task RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				foreach (TEntity item in itemsToRemove)
				{
					context.Set<TEntity>().Remove(item);
				}
				await context.SaveChangesAsync();
			}
		}
		/// <inheritdoc/>
		public async Task<TEntity> GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				return await Task.FromResult(queryOperation(context.Set<TEntity>()).FirstOrDefault()!);
			}
		}
		/// <inheritdoc/>
		public async Task<List<TEntity>> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
			{
				return await Task.FromResult(queryOperation != null
					? queryOperation(context.Set<TEntity>()).ToList()
					: context.Set<TEntity>().ToList());
			}
		}
		/// <inheritdoc/>
		public async Task<List<T>> GetAllForColumn<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class
		{
			using (var context = _dbContextFactory.CreateDbContext())
			{
				return await Task.FromResult(queryOperation(context.Set<TEntity>()).ToList());
			}
		}
		/// <inheritdoc/>
		public async Task UpdateItem<TEntity>(TEntity item) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
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
				await context.SaveChangesAsync();
			}
		}

		/// <inheritdoc/>
		public async Task UpdateItems<TEntity>(List<TEntity> items) where TEntity : class
		{
			using (var context = await _dbContextFactory.CreateDbContextAsync())
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
				await context.SaveChangesAsync();
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

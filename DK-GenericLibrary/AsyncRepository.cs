using DK.GenericLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace DK.GenericLibrary
{
	/// <summary>
	/// Provides the implementation of the IAsyncRepository interface
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	/// <remarks>
	/// Uses IDbContextFactory to create context instances
	/// </remarks>
	/// <param name="dbContextFactory"></param>
	public class AsyncRepository<TContext>(IDbContextFactory<TContext> dbContextFactory) : IAsyncRepository<TContext> where TContext : DbContext
	{


		/// <inheritdoc/>
		public async Task AddItem<TEntity>(TEntity entity) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			context.Set<TEntity>().Add(entity);
			await context.SaveChangesAsync();

		}
		/// <inheritdoc/>
		public async Task AddItems<TEntity>(List<TEntity> entities) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			foreach (TEntity entity in entities)
			{
				context.Set<TEntity>().Add(entity);
			}
			await context.SaveChangesAsync();

		}

		/// <inheritdoc/>
		public async Task RemoveItem<TEntity>(TEntity entity) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			context.Set<TEntity>().Remove(entity);
			await context.SaveChangesAsync();

		}
		/// <inheritdoc/>
		public async Task RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			TEntity entityToRemove = context.Set<TEntity>().Where(searchExpression).FirstOrDefault()!;
			if (entityToRemove != null)
			{
				context.Set<TEntity>().Remove(entityToRemove);
				await context.SaveChangesAsync();
			}

		}
		/// <inheritdoc/>
		public async Task RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			List<TEntity> entitiesToRemove = queryOperation(context.Set<TEntity>()).ToList();
			foreach (var entityToRemove in entitiesToRemove)
			{
				context.Set<TEntity>().Remove(entityToRemove);
			}
			await context.SaveChangesAsync();

		}
		/// <inheritdoc/>
		public async Task RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			foreach (TEntity item in itemsToRemove)
			{
				context.Set<TEntity>().Remove(item);
			}
			await context.SaveChangesAsync();
		}
		/// <inheritdoc/>
		public async Task<TEntity> GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			return await Task.FromResult(queryOperation(context.Set<TEntity>().AsNoTracking()).FirstOrDefault()!);
		}
		/// <inheritdoc/>
		public async Task<List<TEntity>> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			return await Task.FromResult(queryOperation != null
				? queryOperation(context.Set<TEntity>().AsNoTracking()).ToList()
				: context.Set<TEntity>().AsNoTracking().ToList());

		}


		/// <inheritdoc/>
		public async Task<List<T>> GetAllItems<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class 
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
			return await Task.FromResult(queryOperation(context.Set<TEntity>().AsNoTracking()).ToList());
		}

		/// <inheritdoc/>
		public async Task UpdateItem<TEntity>(TEntity item) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
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



		/// <inheritdoc/>
		public async Task UpdateItems<TEntity>(List<TEntity> items) where TEntity : class
		{
			await using var context = await dbContextFactory.CreateDbContextAsync();
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
		}

	}
}

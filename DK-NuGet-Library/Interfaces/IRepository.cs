using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DK_NuGet_Library.Interfaces
{
	public interface IRepository<TContext> where TContext : DbContext
	{
		void AddItem<TEntity>(TEntity entity) where TEntity : class;
		void AddItems<TEntity>(List<TEntity> entities) where TEntity : class;
		void RemoveItem<TEntity>(TEntity entity) where TEntity : class;
		void RemoveItem<TEntity>(Expression<Func<TEntity, bool>> searchExpression) where TEntity : class;
		void RemoveItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class;
		void RemoveItems<TEntity>(List<TEntity> itemsToRemove) where TEntity : class;

		TEntity GetItem<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryOperation) where TEntity : class;
		List<TEntity> GetAllItems<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? queryOperation = null) where TEntity : class;
		List<T> GetAllForColumn<TEntity, T>(Func<IQueryable<TEntity>, IQueryable<T>> queryOperation) where TEntity : class where T : class;
		void UpdateItem<TEntity>(TEntity item) where TEntity : class;
		void UpdateItems<TEntity>(List<TEntity> items) where TEntity : class;
		}
}

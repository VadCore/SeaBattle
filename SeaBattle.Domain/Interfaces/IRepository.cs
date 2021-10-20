using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Interfaces
{
	public interface IRepository<TEntity> where TEntity : BaseEntity<TEntity>
	{
		public TEntity Add(TEntity entity);

		public void Add(IEnumerable<TEntity> entities);

		public TEntity GetById(int id);

		public IReadOnlyCollection<TEntity> GetAll();

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] includeStrings);
		//public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeStrings);

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

		public void Update(TEntity entity);
		public void Delete(TEntity entity);

		public void Delete(int id);

		public void SaveChanges();
	}
}

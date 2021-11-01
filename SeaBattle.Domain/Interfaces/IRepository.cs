using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SeaBattle.Domain.Interfaces
{
	public interface IRepository<TEntity> where TEntity : BaseEntity<TEntity>
	{
		public TEntity Add(TEntity entity);

		public void Add(IEnumerable<TEntity> entities);

		public TEntity GetById(int id, params string[] navigationTitles);

		public IReadOnlyCollection<TEntity> GetAll(params string[] navigationTitles);

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] navigationTitles);
		//public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] navigationTitles);

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params string[] navigationTitles);

		public void Update(TEntity entity);
		public void Delete(TEntity entity);

		public void Delete(int id);

		public void SaveChanges();
	}
}

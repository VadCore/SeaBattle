using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Interfaces
{
	public interface IRepository<TEntity> where TEntity : BaseEntity
	{
		public TEntity Add(TEntity entity);

		public TEntity GetById(int id);

		public IReadOnlyCollection<TEntity> GetAll();

		public TEntity FindFirst(Func<TEntity, bool> predicate);

		public IEnumerable<TEntity> FindAll(Func<TEntity, bool> predicate);

		public void Update(TEntity entity);
		public void Delete(TEntity entity);

		public void Delete(int id);

		public void Save();
	}
}

using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Repositories
{
	public class ORMRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
	{

		protected readonly ORMSet<TEntity> entities;
		protected readonly SeaBattleContext _context;

		public ORMRepository(SeaBattleContext context)
		{
			_context = context;
			entities = _context.Set<TEntity>();
		}

		public TEntity Add(TEntity entity)
		{
			entity.Id = nextId++;

			entities.Add(entity);

			return entity;
		}

		public TEntity GetById(int id)
		{
			var sdf = entities.FirstOrDefault(e => e.Id == id);
			return entities.FirstOrDefault(e => e.Id == id);
		}

		public IReadOnlyCollection<TEntity> GetAll()
		{
			return (IReadOnlyCollection<TEntity>)entities;
		}

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
		{
			return entities.FirstOrDefault(predicate.Compile());
		}

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
		{
			return entities.Where(predicate.Compile());
		}

		public void Update(TEntity entity)
		{
			entities[entities.IndexOf(GetById(entity.Id))] = entity;
		}

		public void Delete(TEntity entity)
		{
			entities.Remove(entity);
			entities[entities.IndexOf(GetById(entity.Id))] = entity;
		}

		public void Delete(int id)
		{
			entities.Remove(GetById(id));
		}

		private void ResetNextId()
		{
			if (entities.Count != 0)
			{
				nextId = entities.Max(e => e.Id) + 1;
			}
		}
	}
}

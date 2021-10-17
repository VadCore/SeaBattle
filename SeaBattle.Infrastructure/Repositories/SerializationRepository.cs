using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Repositories
{
	public class SerializationRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<TEntity>, new()
	{
		protected SerializationSet<TEntity> _entities;
		protected readonly IUnitOfWork _unitOfWork;

		public SerializationRepository(SeaBattleSerializationContext context)
		{
			_unitOfWork = (IUnitOfWork)context;
			_entities = context.Set<TEntity>();
		}

		public TEntity Add(TEntity entity)
		{
			_entities.Add(entity);

			return entity;
		}

		public void Add(IEnumerable<TEntity> entities)
		{
			foreach(var entity in entities)
            {
				_entities.Add(entity);

			}
		}

		public TEntity GetById(int id)
		{
			return _entities.FirstOrDefault(e => e.Id == id);
		}

		public IReadOnlyCollection<TEntity> GetAll()
		{
			return (IReadOnlyCollection<TEntity>)_entities;
		}

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
		{
			return _entities.FirstOrDefault(predicate.Compile());
		}

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
		{
			return _entities.Where(predicate.Compile());
		}

		public void Update(TEntity entity)
		{
			_entities[_entities.IndexOf(GetById(entity.Id))] = entity;
		}

		public void Delete(TEntity entity)
		{
			_entities.Remove(entity);
			_entities[_entities.IndexOf(GetById(entity.Id))] = entity;
		}

		public void Delete(int id)
		{
			_entities.Remove(GetById(id));
		}

		public void SaveChanges()
        {
			_unitOfWork.Commit();
        }
	}
}

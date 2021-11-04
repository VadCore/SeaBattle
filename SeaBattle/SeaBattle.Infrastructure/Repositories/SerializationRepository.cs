using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

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
			foreach (var entity in entities)
			{
				_entities.Add(entity);

			}
		}

		public TEntity GetById(int id, params string[] includeStrings)
		{
			return _entities.GetById(id);
		}

		public IReadOnlyCollection<TEntity> GetAll(params string[] includeStrings)
		{
			return (IReadOnlyCollection<TEntity>)_entities;
		}

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] includeStrings)
		{
			return _entities.FindFirst(predicate, includeStrings);
		}

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params string[] includeStrings)
		{
			return _entities.FindAll(predicate);
		}

		public void Update(TEntity entity)
		{
			_entities.Update(entity);
		}

		public void Delete(TEntity entity)
		{
			_entities.Delete(entity);
		}

		public void Delete(int id)
		{
			_entities.Delete(id);
		}

		public void SaveChanges()
		{
			_unitOfWork.Commit();
		}
	}
}

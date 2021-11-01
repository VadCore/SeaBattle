using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SeaBattle.Infrastructure.Serialization
{
	public class SerializationSet<TEntity> where TEntity : BaseEntity<TEntity>, new()
	{
		private int nextId = 1;

		public HashSet<TEntity> Entities => entities;

		private readonly HashSet<TEntity> entities = new HashSet<TEntity>();

		public SerializationSet(HashSet<TEntity> entities)
		{
			this.entities = entities;

			ResetNextId();
		}

		public SerializationSet()
		{
		}

		public IReadOnlyList<TEntity> AsReadOnly()
		{
			return entities.ToList().AsReadOnly();
		}

		public void Add(TEntity item)
		{
			if (nextId == 1 && entities.Count != 0)
			{
				ResetNextId();
			}

			item.Id = nextId++;

			entities.Add(item);
		}

		public void Delete(TEntity entity)
		{
			entities.Remove(entity);
		}

		public void Delete(int id)
		{
			entities.Remove(GetById(id));
		}

		public void Update(TEntity entity)
		{
			var oldEntity = GetById(entity.Id);

			oldEntity = entity;
		}

		public TEntity GetById(int id)
		{
			if (entities.TryGetValue(new TEntity { Id = id }, out TEntity enity))
			{
				return enity;
			}

			return null;
		}

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] includeStrings)
		{
			return entities.FirstOrDefault(predicate.Compile());
		}

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
		{
			return entities.Where(predicate.Compile());
		}

		public bool Remove(TEntity item)
		{
			return entities.Remove(item);
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

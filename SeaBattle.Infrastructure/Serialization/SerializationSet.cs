using SeaBattle.Domain.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Serialization
{
    public class SerializationSet<TEntity> : IList<TEntity> where TEntity : BaseEntity<TEntity>
    {
        private int nextId = 1;

        private List<TEntity> _entities = new List<TEntity>();

        public TEntity this[int index] { get => _entities[index]; set => _entities[index] = value; }

        public int Count => _entities.Count;

        public bool IsReadOnly => false;

        public IReadOnlyList<TEntity> AsReadOnly()
        {
            return _entities.AsReadOnly();
        }

        public void Add(TEntity item)
        {
            if (nextId == 1 && _entities.Count != 0)
            {
                ResetNextId();
            }

            item.Id = nextId++;

            _entities.Add(item);
        }

        public void Clear()
        {
            _entities.Clear();
        }

        public bool Contains(TEntity item)
        {
            return _entities.Contains(item);
        }

        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            _entities.CopyTo(array, arrayIndex);
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

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        public int IndexOf(TEntity item)
        {
            return _entities.IndexOf(item);
        }

        public void Insert(int index, TEntity item)
        {
            _entities.Insert(index, item);
        }

        public void Update(TEntity entity)
        {
            _entities[_entities.IndexOf(GetById(entity.Id))] = entity;
        }

        public TEntity GetById(int id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] includeStrings)
        {
            return _entities.FirstOrDefault(predicate.Compile());
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _entities.Where(predicate.Compile());
        }

        public bool Remove(TEntity item)
        {
            return _entities.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _entities.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        private void ResetNextId()
        {
            if (_entities.Count != 0)
            {
                nextId = _entities.Max(e => e.Id) + 1;
            }
        }
    }
}

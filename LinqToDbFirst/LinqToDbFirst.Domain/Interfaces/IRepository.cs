using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LinqToDbFirst.Domain.Interfaces
{
    public interface IRepository<T> where T : class, IEntity
    {
        Task<IEnumerable<T>> GetAll();
        public Task<IEnumerable<T>> Find(Expression<Func<T, Boolean>> predicate);
        Task Create(T entity);
        void Update(T entity);

        void Delete(T entity);

        public Task SaveChanges();
    }
}

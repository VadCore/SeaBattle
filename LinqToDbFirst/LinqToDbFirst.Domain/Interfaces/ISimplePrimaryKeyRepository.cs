using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Domain.Interfaces
{
    public interface ISimplePrimaryKeyRepository<T> : IRepository<T> where T : class, ISimplePrimaryKeyEntity
    {
        Task<T> GetById(int id);
        Task Delete(int id);
    }
} 
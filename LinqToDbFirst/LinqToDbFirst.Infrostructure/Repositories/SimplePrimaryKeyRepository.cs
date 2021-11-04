using LinqToDbFirst.Domain.Interfaces;
using LinqToDbFirst.Infrostructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Infrostructure.Repositories
{
    public abstract class SimplePrimaryKeyRepository<T> : Repository<T>, ISimplePrimaryKeyRepository<T> where T : class, ISimplePrimaryKeyEntity
    {

        public SimplePrimaryKeyRepository(AdventureWorksLT2019Context context): base(context) { }

        public async Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
                entities.Remove(entity);
        }
    }
}

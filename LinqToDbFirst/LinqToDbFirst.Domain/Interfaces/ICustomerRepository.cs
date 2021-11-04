using LinqToDbFirst.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Domain.Interfaces
{
    public interface ICustomerRepository : ISimplePrimaryKeyRepository<Customer>
    {

    }
}

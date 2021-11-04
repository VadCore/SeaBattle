using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.Infrostructure.Repositories
{
    public class CustomerRepository : SimplePrimaryKeyRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AdventureWorksLT2019Context context) : base(context){}
    }
}

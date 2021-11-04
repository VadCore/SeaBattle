using LinqToDbFirst.Domain.Entities;
using LinqToDbFirst.Application.Services.Interfaces;
using LinqToDbFirst.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LinqToDbFirst.Domain.DTOs;

namespace LinqToDbFirst.Application.Services
{
    public class CustomerService : Service, ICustomerService
    {
        private ICustomerRepository _customers;

        public CustomerService(ICustomerRepository customers, IMapper mapper) : base(mapper)
        {
            _customers = customers;
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = await _customers.GetAll();

            return _mapper.Map<IEnumerable<Customer>, List<CustomerDTO>>(customers);
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllProductsWithTotalQtyAndTotalCost()
        {
            var customers = await _customers.GetAll();

            return _mapper.Map<IEnumerable<Customer>, List<CustomerDTO>>(customers);
        }
    }
}

using LinqToDbFirst.Domain.DTOs;
using LinqToDbFirst.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LinqToDbFirst.WebApi.Controllers
{
    public class CustomerController : BaseController
    {

        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllCustomers()
        {
            return OkOrNotFound(await _customerService.GetAllCustomers());
        }
    }
}

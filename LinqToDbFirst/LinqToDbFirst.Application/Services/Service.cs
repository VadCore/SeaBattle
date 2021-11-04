using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqToDbFirst.Application.Services
{
    public abstract class Service
    {
        protected readonly IMapper _mapper;

        protected Service(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}

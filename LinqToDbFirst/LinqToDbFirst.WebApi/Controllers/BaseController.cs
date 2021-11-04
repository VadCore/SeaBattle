using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LinqToDbFirst.WebApi.Controllers
{
    [ApiController]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {

        protected ActionResult OkOrNotFound<T>(T dto)
        {
            if (dto is null
                || typeof(IEnumerable<object>).IsAssignableFrom(typeof(T))
                    && ((IEnumerable<dynamic>)dto).Count() == 0)
            {
                return NotFound();
            }

            return Ok(dto);
        }
    }
}

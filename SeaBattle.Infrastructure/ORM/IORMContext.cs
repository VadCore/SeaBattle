using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public interface IORMContext
    {
        public SqlConnection Connection { get; }
        public SqlTransaction Transaction { get; }

    }
}

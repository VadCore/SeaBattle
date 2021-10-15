using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public class ORMSet<TEntity> where TEntity : BaseEntity
    {
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public ORMSet(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }
    }
}

using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SeaBattle.Infrastructure.ORM
{
    public abstract class ORMContext<TContext> where TContext : ORMContext<TContext>
    {
        private string _connectionString;
        public SqlConnection Connection { get; }
        public SqlTransaction Transaction { get; }


        private static readonly FieldInfo[] fieldInfos = GetFieldInfos();
        private readonly IDictionary<Type, FieldInfo> entityTypeFieldInfo = GetEntityTypeFieldInfos();

        public ORMContext(string connectionString)
        {
            _connectionString = connectionString;

            Connection = new SqlConnection(_connectionString);

            Transaction = Connection.BeginTransaction();

            foreach (var field in fieldInfos)
            {
                if (field.GetValue(this) is null)
                {
                    field.SetValue(this, Activator.CreateInstance(field.FieldType, Connection, Transaction));
                }
            }
        }

        public DataSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            var field = entityTypeFieldInfo[typeof(TEntity)];

            if (field.GetValue(this) is null)
            {
                field.SetValue(this, Activator.CreateInstance(field.FieldType, Connection, Transaction));
            }

            return (DataSet<TEntity>)entityTypeFieldInfo[typeof(TEntity)].GetValue(this);
        }

        private static FieldInfo[] GetFieldInfos()
        {
            return typeof(TContext).GetFields(ReflectionConstants.PublicInstance).ToArray();
        }

        private static IDictionary<Type, FieldInfo> GetEntityTypeFieldInfos()
        {
            var collectionEntitiesByEntityType = new Dictionary<Type, FieldInfo>();

            foreach (var field in fieldInfos)
            {
                var type = field.FieldType.GetGenericArguments().FirstOrDefault();
                collectionEntitiesByEntityType.Add(type, field);
            }

            return collectionEntitiesByEntityType;
        }

        public void SaveChanges()
        {
            Transaction.Commit();
        }
    }
}

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
    public abstract class ORMContext<TContext> : IORMContext where TContext : ORMContext<TContext>
    {
        private string _connectionString;
        public SqlConnection Connection { get; }
        public SqlTransaction Transaction => transaction ??= Connection.BeginTransaction();
        private SqlTransaction transaction;

        private static readonly PropertyInfo[] propertyInfos = GetPropertyInfos();
        private readonly IDictionary<Type, PropertyInfo> entityTypePropertyInfos = GetEntityTypePropertyInfos();


        static ORMContext()
        {
            GetPropertyInfos();
        }

        public ORMContext(string connectionString)
        {
            _connectionString = connectionString;

            Connection = new SqlConnection(_connectionString);

            Connection.Open();

            //transaction = Connection.BeginTransaction();
        }

        public ORMSet<TEntity> Set<TEntity>() where TEntity : BaseEntity<TEntity>, new()
        {
            var property = entityTypePropertyInfos[typeof(TEntity)];

            if (property.GetValue(this) is null)
            {
                property.SetValue(this, Activator.CreateInstance(property.PropertyType, new object[] { this, property.Name }));
            }

            return (ORMSet<TEntity>)entityTypePropertyInfos[typeof(TEntity)].GetValue(this);
        }

        private static PropertyInfo[] GetPropertyInfos()
        {

            return typeof(TContext).GetProperties(ReflectionConstants.PublicInstance)
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(ORMSet<>)).ToArray();
        }

        private static IDictionary<Type, PropertyInfo> GetEntityTypePropertyInfos()
        {
            var collectionEntitiesByEntityType = new Dictionary<Type, PropertyInfo>();

            foreach (var property in propertyInfos)
            {
                var type = property.PropertyType.GetGenericArguments().FirstOrDefault();
                collectionEntitiesByEntityType.Add(type, property);
            }

            return collectionEntitiesByEntityType;
        }

        public void Commit()
        {
            try
            {
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            finally
            {
                transaction.Dispose();
                transaction = null;
            }
        }
    }
}

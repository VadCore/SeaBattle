using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace SeaBattle.Infrastructure.ORM
{
    public abstract class ORMContext<TContext> : IORMContext where TContext : ORMContext<TContext>
    {
        private string _connectionString;
        public SqlConnection Connection { get; }

        public IDictionary<string, string> NavigationTableTitleByNavigationIncludes => navigationTableTitleByIncludes;
        protected static IDictionary<string, string> navigationTableTitleByIncludes = new Dictionary<string, string>();
        public IDictionary<string, string> FkBindingByNavigationIncludes => fkBindingByNavigationIncludes;
        protected static IDictionary<string, string> fkBindingByNavigationIncludes = new Dictionary<string, string>();

        public SqlTransaction Transaction => transaction ??= Connection.BeginTransaction();
        private SqlTransaction transaction;

        private static PropertyInfo[] propertyInfos;
        private readonly IDictionary<Type, PropertyInfo> ormSetPropertyInfoByEntityTypes = CreateOrmSetPropertyInfoByEntityTypes();


        public IDictionary<Type, string> DbTableTitleByEntityTypes => dbTableTitleByEntityTypes;
        private static IDictionary<Type, string> dbTableTitleByEntityTypes = new Dictionary<Type, string>();

        public IDictionary<Type, MethodInfo> CreateEntityByEntityTypes => createEntityByEntityTypes;
        private static IDictionary<Type, MethodInfo> createEntityByEntityTypes = new Dictionary<Type, MethodInfo>();


        static ORMContext()
        {
            SetupPropertyInfos();
        }

        public ORMContext(string connectionString)
        {
            _connectionString = connectionString;

            Connection = new SqlConnection(_connectionString);

            Connection.Open();

            //transaction = Connection.BeginTransaction();
        }

        protected Configurator<TEntity> Configure<TEntity>()
        {
            return new Configurator<TEntity>(this);
        }

        public ORMSet<TEntity> Set<TEntity>() where TEntity : BaseEntity<TEntity>, new()
        {
            var property = ormSetPropertyInfoByEntityTypes[typeof(TEntity)];

            if (property.GetValue(this) is null)
            {
                property.SetValue(this, Activator.CreateInstance(property.PropertyType, new object[] { this, property.Name }));
            }

            return (ORMSet<TEntity>)ormSetPropertyInfoByEntityTypes[typeof(TEntity)].GetValue(this);
        }

        private static void SetupPropertyInfos()
        {

            propertyInfos = typeof(TContext).GetProperties(ReflectionConstants.PublicInstance)
                .Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(ORMSet<>)).ToArray();

            foreach(var property in propertyInfos)
            {
                var type = property.PropertyType.GetGenericArguments().FirstOrDefault();
                dbTableTitleByEntityTypes.Add(type, property.Name);

                var methodCreateEntity = property.PropertyType.GetMethod("CreateEntity");
                createEntityByEntityTypes.Add(type, methodCreateEntity);
            }
        }


        private static IDictionary<Type, PropertyInfo> CreateOrmSetPropertyInfoByEntityTypes()
        {
            var ormSetPropertyInfoByEntityTypes = new Dictionary<Type, PropertyInfo>();

            foreach (var property in propertyInfos)
            {
                var type = property.PropertyType.GetGenericArguments().FirstOrDefault();
                ormSetPropertyInfoByEntityTypes.Add(type, property);
            }

            return ormSetPropertyInfoByEntityTypes;
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

        public void Dispose()
        {
            transaction.Rollback();
            transaction.Dispose();
            Connection.Close();
        }
    }
}

using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.ORM.NavigationConfigurator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SeaBattle.Infrastructure.ORM
{
	public abstract class ORMContext<TContext> : IORMContext where TContext : ORMContext<TContext>
	{
		private readonly string _connectionString;
		public SqlConnection Connection { get; }
		public SqlTransaction Transaction => transaction ??= Connection.BeginTransaction();
		private SqlTransaction transaction;

		private static PropertyInfo[] propertyInfos;
		public IDictionary<Type, PropertyInfo> OrmSetPropertyInfoByEntityTypes => ormSetPropertyInfoByEntityTypes;
		private static readonly IDictionary<Type, PropertyInfo> ormSetPropertyInfoByEntityTypes = new Dictionary<Type, PropertyInfo>();
		public IDictionary<Type, string> DbTableTitleByEntityTypes => dbTableTitleByEntityTypes;
		private static readonly IDictionary<Type, string> dbTableTitleByEntityTypes = new Dictionary<Type, string>();
		public IDictionary<Type, MethodInfo> CreateEntityByEntityTypes => createEntityByEntityTypes;
		private static readonly IDictionary<Type, MethodInfo> createEntityByEntityTypes = new Dictionary<Type, MethodInfo>();
		public IDictionary<string, Navigation> NavigationByTitles => navigationByTitles;
		private static readonly IDictionary<string, Navigation> navigationByTitles = new Dictionary<string, Navigation>();


		private static bool isConfiguredNavigations = false;

		static ORMContext()
		{
			SetupPropertyInfos();
			
		}

		public ORMContext(string connectionString)
		{

			_connectionString = connectionString;

			Connection = new SqlConnection(_connectionString);

			Connection.Open();

			foreach (var property in propertyInfos)
			{
				var type = property.PropertyType.GetGenericArguments().FirstOrDefault();

				property.SetValue(this, Activator.CreateInstance(property.PropertyType, new object[] { this, property.Name }));
			}

			if (!isConfiguredNavigations)
			{
				ConfigureNavigations();
				isConfiguredNavigations = true;
			}
		}

		protected Configurator<TEntity> Configure<TEntity>() where TEntity : BaseEntity<TEntity>, new()
		{
			return new Configurator<TEntity>(this);
		}

		public ORMSet<TEntity> GetORMSet<TEntity>() where TEntity : BaseEntity<TEntity>, new()
		{
			return (ORMSet<TEntity>)OrmSetPropertyInfoByEntityTypes[typeof(TEntity)].GetValue(this);
		}

		private static void SetupPropertyInfos()
		{
			propertyInfos = typeof(TContext).GetProperties(ReflectionConstants.PublicInstance)
				.Where(pi => pi.PropertyType.IsGenericType && pi.PropertyType.GetGenericTypeDefinition() == typeof(ORMSet<>)).ToArray();

			foreach (var property in propertyInfos)
			{
				var type = property.PropertyType.GetGenericArguments().FirstOrDefault();
				dbTableTitleByEntityTypes.Add(type, property.Name);

				var methodCreateEntity = property.PropertyType.GetMethod("CreateEntity");
				createEntityByEntityTypes.Add(type, methodCreateEntity);

				ormSetPropertyInfoByEntityTypes.Add(type, property);
			}
		}

		protected virtual void ConfigureNavigations()
        {

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
			transaction?.Rollback();
			transaction?.Dispose();
			Connection?.Close();
		}
	}
}

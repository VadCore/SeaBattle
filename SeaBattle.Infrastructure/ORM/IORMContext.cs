using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.ORM.NavigationConfigurator;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SeaBattle.Infrastructure.ORM
{
	public interface IORMContext : IDisposable
	{
		public SqlConnection Connection { get; }
		public SqlTransaction Transaction { get; }
		public IDictionary<Type, PropertyInfo> OrmSetPropertyInfoByEntityTypes { get; }
		public IDictionary<Type, MethodInfo> CreateEntityByEntityTypes { get; }
		public IDictionary<Type, string> DbTableTitleByEntityTypes { get; }
		public IDictionary<string, Navigation> NavigationByTitles { get; }

		public ORMSet<TEntity> GetORMSet<TEntity>() where TEntity : BaseEntity<TEntity>, new();
	}
}

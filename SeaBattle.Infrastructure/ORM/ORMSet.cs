using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.ORM.NavigationConfigurator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SeaBattle.Infrastructure.ORM
{
	public class ORMSet<TEntity> where TEntity : BaseEntity<TEntity>, new()
	{
		private readonly string _dbTableTitle;
		private static PropertyInfo[] noPKPropertyInfos;
		private static PropertyInfo[] propertyInfos;
		private static string[] noPKPropertyTitles;

		public IDictionary<string, PropertyInfo> PropertyInfoByPropertyTitles = propertyInfoByPropertyTitles;
		private static IDictionary<string, PropertyInfo> propertyInfoByPropertyTitles;

		public IDictionary<string, PropertyInfo> NavigationPropertyInfoByPropertyTitles = navigationPropertyInfoByPropertyTitles;
		private static IDictionary<string, PropertyInfo> navigationPropertyInfoByPropertyTitles;

		private static IDictionary<string, PropertyInfo> noPKPropertyInfoByPropertyTitles;

		private readonly SqlConnection _connection;

		private readonly IORMContext _oRMContext;

		public HashSet<TEntity> entities = new();

		static ORMSet()
		{
			SetupPropertyInfos();
		}

		public ORMSet(IORMContext ormContext, string dbTableTitle)
		{
			_connection = ormContext.Connection;
			_oRMContext = ormContext;
			_dbTableTitle = dbTableTitle;
		}

		public TEntity Add(TEntity entity)
		{
			string sqlExpression =
				$"INSERT INTO {_dbTableTitle} ({string.Join(", ", noPKPropertyTitles)}) " +
				$"VALUES ({string.Join(", ", noPKPropertyInfos.Select(pi => SqlConverter.ConvertToDbValue(pi.GetValue(entity))))}); " +
				$"SELECT CAST(scope_identity() AS int)";

			entity.Id = ExecuteScalar<int>(sqlExpression);

			return entity;
		}

		public void Add(IEnumerable<TEntity> entities)
		{
			var values = entities.Select(e => $"({string.Join(", ", noPKPropertyInfos.Select(pi => SqlConverter.ConvertToDbValue(pi.GetValue(e))))})");

			string sqlExpression =
				$"INSERT INTO {_dbTableTitle} ({string.Join(", ", noPKPropertyTitles)}) " +
				$"VALUES {string.Join(", ", values)}";

			ExecuteNonQuery(sqlExpression);
		}

		public TEntity GetById(int id, params string[] navigationTitles)
		{
			var navigations = navigationTitles?.Select(nt => _oRMContext.NavigationByTitles[nt]);

			var joinString = string.Join(" ", navigations?.Select(n =>
			$"LEFT JOIN {n.NavigationEntityDBTableTitle} " +
			$"ON {n.PkFkBinding.Text} "));

			string sqlExpression =
				$"SELECT * " +
				$"FROM {_dbTableTitle} " +
				$"{joinString}" +
				$"WHERE Id = {id}";

			return ExecuteReaderEntity(sqlExpression, navigations?.ToList());
		}

		public IReadOnlyCollection<TEntity> GetAll(params string[] navigationTitles)
		{
			var navigations = navigationTitles?.Select(nt => _oRMContext.NavigationByTitles[nt]);

			var joinString = string.Join(" ", navigations?.Select(n =>
			$"LEFT JOIN {n.NavigationEntityDBTableTitle} " +
			$"ON {n.PkFkBinding.Text} "));

			string sqlExpression =
				$"SELECT * " +
				$"FROM {_dbTableTitle}" +
				$"{joinString}";

			return ExecuteReaderCollectionEntities(sqlExpression, navigations?.ToList());
		}

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] navigationTitles)
		{
			var sqlConverter = new SqlConverter(_oRMContext);

			var navigations = navigationTitles?.Select(nt => _oRMContext.NavigationByTitles[nt]);

			var joinString = string.Join(" ", navigations?.Select(n =>
			$"LEFT JOIN {n.NavigationEntityDBTableTitle} " +
			$"ON {n.PkFkBinding.Text} "));

			string sqlExpression =
				$"SELECT * " +
				$"FROM {_dbTableTitle} " +
				$"{joinString}" +
				$"WHERE {sqlConverter.ConvertFromExpression(predicate)}";

			return ExecuteReaderEntity(sqlExpression, navigations?.ToList());
		}

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params string[] navigationTitles)
		{
			var sqlConverter = new SqlConverter(_oRMContext);

			var navigations = navigationTitles?.Select(nt => _oRMContext.NavigationByTitles[nt]);

			var joinString = string.Join(" ", navigations?.Select(n =>
			$"LEFT JOIN {n.NavigationEntityDBTableTitle} " +
			$"ON {n.PkFkBinding.Text} "));

			string sqlExpression =
				$"SELECT * " +
				$"FROM {_dbTableTitle} " +
				$"{joinString}" +
				$"WHERE {sqlConverter.ConvertFromExpression(predicate)}";

			return ExecuteReaderCollectionEntities(sqlExpression, navigations?.ToList());
		}

		public void Update(TEntity entity)
		{
			string sqlExpression =
				$"UPDATE {_dbTableTitle} " +
				$"SET {string.Join(", ", noPKPropertyInfoByPropertyTitles.Select(pi => $"{pi.Key} = {SqlConverter.ConvertToDbValue(pi.Value.GetValue(entity))}"))} " +
				$"WHERE Id = {entity.Id}";

			ExecuteNonQuery(sqlExpression);
		}

		public void Delete(TEntity entity)
		{
			Delete(entity.Id);
		}

		public void Delete(int id)
		{
			string sqlExpression =
				$"DELETE " +
				$"FROM {_dbTableTitle} " +
				$"WHERE Id = {id}";

			ExecuteNonQuery(sqlExpression);
		}

		public void ExecuteNonQuery(string sqlExpression)
		{
			var command = new SqlCommand(sqlExpression, _connection);
			command.Transaction = _oRMContext.Transaction;

			command.ExecuteNonQuery();
		}

		public TEntity ExecuteReaderEntity(string sqlExpression, IList<Navigation> navigations = null)
		{
			return ExecuteReaderCollectionEntities(sqlExpression, navigations)?.FirstOrDefault();
		}

		public IReadOnlyCollection<TEntity> ExecuteReaderCollectionEntities(string sqlExpression, IList<Navigation> navigations = null)
		{
			var enities = new List<TEntity>();

			var command = new SqlCommand(sqlExpression, _connection);
			command.Transaction = _oRMContext.Transaction;

			using var reader = command.ExecuteReader();

			while (reader.Read())
			{
				enities.Add(CreateEntity(reader, 0, navigations));
			}

			return enities;
		}

		public TEntity CreateEntity(SqlDataReader reader, int fieldIndex, IList<Navigation> navigations = default, int navigationIndex = -1, object previosJoinedEntity = default)
		{
			//create entity
			var entity = new TEntity();

			for (int lim = fieldIndex + propertyInfos.Length; fieldIndex < lim; fieldIndex++)
			{
				var propertyInfo = propertyInfoByPropertyTitles[reader.GetName(fieldIndex)];

				propertyInfo.SetValue(entity, SqlConverter.ConvertFromDbValue(reader.GetValue(fieldIndex), propertyInfo.PropertyType));
			}

			if (entity.Id == 0)
			{
				return null;
			}

			//add entity into hashset
			if (!entities.Add(entity))
			{
				entity = entities.First(e => e.Id == entity.Id);
			}

			//join with previous
			if (previosJoinedEntity is not null)
			{
				var previosNavigation = navigations[navigationIndex];

				if (previosNavigation is INavigationOneToMany)
				{
					var navigationCollectionValue =
						(IList<TEntity>)previosNavigation.NavigationPropertyInfo.GetValue(previosJoinedEntity);

					if (!navigationCollectionValue.Any(e => e == entity))
					{
						navigationCollectionValue.Add(entity);
					}

					previosNavigation.ReverseNavigation.NavigationPropertyInfo.SetValue(entity, previosJoinedEntity);
				}
			}

			while (++navigationIndex < navigations?.Count 
				  && navigations[navigationIndex].EntityType == typeof(TEntity))
			{
				//recursion CreateEntity
				var nextNavigation = navigations[navigationIndex];

				var navigationEntityORMSet = 
					_oRMContext.OrmSetPropertyInfoByEntityTypes[nextNavigation.NavigationEntityType].GetValue(_oRMContext);

				var navigationEntity = _oRMContext.CreateEntityByEntityTypes[nextNavigation.NavigationEntityType]
							.Invoke(navigationEntityORMSet, new object[] { reader, fieldIndex, navigations, navigationIndex, entity });

				fieldIndex += nextNavigation.NavigationEntityTypePropertyInfosCount;

				//join with next
				if (navigationEntity is not null)
				{
					if (nextNavigation is INavigationOneToOne)
					{
						nextNavigation.NavigationPropertyInfo.SetValue(entity, navigationEntity);
						nextNavigation.ReverseNavigation.NavigationPropertyInfo.SetValue(previosJoinedEntity, entity);
					}
					else if (nextNavigation is INavigationManyToOne)
					{
						nextNavigation.NavigationPropertyInfo.SetValue(entity, navigationEntity);

						var nextNavigationCollectionValue =
							(IList<TEntity>)nextNavigation.ReverseNavigation.NavigationPropertyInfo.GetValue(navigationEntity);

						if (!nextNavigationCollectionValue.Any(e => e == entity))
						{
							nextNavigationCollectionValue.Add(entity);
						}
					}
				}
			}

			return entity;
		}

		public TResult ExecuteScalar<TResult>(string sqlExpression)
		{
			var command = new SqlCommand(sqlExpression, _connection);
			command.Transaction = _oRMContext.Transaction;

			return (TResult)command.ExecuteScalar();
		}

		public static void SetupPropertyInfos()
		{
			propertyInfos = typeof(TEntity).GetProperties(ReflectionConstants.PublicInstance).ToArray();

			var navigationPropertyInfos = propertyInfos.Where(pi => IsBaseEntity(pi.PropertyType)
						  || (pi.PropertyType.IsGenericType
							&& pi.PropertyType.GenericTypeArguments.Any(a => IsBaseEntity(a)))).ToArray();

			propertyInfos = propertyInfos.Except(navigationPropertyInfos).ToArray();

			navigationPropertyInfoByPropertyTitles = navigationPropertyInfos.ToDictionary(pi => pi.Name, pi => pi);

			noPKPropertyInfos = propertyInfos.Where(pi => pi.Name != "Id").ToArray();

			noPKPropertyTitles = noPKPropertyInfos.Select(pi => pi.Name).ToArray();

			propertyInfoByPropertyTitles = propertyInfos.ToDictionary(pi => pi.Name, pi => pi);

			noPKPropertyInfoByPropertyTitles = noPKPropertyInfos.ToDictionary(pi => pi.Name, pi => pi);
		}

		private static bool IsBaseEntity(Type type)
		{
			do
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(BaseEntity<>))
				{
					return true;
				}
			}
			while ((type = type.BaseType) != null);

			return false;
		}
	}
}

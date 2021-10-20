using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public class ORMSet<TEntity> where TEntity : BaseEntity<TEntity>, new()
    {

        private string _dbTableTitle;
        private static PropertyInfo[] noPKPropertyInfos;
        private static PropertyInfo[] propertyInfos;
        private static string[] noPKPropertyTitles;

        private static IDictionary<string, PropertyInfo> propertyInfoByPropertyTitles;

        private static IDictionary<string, PropertyInfo> noPKPropertyInfoByPropertyTitles;

        private SqlConnection _connection;

        private IORMContext _oRMContext;

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

        public TEntity GetById(int id)
        {
            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle} " +
                $"WHERE Id = {id}";

            return ExecuteReaderEntity(sqlExpression);
        }

        public IReadOnlyCollection<TEntity> GetAll()
        {
            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle}";

            return ExecuteReaderCollectionEntities(sqlExpression);
        }

        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] includeStrings) // "Board.CoordinateShips"
        {
            var sqlConverter = new SqlConverter(_oRMContext);

            var joinString = string.Join(" ", includeStrings.Select(i=> $"JOIN {_oRMContext.NavigationTableTitleByNavigationIncludes[i]} " +
                                                                        $"ON {_oRMContext.FkBindingByNavigationIncludes[i]} "));
           
            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle} " +
                $"{joinString}" +
                $"WHERE {sqlConverter.ConvertFromExpression(predicate)}";

            return ExecuteReaderEntity(sqlExpression);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlConverter = new SqlConverter(_oRMContext);

            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle} " +
                $"WHERE {sqlConverter.ConvertFromExpression(predicate)}";

            return ExecuteReaderCollectionEntities(sqlExpression);
        }

        public void Update(TEntity entity)
        {
            string sqlExpression =
                $"UPDATE {_dbTableTitle} " +
                $"SET {string.Join(", ", noPKPropertyInfoByPropertyTitles.Select(pi => $"{pi.Key} = {SqlConverter.ConvertToDbValue(pi.Value.GetValue(entity))}" ))} " +
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

        public IReadOnlyCollection<TEntity> ExecuteReaderCollectionEntities(string sqlExpression)
        {
            var enities = new List<TEntity>();

            var command = new SqlCommand(sqlExpression, _connection);
            command.Transaction = _oRMContext.Transaction;

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    enities.Add(CreateEntity(reader, 0));
                }
            }

            return enities;
        }

        public TEntity ExecuteReaderEntity(string sqlExpression)
        {
            var command = new SqlCommand(sqlExpression, _connection);
            command.Transaction = _oRMContext.Transaction;

            using var reader = command.ExecuteReader();

            var records = new List<(Board board, CoordinateShip coordinateShip)>();

            var entities = new Dictionary<TEntity, List<CoordinateShip>>();

            TEntity entity = null;

            while (reader.Read())
            {
                var fieldIndex = 0;

                entity = CreateEntity(reader, fieldIndex);
                fieldIndex += propertyInfos.Length;

                //if( entities.TryGetValue(entity, out coordinateShips)



                if (reader.FieldCount > fieldIndex)
                {
                    var coordinateShip = (CoordinateShip)_oRMContext.CreateEntityByEntityTypes[typeof(CoordinateShip)]
                                                    .Invoke(null, new object[] { reader, fieldIndex });

                    var board = (Board)Convert.ChangeType(entity, typeof(Board));
                    records.Add((board, coordinateShip));
                }
            }
            if (records.Count > 1)
            {
                var lookup = records.ToLookup(r => r.board, r => r.coordinateShip);

                var lookthis = lookup.ToDictionary(g => g.Key, g=> g.Key);

                var entites = new List<Board>();


                foreach(var group in lookup)
                {
                    var entity2 = group.Key;
                    entity2.CoordinateShips = group.ToList();

                    entites.Add(entity2);
                }

                var stop = true;
            }

            return entity;
        }

        public static TEntity CreateEntity(SqlDataReader reader, int fieldIndex)
        {
            var entity = new TEntity();

            for (int i = fieldIndex, iLim = i + propertyInfos.Length; i < iLim; i++)
            {
                var propertyInfo = propertyInfoByPropertyTitles[reader.GetName(i)];

                propertyInfo.SetValue(entity, SqlConverter.ConvertFromDbValue(reader.GetValue(i), propertyInfo.PropertyType));
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
            propertyInfos = typeof(TEntity).GetProperties(ReflectionConstants.PublicInstance)
                .Where(pi => !IsBaseEntity(pi.PropertyType)
                          && !(pi.PropertyType.IsGenericType
                            && pi.PropertyType.GenericTypeArguments.Any(a => IsBaseEntity(a))))
                .ToArray();

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

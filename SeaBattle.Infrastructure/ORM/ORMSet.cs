using Microsoft.Data.SqlClient;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public class ORMSet<TEntity> where TEntity : BaseEntity, new()
    {

        private string _dbTableTitle;
        private static PropertyInfo[] noPKPropertyInfos;
        private static PropertyInfo[] propertyInfos;
        private static string[] noPKPropertyTitles;

        private static IDictionary<string, PropertyInfo> propertyInfoByPropertyTitles;

        private static IDictionary<string, PropertyInfo> noPKPropertyInfoByPropertyTitles;

        private SqlConnection _connection;
        private SqlTransaction _transaction;

        static ORMSet()
        {
            SetupPropertyInfos();
        }

        public ORMSet(SqlConnection connection, SqlTransaction transaction, string dbTableTitle)
        {
            _connection = connection;
            _transaction = transaction;
            _dbTableTitle = dbTableTitle;
        }

        public TEntity Add(TEntity entity)
        {
            string sqlExpression =
                $"INSERT INTO {_dbTableTitle} ({string.Join(", ", noPKPropertyTitles)})" +
                $"VALUES({string.Join(", ", noPKPropertyInfos.Select(pi => pi.GetValue(entity)))});" +
                $"SELECT CAST(scope_identity() AS int)";

            entity.Id = ExecuteScalar<int>(sqlExpression);

            return entity;
        }

        public TEntity GetById(int id)
        {
            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle}" +
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


        public void Update(TEntity entity)
        {
            string sqlExpression =
                $"UPDATE {_dbTableTitle}" +
                $"SET {string.Join(", ", noPKPropertyInfoByPropertyTitles.Select(pi => $"{pi.Key} = {pi.Value.GetValue(entity)}" ))} " +
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
                $"FROM {_dbTableTitle}" +
                $"WHERE Id = {id}";

            ExecuteNonQuery(sqlExpression);
        }


        public void ExecuteNonQuery(string sqlExpression)
        {
            var command = new SqlCommand(sqlExpression, _connection);

            command.ExecuteNonQuery();
        }


        public IReadOnlyCollection<TEntity> ExecuteReaderCollectionEntities(string sqlExpression)
        {
            var enities = new List<TEntity>();

            var command = new SqlCommand(sqlExpression, _connection);

            using var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    enities.Add(CreateEntity(reader));
                }
            }

            return enities;
        }

        public TEntity ExecuteReaderEntity(string sqlExpression)
        {
            var command = new SqlCommand(sqlExpression, _connection);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return CreateEntity(reader);
            }

            return null;
        }

        private static TEntity CreateEntity(SqlDataReader reader)
        {
            var entity = new TEntity();

            for (int i = 0; i < reader.FieldCount; i++)
            {

                var propertyInfo = propertyInfoByPropertyTitles[reader.GetName(i)];

                propertyInfo.SetValue(entity, Convert.ChangeType(reader.GetValue(i), propertyInfo.PropertyType));
            }

            return entity;
        }

        public TResult ExecuteScalar<TResult>(string sqlExpression)
        {
            var command = new SqlCommand(sqlExpression, _connection);
            command.Transaction = _transaction;

            return (TResult)command.ExecuteScalar();
        }

        public static void SetupPropertyInfos()
        {
            propertyInfos = typeof(TEntity).GetProperties(ReflectionConstants.PublicInstance).ToArray();

            noPKPropertyInfos = propertyInfos.Where(pi => pi.Name != "Id").ToArray();

            noPKPropertyTitles = noPKPropertyInfos.Select(pi => pi.Name).ToArray();

            propertyInfoByPropertyTitles = propertyInfos.ToDictionary(pi => pi.Name, pi => pi);

            noPKPropertyInfoByPropertyTitles = noPKPropertyInfos.ToDictionary(pi => pi.Name, pi => pi);
        }


    }
}

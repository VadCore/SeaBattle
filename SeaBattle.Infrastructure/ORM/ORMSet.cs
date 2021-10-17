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
                $"VALUES ({string.Join(", ", noPKPropertyInfos.Select(pi => ConvertToDbValue(pi.GetValue(entity))))}); " +
                $"SELECT CAST(scope_identity() AS int)";

            entity.Id = ExecuteScalar<int>(sqlExpression);

            return entity;
        }

        public void Add(IEnumerable<TEntity> entities)
        {
            var values = entities.Select(e => $"({string.Join(", ", noPKPropertyInfos.Select(pi => ConvertToDbValue(pi.GetValue(e))))})");

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


        private class ExpressionToSqlConverter : ExpressionVisitor
        {
            private readonly StringBuilder buffer = new();

            public string Convert<T>(Expression<T> expression)
            {
                Visit(expression);

                return buffer.ToString();
            }

            public override Expression Visit(Expression node)
            {
                return base.Visit(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if(node.Expression.NodeType == ExpressionType.Parameter)
                {
                    buffer.Append(node.Member.Name);
                    return node;
                }
                else if (node.Expression.NodeType == ExpressionType.Constant)
                {
                    var constantExpression = ((ConstantExpression)(node.Expression)).Value;

                    var constant = node.Member.MemberType switch
                    {
                        MemberTypes.Field => ((FieldInfo)node.Member).GetValue(constantExpression),
                        MemberTypes.Property => ((PropertyInfo)node.Member).GetValue(constantExpression),
                        _ => throw new ArgumentException("Unknown members type for constants!")
                    };

                    buffer.Append(ConvertToDbValue(constant));
                    return node;
                }
                else if (node.Expression.NodeType == ExpressionType.MemberAccess
                    && node.Expression is MemberExpression embeddedNode
                    && embeddedNode.Expression.NodeType == ExpressionType.Constant)
                {


                    var constantExpression = ((ConstantExpression)(embeddedNode.Expression)).Value;

                    var embeddedConstant = embeddedNode.Member.MemberType switch
                    {
                        MemberTypes.Field => ((FieldInfo)embeddedNode.Member).GetValue(constantExpression),
                        MemberTypes.Property => ((PropertyInfo)embeddedNode.Member).GetValue(constantExpression),
                        _ => throw new ArgumentException("Unknown members type for constants!")
                    };

                    var constant = node.Member.MemberType switch
                    {
                        MemberTypes.Field => ((FieldInfo)node.Member).GetValue(embeddedConstant),
                        MemberTypes.Property => ((PropertyInfo)node.Member).GetValue(embeddedConstant),
                        _ => throw new ArgumentException("Unknown members type for constants!")
                    };

                    buffer.Append(ConvertToDbValue(constant));
                    return node;
                }

                return base.VisitMember(node);
            }

            protected override Expression VisitBinary(BinaryExpression binary)
            {
                buffer.Append('(');
                Visit(binary.Left);

                var operatorTitle = binary.NodeType switch
                {
                    ExpressionType.AndAlso => "AND",
                    ExpressionType.OrElse => "OR",
                    ExpressionType.Equal => "=",
                    ExpressionType.NotEqual => "!=",
                    _ => throw new ArgumentException("Unknown binary operator for ExpressionVisitor!")
                };

                buffer.Append(" " + operatorTitle + " ");

                Visit(binary.Right);
                buffer.Append(')');

                return binary;
            }
        }



        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlConverter = new ExpressionToSqlConverter();

            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle} " +
                $"WHERE {sqlConverter.Convert(predicate)}";

            return ExecuteReaderEntity(sqlExpression);
        }


        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            var sqlConverter = new ExpressionToSqlConverter();

            string sqlExpression =
                $"SELECT * " +
                $"FROM {_dbTableTitle} " +
                $"WHERE {sqlConverter.Convert(predicate)}";

            return ExecuteReaderCollectionEntities(sqlExpression);
        }










        public void Update(TEntity entity)
        {
            string sqlExpression =
                $"UPDATE {_dbTableTitle} " +
                $"SET {string.Join(", ", noPKPropertyInfoByPropertyTitles.Select(pi => $"{pi.Key} = {ConvertToDbValue(pi.Value.GetValue(entity))}" ))} " +
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
                    enities.Add(CreateEntity(reader));
                }
            }

            return enities;
        }

        public TEntity ExecuteReaderEntity(string sqlExpression)
        {
            var command = new SqlCommand(sqlExpression, _connection);
            command.Transaction = _oRMContext.Transaction;

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

                propertyInfo.SetValue(entity, ConvertFromDbValue(reader.GetValue(i), propertyInfo.PropertyType));
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

        public static string ConvertToDbValue(object @object)
        {
            if(@object is null)
            {
                return "NULL";
            }
            else if (@object is string)
            {
                return $"'{@object}'";
            }
            else if(@object is Enum @enum)
            {
                return ((int)@enum.GetTypeCode()).ToString();
            }

            return @object.ToString();
        }

        public static object ConvertFromDbValue(object @object, Type toType)
        {
            if (@object is DBNull)
            {
                return null;
            }
            else if (toType.IsEnum)
            {
                return Enum.ToObject(toType, Convert.ChangeType(@object, typeof(int)));
            }
            else if (toType == typeof(int?))
            {
                return (int?)Convert.ChangeType(@object, typeof(int));
            }

            return Convert.ChangeType(@object, toType);
        }
    }
}

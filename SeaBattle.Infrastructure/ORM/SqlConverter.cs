using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public class SqlConverter : ExpressionVisitor
    {
        private readonly StringBuilder buffer = new();

        private readonly IORMContext _ormContext;
        private readonly bool withTableTitles;

        public SqlConverter(IORMContext ormContext, bool withTableTitles = true)
        {
            _ormContext = ormContext;
            this.withTableTitles = withTableTitles;
        }

        public string ConvertFromExpression<T>(Expression<T> expression)
        {
            Visit(expression);

            return buffer.ToString();
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

        protected override Expression VisitMember(MemberExpression node)
        {

            //Algorith for visit
            // var coordinateShip = new CoordinateShip();
            //_.coordinateShip.Ship.Id  (MemberExpression) = _.coordinateShip.Ship (Expression) + Id (Member)
            //_.coordinateShip.Ship (MemberExpression) = _.coordinateShip (Expression) + ship (Member);
            //_.coordinateShip (MemberExpression) = _ (Expression) + CoordinateShip(Member)
            //_ (ConstantExpression)
            //var constant = coordinateShip;
            //constant = constant.Ship;
            //constant = constant.Id;



            if (node.Expression.NodeType == ExpressionType.Parameter)
            {
                var prefix = withTableTitles ? _ormContext.DbTableTitleByEntityTypes[node.Expression.Type] + "." : string.Empty;

                buffer.Append(prefix + node.Member.Name);
            }
            else if (TryGetConstant(node, out object constant))
            {
                buffer.Append(ConvertToDbValue(constant));
            }

            return node;
        }

        private bool TryGetConstant(MemberExpression node, out object @object)
        {
           
            if (node.Expression is MemberExpression embeddedNode)
            {
                if(!TryGetConstant(embeddedNode, out @object))
                {
                    return false;
                }
            }
            else if (node.Expression.NodeType == ExpressionType.Constant)
            {
                @object = ((ConstantExpression)(node.Expression)).Value;
            }
            else
            {
                @object = null;

                return false;
            }

            @object = node.Member.MemberType switch
            {
                MemberTypes.Field => ((FieldInfo)node.Member).GetValue(@object),
                MemberTypes.Property => ((PropertyInfo)node.Member).GetValue(@object),
                _ => throw new ArgumentException("Unknown members type for constants!")
            };
            
            return true;
        }

        public static string ConvertToDbValue(object @object)
        {
            if (@object is null)
            {
                return "NULL";
            }
            else if (@object is string)
            {
                return $"'{@object}'";
            }
            else if (@object is Enum @enum)
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

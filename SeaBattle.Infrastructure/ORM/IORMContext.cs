using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public interface IORMContext : IDisposable
    {
        public SqlConnection Connection { get; }
        public SqlTransaction Transaction { get; }

        public IDictionary<string, string> NavigationTableTitleByNavigationIncludes { get; }
        public IDictionary<string, string> FkBindingByNavigationIncludes { get; }

        public IDictionary<Type, MethodInfo> CreateEntityByEntityTypes { get; }
        public IDictionary<Type, string> DbTableTitleByEntityTypes { get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public class NavigationConfigurator<TEntity, TNavigation>
    {
        private readonly string navigationInclude;

        private readonly IORMContext _ormContext;

        public NavigationConfigurator(string navigationInclude, IORMContext ormContext)
        {
            this.navigationInclude = navigationInclude;
            _ormContext = ormContext;
        }

        public void ByForeignKey<TForeignKey>(Expression<Func<TEntity, TForeignKey>> foreignkey)
        {
            var foreignkeyTitle = new SqlConverter(_ormContext, false).ConvertFromExpression(foreignkey);

            var primaryKey = _ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)] + ".Id";
            var foreignKey = _ormContext.DbTableTitleByEntityTypes[typeof(TEntity)] + "." + foreignkeyTitle;

            var fkBinding = primaryKey + " = " + foreignKey;

            _ormContext.FkBindingByNavigationIncludes.Add(navigationInclude, fkBinding);
        }

        public void ByForeignKey<TForeignKey>(Expression<Func<TNavigation, TForeignKey>> foreignkey)
        {
            var foreignkeyTitle = new SqlConverter(_ormContext, false).ConvertFromExpression(foreignkey);

            var primaryKey = _ormContext.DbTableTitleByEntityTypes[typeof(TEntity)] + ".Id";
            var foreignKey = _ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)] + "." + foreignkeyTitle;

            var fkBinding = primaryKey + " = " + foreignKey;

            _ormContext.FkBindingByNavigationIncludes.Add(navigationInclude, fkBinding);
        }
    }
}

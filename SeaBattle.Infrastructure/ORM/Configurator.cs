using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.ORM
{
    public class Configurator<TEntity>
    {

        private readonly IORMContext _ormContext;

        public Configurator(IORMContext ormContext)
        {
            _ormContext = ormContext;
        }

        public NavigationConfigurator<TEntity, TNavigation> WithNavigation<TNavigation>(Expression<Func<TEntity, TNavigation>> navigation)
            where TNavigation : BaseEntity<TNavigation>
        {
            var navigationTitle = new SqlConverter(_ormContext, false).ConvertFromExpression(navigation);
            var navigationInclude = typeof(TEntity).Name + "." + navigationTitle;
            var navigationTableTitle = _ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)];

            _ormContext.NavigationTableTitleByNavigationIncludes.Add(navigationInclude, navigationTableTitle);

            return new NavigationConfigurator<TEntity, TNavigation>(navigationInclude, _ormContext);
        }

        public NavigationConfigurator<TEntity, TNavigation> WithNavigation<TNavigation>(Expression<Func<TEntity, IEnumerable<TNavigation>>> navigations)
        {
            var navigationTitle = new SqlConverter(_ormContext, false).ConvertFromExpression(navigations);
            var navigationInclude = typeof(TEntity).Name + "." + navigationTitle;
            var navigationTableTitle = _ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)];

            _ormContext.NavigationTableTitleByNavigationIncludes.Add(navigationInclude, navigationTableTitle);

            return new NavigationConfigurator<TEntity, TNavigation>(navigationInclude, _ormContext);
        }
    }
}

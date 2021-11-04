using System;
using System.Linq.Expressions;

namespace SeaBattle.Infrastructure.ORM.NavigationConfigurator
{
	public class NavigationConfigurator<TEntity, TNavigation>
	{
		private readonly IORMContext _ormContext;
		private readonly Navigation _navigation;
		private readonly Navigation _reverseNavigation;

		public NavigationConfigurator(IORMContext ormContext, Navigation navigation, Navigation reverseNavigation)
		{
			_ormContext = ormContext;
			_navigation = navigation;
			_reverseNavigation = reverseNavigation;
		}

		public void ByForeignKey<TForeignKey>(Expression<Func<TEntity, TForeignKey>> foreignkey)
		{
			var foreignKeyTitle = new SqlConverter(_ormContext, false).ConvertFromExpression(foreignkey);

			var primaryKey = _ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)] + ".Id";
			var foreignKey = _ormContext.DbTableTitleByEntityTypes[typeof(TEntity)] + "." + foreignKeyTitle;

			var fkBinding = primaryKey + " = " + foreignKey;

			_navigation.PkFkBinding = _reverseNavigation.PkFkBinding = new FKBinding(fkBinding);
		}

		public void ByForeignKey<TForeignKey>(Expression<Func<TNavigation, TForeignKey>> foreignkey)
		{
			var foreignKeyTitle = new SqlConverter(_ormContext, false).ConvertFromExpression(foreignkey);

			var primaryKey = _ormContext.DbTableTitleByEntityTypes[typeof(TEntity)] + ".Id";
			var foreignKey = _ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)] + "." + foreignKeyTitle;

			var fkBinding = primaryKey + " = " + foreignKey;

			_navigation.PkFkBinding = _reverseNavigation.PkFkBinding = new FKBinding(fkBinding);
		}
	}
}

using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SeaBattle.Infrastructure.ORM.NavigationConfigurator
{
	public class NavigationOneToMany<TEntity, TNavigation> : Navigation, INavigationOneToMany
		where TEntity : BaseEntity<TEntity>, new()
		where TNavigation : BaseEntity<TNavigation>, new()
	{
		public Expression<Func<TEntity, IEnumerable<TNavigation>>> NavigationKey { get; set; }

		public NavigationOneToMany(IORMContext ormContext,
								   Expression<Func<TEntity, IEnumerable<TNavigation>>> navigationKey)
			: base(typeof(TEntity), typeof(TNavigation))
		{
			var navigationFieldTitle = new SqlConverter(ormContext, false).ConvertFromExpression(navigationKey);
			Title = typeof(TEntity).Name + "." + navigationFieldTitle;

			NavigationKey = navigationKey;
			var rty = ormContext.GetORMSet<TEntity>();
			NavigationPropertyInfo = ormContext.GetORMSet<TEntity>().NavigationPropertyInfoByPropertyTitles[navigationFieldTitle];

			NavigationEntityTypePropertyInfosCount = ormContext.GetORMSet<TNavigation>().PropertyInfoByPropertyTitles.Count;

			NavigationEntityDBTableTitle = ormContext.DbTableTitleByEntityTypes[typeof(TNavigation)];

			ormContext.NavigationByTitles.Add(Title, this);
		}

	}

	public interface INavigationOneToMany { }
}

using SeaBattle.Domain.Entities;
using System;
using System.Linq.Expressions;

namespace SeaBattle.Infrastructure.ORM.NavigationConfigurator
{
	public class NavigationOneToOne<TEntity, TNavigation> : Navigation, INavigationOneToOne
		where TEntity : BaseEntity<TEntity>, new()
		where TNavigation : BaseEntity<TNavigation>, new()
	{
		public Expression<Func<TEntity, TNavigation>> NavigationKey { get; set; }

		public NavigationOneToOne(IORMContext ormContext,
								   Expression<Func<TEntity, TNavigation>> navigationKey)
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

	public interface INavigationOneToOne { }
}

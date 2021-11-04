using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SeaBattle.Infrastructure.ORM.NavigationConfigurator
{
	public class Configurator<TEntity>
		where TEntity : BaseEntity<TEntity>, new()
	{

		private readonly IORMContext _ormContext;

		public Configurator(IORMContext ormContext)
		{
			_ormContext = ormContext;
		}

		public NavigationConfigurator<TEntity, TNavigation> WithNavigation<TNavigation>(
			Expression<Func<TEntity, TNavigation>> navigationKey,
			Expression<Func<TNavigation, TEntity>> reverseNavigationKey)
			where TNavigation : BaseEntity<TNavigation>, new()
		{
			var navigation = new NavigationOneToOne<TEntity, TNavigation>(_ormContext, navigationKey);
			var reverseNavigation = new NavigationOneToOne<TNavigation, TEntity>(_ormContext, reverseNavigationKey);

			navigation.ReverseNavigation = reverseNavigation;
			reverseNavigation.ReverseNavigation = navigation;

			return new NavigationConfigurator<TEntity, TNavigation>(_ormContext, navigation, reverseNavigation);
		}

		public NavigationConfigurator<TEntity, TNavigation> WithNavigation<TNavigation>(
			Expression<Func<TEntity, IEnumerable<TNavigation>>> navigationKey,
			Expression<Func<TNavigation, TEntity>> reverseNavigationKey)
			where TNavigation : BaseEntity<TNavigation>, new()
		{
			var navigation = new NavigationOneToMany<TEntity, TNavigation>(_ormContext, navigationKey);
			var reverseNavigation = new NavigationManyToOne<TNavigation, TEntity>(_ormContext, reverseNavigationKey);

			navigation.ReverseNavigation = reverseNavigation;
			reverseNavigation.ReverseNavigation = navigation;

			return new NavigationConfigurator<TEntity, TNavigation>(_ormContext, navigation, reverseNavigation);
		}

		public NavigationConfigurator<TEntity, TNavigation> WithNavigation<TNavigation>(
			Expression<Func<TEntity, TNavigation>> navigationKey,
			Expression<Func<TNavigation, IEnumerable<TEntity>>> reverseNavigationKey)
			where TNavigation : BaseEntity<TNavigation>, new()
		{
			var navigation = new NavigationManyToOne<TEntity, TNavigation>(_ormContext, navigationKey);
			var reverseNavigation = new NavigationOneToMany<TNavigation, TEntity>(_ormContext, reverseNavigationKey);

			navigation.ReverseNavigation = reverseNavigation;
			reverseNavigation.ReverseNavigation = navigation;

			return new NavigationConfigurator<TEntity, TNavigation>(_ormContext, navigation, reverseNavigation);
		}
	}
}

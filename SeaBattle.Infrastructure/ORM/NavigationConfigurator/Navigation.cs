using System;
using System.Reflection;

namespace SeaBattle.Infrastructure.ORM.NavigationConfigurator
{
	public abstract class Navigation
	{
		public string Title { get; set; }
		public FKBinding PkFkBinding { get; set; }
		public Type EntityType { get; set; }
		public Type NavigationEntityType { get; set; }
		public string NavigationEntityDBTableTitle { get; set; }
		public PropertyInfo NavigationPropertyInfo { get; set; }
		public int NavigationEntityTypePropertyInfosCount { get; set; }

		public Navigation ReverseNavigation { get; set; }

		protected Navigation(Type entityType, Type navigationEntityType)
		{
			EntityType = entityType;
			NavigationEntityType = navigationEntityType;
		}
	}
}

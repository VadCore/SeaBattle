using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Common
{
	public abstract class DataContext<TContext> : IDataContext where TContext : DataContext<TContext>
	{
		private static readonly FieldInfo[] fieldInfos = GetFieldInfos();
		private static readonly IDictionary<Type, FieldInfo> entityTypeFieldInfo = GetEntityTypeFieldInfos();

		[JsonIgnore]
		public IDataHandler<TContext> DataHandler { get; set; }

		public DataContext(IDataHandler<TContext> dataHandler)
		{
			DataHandler = dataHandler;

			foreach (var field in fieldInfos)
			{
				if (field.GetValue(this) is null)
				{
					field.SetValue(this, Activator.CreateInstance(field.FieldType));
				}
			}
		}

		public DataContext()
		{
			foreach (var field in fieldInfos)
			{
				if (field.GetValue(this) is null)
				{
					field.SetValue(this, Activator.CreateInstance(field.FieldType));
				}
			}
		}

		public DataSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
		{
			return (DataSet<TEntity>)entityTypeFieldInfo[typeof(TEntity)].GetValue(this);
		}

		private static FieldInfo[] GetFieldInfos()
		{
			return typeof(TContext).GetFields(ReflectionConstants.PublicInstance).ToArray();
		}

		private static IDictionary<Type, FieldInfo> GetEntityTypeFieldInfos()
		{
			var collectionEntitiesByEntityType = new Dictionary<Type, FieldInfo>();

			foreach (var field in fieldInfos)
			{
				var type = field.FieldType.GetGenericArguments().FirstOrDefault();
				collectionEntitiesByEntityType.Add(type, field);
			}

			return collectionEntitiesByEntityType;
		}

		public void SaveChanges()
		{
			DataHandler.SaveContext((TContext)this);
		}
	}
}

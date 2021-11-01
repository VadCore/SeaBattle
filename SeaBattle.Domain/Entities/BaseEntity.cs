using SeaBattle.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SeaBattle.Domain.Entities
{
	public abstract class BaseEntity<TEntity> : IEquatable<TEntity> where TEntity : BaseEntity<TEntity>
	{
		public int Id { get; set; }

		private static readonly FieldInfo[] fieldInfos = GetFieldInfos();

		public bool Equals(TEntity other)
		{
			return Id == other?.Id;
		}

		public override bool Equals(object other)
		{
			return Equals(other as TEntity);
		}

		public static bool operator ==(BaseEntity<TEntity> left, TEntity right)
		{
			return (left is null == right is null) && (left is null || left.Equals(right));
		}

		public static bool operator !=(BaseEntity<TEntity> left, TEntity right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}


		private static FieldInfo[] GetFieldInfos()
		{
			var fields = typeof(BaseEntity<TEntity>)
							.GetFields(ReflectionConstants.PublicInstance)
							.Where(fi => !IsBaseEntity(fi.FieldType)
										&& !(fi.FieldType.IsGenericType
										&& fi.FieldType.GenericTypeArguments.Any(a => IsBaseEntity(a)))).ToArray();

			var entity = typeof(TEntity);

			while (entity != typeof(BaseEntity<TEntity>))
			{
				fields = fields.Concat(
					entity.GetFields(ReflectionConstants.PublicInstance)
						  .Where(fi => !IsBaseEntity(fi.FieldType)
									&& !(fi.FieldType.IsGenericType
									  && fi.FieldType.GenericTypeArguments.Any(a => IsBaseEntity(a))))).ToArray();

				entity = entity.BaseType;
			}

			return fields;
		}

		public override string ToString()
		{
			return typeof(TEntity).Name + ": " + ValuesToString(fieldInfos);
		}

		public string ValuesToString(IEnumerable<FieldInfo> fieldInfos)
		{
			return string.Join(", ", fieldInfos.Select(f =>
								f.Name[1..f.Name.IndexOf('>')] + " - " + f.GetValue(this)?.ToString()));
		}

		private static bool IsBaseEntity(Type type)
		{
			do
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(BaseEntity<>))
				{
					return true;
				}
			}
			while ((type = type.BaseType) != null);

			return false;
		}


	}
}

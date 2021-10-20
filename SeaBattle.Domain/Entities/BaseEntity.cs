using SeaBattle.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
	public abstract class BaseEntity<TEntity> : IEquatable<TEntity> where TEntity : BaseEntity<TEntity>
	{
		public int Id { get; set; }

		private static readonly FieldInfo[] fieldInfos = GetFieldInfos();

		public bool Equals(TEntity other)
		{
			return Id == other.Id;
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
			var fields = typeof(BaseEntity<TEntity>).GetFields(ReflectionConstants.PublicInstance);

			var entity = typeof(TEntity);

			while (entity != typeof(BaseEntity<TEntity>))
			{
				fields = fields.Concat(entity.GetFields(ReflectionConstants.PublicInstance)).ToArray();
				entity = entity.BaseType;
			}

			return fields;
		}

        public override string ToString() =>
			typeof(TEntity).Name + ": " + ValuesToString(fieldInfos);

		public string ValuesToString(IEnumerable<FieldInfo> fieldInfos) =>
			string.Join(", ", fieldInfos.Select(f =>
				f.Name[1..f.Name.IndexOf('>')] + " - " + f.GetValue(this)?.ToString()));

        
    }
}

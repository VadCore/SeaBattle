using SeaBattle.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Common
{
	public abstract class ValueObject<T> : IEquatable<T>
		where T : ValueObject<T>
	{
		private static readonly FieldInfo[] fieldInfos = GetFieldInfos();

		public virtual IEnumerable<object> GetEqualityAttributes()
		{
			foreach (var field in fieldInfos)
			{
				yield return field.GetValue(this);
			}
		}

		public bool IsEqualByAttributes(T other)
		{
			return GetEqualityAttributes().SequenceEqual(other.GetEqualityAttributes());
		}

		public bool Equals(T other)
		{
			return other != null && IsEqualByAttributes(other);
		}

		public override bool Equals(object other)
		{
			return Equals(other as T);
		}

		public static bool operator ==(ValueObject<T> left, T right)
		{
			return !(left is null ^ right is null) && (left is null || left.Equals(right));
		}

		public static bool operator !=(ValueObject<T> left, T right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GetEqualityAttributes());
		}

		private static FieldInfo[] GetFieldInfos()
		{
			return typeof(T).GetFields(ReflectionConstants.PublicInstance).ToArray();
		}
	}
}

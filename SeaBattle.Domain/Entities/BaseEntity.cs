using SeaBattle.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
	public abstract class BaseEntity<TEntity>
	{
		public int Id { get; set; }

		private static readonly FieldInfo[] fieldInfos = GetFieldInfos();

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

using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Interfaces
{
	public interface IDataContext
	{
		public DataSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;

		public void SaveChanges();
	}
}

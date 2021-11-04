using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;

namespace SeaBattle.Application.Services
{
	public abstract class BaseService<TEntity> where TEntity : BaseEntity<TEntity>
	{
		protected readonly IRepository<TEntity> _entities;

		public BaseService(IRepository<TEntity> entities)
		{
			_entities = entities;
		}
	}
}

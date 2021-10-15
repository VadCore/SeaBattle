using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Repositories
{
	public class ORMRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity, new()
	{

		protected readonly ORMSet<TEntity> entities;
		protected readonly IUnitOfWork _unitOfWork;


		public ORMRepository(SeaBattleContext context)
		{
			_unitOfWork = (IUnitOfWork)context;
			entities = context.Set<TEntity>();
		}

		public TEntity Add(TEntity entity)
		{
			return entities.Add(entity);
		}

		public TEntity GetById(int id)
		{
			return entities.GetById(id);
		}

		public IReadOnlyCollection<TEntity> GetAll()
		{
			return (IReadOnlyCollection<TEntity>)entities.GetAll();
		}

        public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate)
        {
			return null;

            //return entities.FirstOrDefault(predicate.Compile());
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
			return null;

			//return entities.Where(predicate.Compile());
        }

        public void Update(TEntity entity)
		{
			entities.Update(entity);
		}

		public void Delete(TEntity entity)
		{
			entities.Delete(entity);
		}

		public void Delete(int id)
		{
			entities.Delete(id);
		}

		

        public void SaveChanges()
        {
			_unitOfWork.Commit();
        }
    }
}

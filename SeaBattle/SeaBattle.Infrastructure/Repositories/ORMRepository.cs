﻿using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.ORM;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SeaBattle.Infrastructure.Repositories
{
	public class ORMRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity<TEntity>, new()
	{

		protected readonly ORMSet<TEntity> _entities;
		protected readonly IUnitOfWork _unitOfWork;


		public ORMRepository(SeaBattleORMContext context)
		{
			_unitOfWork = (IUnitOfWork)context;
			_entities = context.GetORMSet<TEntity>();
		}

		public TEntity Add(TEntity entity)
		{
			return _entities.Add(entity);
		}

		public void Add(IEnumerable<TEntity> entities)
		{
			_entities.Add(entities);
		}

		public TEntity GetById(int id, params string[] navigationTitles)
		{
			return _entities.GetById(id, navigationTitles);
		}

		public IReadOnlyCollection<TEntity> GetAll(params string[] navigationTitles)
		{
			return _entities.GetAll(navigationTitles);
		}

		public TEntity FindFirst(Expression<Func<TEntity, bool>> predicate, params string[] navigationTitles)
		{
			return _entities.FindFirst(predicate, navigationTitles);
		}

		public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, params string[] navigationTitles)
		{
			return _entities.FindAll(predicate, navigationTitles);
		}

		public void Update(TEntity entity)
		{
			_entities.Update(entity);
		}

		public void Delete(TEntity entity)
		{
			_entities.Delete(entity);
		}

		public void Delete(int id)
		{
			_entities.Delete(id);
		}



		public void SaveChanges()
		{
			_unitOfWork.Commit();
		}



	}
}
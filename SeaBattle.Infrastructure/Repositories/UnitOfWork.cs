using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly IDataContext _context;

		public IRepository<Board> Boards => _boards ??= new Repository<Board>(_context);
		private IRepository<Board> _boards;

		public IRepository<Player> Players => _players ??= new Repository<Player>(_context);
		private IRepository<Player> _players;

		public IRepository<Ship> Ships => _ships ??= new Repository<Ship>(_context);
		private IRepository<Ship> _ships;

		public IRepository<BattleAbility> BattleAbilities => _battleAbilities ??= new Repository<BattleAbility>(_context);
		private IRepository<BattleAbility> _battleAbilities;

		public IRepository<SupportAbility> SupportAbilities => _supportAbilities ??= new Repository<SupportAbility>(_context);
		private IRepository<SupportAbility> _supportAbilities;

		public IRepository<CoordinateShip> CoordinateShips => _coordinateships ??= new Repository<CoordinateShip>(_context);
		private IRepository<CoordinateShip> _coordinateships;

		public IRepository<Size> Sizes => _sizes ??= new Repository<Size>(_context);
		private IRepository<Size> _sizes;

		public UnitOfWork(IDataContext context)
		{
			_context = context;
		}

		public void Commit()
		{
			_context.SaveChanges();
		}
	}
}

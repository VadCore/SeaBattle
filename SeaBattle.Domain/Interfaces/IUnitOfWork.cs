using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Interfaces
{
	public interface IUnitOfWork
	{
		public IRepository<Board> Boards { get; }
		public IRepository<Player> Players { get; }
		public IRepository<Ship> Ships { get; }
		public IRepository<BattleAbility> BattleAbilities { get; }
		public IRepository<SupportAbility> SupportAbilities { get; }
		public IRepository<CoordinateShip> CoordinateShips { get; }
		public IRepository<Size> Sizes { get; }
		public void Commit();
	}
}

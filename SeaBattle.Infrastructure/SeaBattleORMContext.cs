using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.ORM;
using SeaBattle.UI.Configs;

namespace SeaBattle.Infrastructure
{
	public class SeaBattleORMContext : ORMContext<SeaBattleORMContext>, IUnitOfWork
	{

		public ORMSet<Board> Boards { get; set; }
		public ORMSet<Player> Players { get; set; }
		public ORMSet<Ship> Ships { get; set; }
		public ORMSet<BattleAbility> BattleAbilities { get; set; }
		public ORMSet<SupportAbility> SupportAbilities { get; set; }
		public ORMSet<CoordinateShip> CoordinateShips { get; set; }
		public ORMSet<Size> Sizes { get; set; }

		public SeaBattleORMContext(IAppOptions options) : base(options.DbConnectionString)
		{
			Configure<Board>().WithNavigation(b => b.CoordinateShips, cs => cs.Board)
							  .ByForeignKey(cs => cs.BoardId);

			Configure<Board>().WithNavigation(b => b.Players, cs => cs.Board)
							  .ByForeignKey(cs => cs.BoardId);

			Configure<CoordinateShip>().WithNavigation(cs => cs.Ship, s => s.CoordinateShips)
									   .ByForeignKey(cs => cs.ShipId);
		}
	}
}

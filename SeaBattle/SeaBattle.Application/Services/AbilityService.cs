using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;

namespace SeaBattle.Application.Services
{
	public abstract class AbilityService<TAbility> : BaseService<TAbility> where TAbility : Ability<TAbility>
	{
		protected readonly IRepository<Player> _players;
		protected readonly IRepository<Board> _boards;
		protected readonly IRepository<Size> _sizes;
		protected readonly IShipService _shipService;
		protected readonly IRepository<CoordinateShip> _coordinateShips;
		protected readonly IRepository<Ship> _ships;

		protected AbilityService(IRepository<TAbility> entities, IRepository<Player> players, IRepository<Board> boards, IRepository<Size> sizes, IShipService shipService, IRepository<Ship> ships, IRepository<CoordinateShip> coordinateShips) : base(entities)
		{
			_players = players;
			_boards = boards;
			_sizes = sizes;
			_shipService = shipService;
			_ships = ships;
			_coordinateShips = coordinateShips;
		}

		protected Ship GetTargetShip(Ship shipFrom, Coordinate coordinate)
		{
			var player = _players.GetById(shipFrom.PlayerId);
			var board = _boards.GetById(player.BoardId);
			var size = _sizes.GetById(shipFrom.SizeId);

			if (_shipService.CalculateDistanceFromNearestPoint(shipFrom, size, coordinate) > size.Range)
			{
				return null;
			}

			var shipId = _coordinateShips.FindFirst(cs => cs.BoardId == board.Id &&
													cs.Quadrant == coordinate.Quadrant
													&& cs.XAbs == coordinate.XAbs
													&& cs.YAbs == cs.YAbs).ShipId;

			if (shipId == null)
			{
				return null;
			}

			return _ships.GetById((int)shipId);
		}

		protected bool StartReloading(Ship ship, Ability<TAbility> ability)
		{
			var player = _players.GetById(ship.PlayerId);
			var board = _boards.GetById(player.BoardId);

			if (board.Turn >= ability.ReloadTurn)
			{
				var size = _sizes.GetById(ship.SizeId);
				ability.ReloadTurn = board.Turn + size.Reloading;
			}
			else
			{
				return false;
			}

			return true;
		}
	}
}

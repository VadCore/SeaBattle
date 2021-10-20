using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Enums;
using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services
{
	public class ShipService : BaseService<Ship>, IShipService
	{
		protected readonly IRepository<Player> _players;
		protected readonly IRepository<Board> _boards;
		protected readonly IRepository<Size> _sizes;
		protected readonly IRepository<CoordinateShip> _coordinateShips;
		protected readonly IRepository<SupportAbility> _supportAbilities;
		protected readonly IRepository<BattleAbility> _battleAbilities;

        public ShipService(IRepository<Player> players,
                           IRepository<Board> boards,
                           IRepository<Size> sizes,
                           IRepository<CoordinateShip> coordinateShips,
                           IRepository<Ship> ships, IRepository<SupportAbility> supportAbilities, IRepository<BattleAbility> battleAbilities) : base(ships)
        {
            _players = players;
            _boards = boards;
            _sizes = sizes;
            _coordinateShips = coordinateShips;
            _supportAbilities = supportAbilities;
            _battleAbilities = battleAbilities;
        }

        public Ship CreateBattle(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var ship = Create(sizeId, player, coordinate, rotation);

			if (ship is not null)
			{
				_battleAbilities.Add(new BattleAbility(ship.Id));
				_entities.SaveChanges();
			}


			return ship;
		}

		public Ship CreateSupport(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var ship = Create(sizeId, player, coordinate, rotation);

			if (ship is not null)
			{
				_supportAbilities.Add(new SupportAbility(ship.Id));
			}

			_entities.SaveChanges();

			return ship;
		}

		public Ship CreateUniversal(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var ship = Create(sizeId, player, coordinate, rotation);

			if (ship is not null)
			{
				_battleAbilities.Add(new BattleAbility(ship.Id));
				_supportAbilities.Add(new SupportAbility(ship.Id));
			}

			_entities.SaveChanges();

			return ship;
		}

		private Ship Create(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{

			var size = _sizes.GetById((int)sizeId);
			var board = _boards.GetById(player.BoardId);

			var ship = new Ship((int)sizeId, player.Id, size.HealthMax);

			if (Allocate(ship, size.Length, board, coordinate, rotation, true))
			{
				ship.CenterCoordinateShipId = _coordinateShips.FindFirst(cs => cs.BoardId == board.Id
																&& cs.Quadrant == coordinate.Quadrant
																&& cs.XAbs == coordinate.XAbs
																&& cs.YAbs == coordinate.YAbs).Id;
				_entities.Add(ship);

				if (!Allocate(ship, size.Length, board, coordinate, rotation))
				{
					_entities.Delete(ship.Id);
					return null;
				}

				return ship;
			}

			return null;
		}


		private bool Allocate(Ship ship, int length, Board board, Coordinate to, Rotation targetRotation, bool isOnlyCheck = false)
		{
			if (to.XAbs + (targetRotation == Rotation.Horizontal ? length / 2 : 0) > board.XAbsMax
			 || to.YAbs + (targetRotation == Rotation.Vertical ? length / 2 : 0) > board.YAbsMax)
			{
				return false;
			}

			var step = Vector2D.Create(targetRotation);

			var coordinate = to - (length / 2) * step;

			for (int i = 0; i < length; i++)
			{
				var coordinateShip = _coordinateShips.FindFirst(cs => cs.BoardId == board.Id
																&& cs.Quadrant == coordinate.Quadrant
																&& cs.XAbs == coordinate.XAbs
																&& cs.YAbs == coordinate.YAbs);
				if (coordinateShip.ShipId != null && coordinateShip.ShipId != ship.Id)
				{
					if(!isOnlyCheck)
                    {
                        for (int k = i - 1; k >= 0; k--)
                        {

                            coordinate -= (to.Quadrant == coordinate.Quadrant) ? step : -step;
                            coordinateShip = _coordinateShips.FindFirst(cs => cs.BoardId == board.Id
                                                                        && cs.Quadrant == coordinate.Quadrant
                                                                        && cs.XAbs == coordinate.XAbs
                                                                        && cs.YAbs == coordinate.YAbs);

                            coordinateShip.ShipId = null;

                            coordinateShip.Ship = null;

							_coordinateShips.Update(coordinateShip);
						}
                    }

					return false;
				}

				if (!isOnlyCheck)
				{
                    coordinateShip.ShipId = ship.Id;

                    coordinateShip.Ship = ship;

					_coordinateShips.Update(coordinateShip);
                }

				coordinate += (to.Quadrant == coordinate.Quadrant) ? step : -step;
			}
			if (!isOnlyCheck)
			{
                ship.Rotation = targetRotation;

                ship.CenterCoordinateShipId = _coordinateShips.FindFirst(cs => cs.BoardId == board.Id
                                                                            && cs.Quadrant == to.Quadrant
                                                                            && cs.XAbs == to.XAbs
                                                                            && cs.YAbs == to.YAbs).Id;

				_entities.Update(ship);
            }

			return true;
		}
		

		public void Dislocate(Ship ship, int length, Board board, Coordinate from)
		{
			var step = Vector2D.Create(ship.Rotation);

			var coordinate = from - (length / 2) * step;

			for (int i = length; i > 0; i--)
			{
				var coordinateShip = _coordinateShips.FindFirst(cs => cs.BoardId == board.Id
																   && cs.Quadrant == coordinate.Quadrant
																   && cs.XAbs == coordinate.XAbs
																   && cs.YAbs == coordinate.YAbs);

				coordinateShip.ShipId = null;

				coordinateShip.Ship = null;

				_coordinateShips.Update(coordinateShip);

				coordinate += (from.Quadrant == coordinate.Quadrant) ? step : -step;
			}
		}

		private void Dislocate(Ship ship, int length, Board board)
		{
			var centralCoordinate = _coordinateShips.GetById(ship.CenterCoordinateShipId).GetCoordinate();

			Dislocate(ship, length, board, centralCoordinate);
		}

		private void Dislocate(Ship ship)
		{
			var size = _sizes.GetById(ship.SizeId);
			var player = _players.GetById(ship.PlayerId);
			var board = _boards.GetById(player.BoardId);

			var centralCoordinate = _coordinateShips.GetById(ship.CenterCoordinateShipId).GetCoordinate();

			Dislocate(ship, size.Length, board, centralCoordinate);
		}

		public void Kill(Ship ship)
        {
			Dislocate(ship);

			_entities.SaveChanges();
        }

		public bool Relocate(Ship ship, Coordinate to, Rotation targetRotation)
		{
			var size = _sizes.GetById(ship.SizeId);
			var player = _players.GetById(ship.PlayerId);
			var board = _boards.GetById(player.BoardId);

			var centralCoordinate = _coordinateShips.GetById(ship.CenterCoordinateShipId).GetCoordinate();

			if (centralCoordinate.CalculateDistance(to) > size.Speed)
			{
				return false;
			}

			var currentRotation = ship.Rotation;

			Dislocate(ship, size.Length, board);

			if (!Allocate(ship, size.Length, board, to, targetRotation))
			{
				Allocate(ship, size.Length, board, centralCoordinate, currentRotation);

				return false;
			}

			_entities.SaveChanges();

			return true;
		}

		public bool Relocate(Ship ship, Coordinate to)
		{
			return Relocate(ship, to, ship.Rotation);
		}

		public bool Rotate(Ship ship, Rotation targetRotation)
		{
			var centralCoordinate = _coordinateShips.GetById(ship.CenterCoordinateShipId).GetCoordinate();

			return Relocate(ship, centralCoordinate, targetRotation);
		}

		public int CalculateDistanceFromNearestPoint(Ship ship, Size shipSize, Coordinate to)
		{

			var from = _coordinateShips.GetById(ship.CenterCoordinateShipId).GetCoordinate();

			var step = Vector2D.Create(ship.Rotation);



			from -= (shipSize.Length / 2) * step;

			int distanceMin = int.MaxValue;

			for (int i = shipSize.Length; i > 0; i--)
			{
				distanceMin = Math.Min(from.CalculateDistance(to), distanceMin);

				from += (to.Quadrant == from.Quadrant) ? step : -step;
			}

			return distanceMin;
		}

	}
}

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
		public ShipService(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public Ship CreateBattle(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var ship = Create(sizeId, player, coordinate, rotation);

			if (ship is not null)
			{
				_unitOfWork.BattleAbilities.Add(new BattleAbility(ship.Id));
				_unitOfWork.Commit();
			}


			return ship;
		}

		public Ship CreateSupport(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var ship = Create(sizeId, player, coordinate, rotation);

			if (ship is not null)
			{
				_unitOfWork.SupportAbilities.Add(new SupportAbility(ship.Id));
			}

			_unitOfWork.Commit();

			return ship;
		}

		public Ship CreateUniversal(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var ship = Create(sizeId, player, coordinate, rotation);

			if (ship is not null)
			{
				_unitOfWork.BattleAbilities.Add(new BattleAbility(ship.Id));
				_unitOfWork.SupportAbilities.Add(new SupportAbility(ship.Id));
			}

			_unitOfWork.Commit();

			return ship;
		}

		private Ship Create(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation)
		{
			var size = _unitOfWork.Sizes.GetById((int)sizeId);
			var ship = _unitOfWork.Ships.Add(new Ship((int)sizeId, player.Id, size.HealthMax));
			var board = _unitOfWork.Boards.GetById(player.BoardId);

			if (!Allocate(ship, size.Length, board, coordinate, rotation))
			{
				_unitOfWork.Ships.Delete(ship.Id);
				return null;
			}

			return ship;
		}



		private bool Allocate(Ship ship, int length, Board board, Coordinate to, Rotation targetRotation)
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
				var coordinateShip = _unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
																				&& cs.Coordinate == coordinate);
				if (coordinateShip.ShipId != 0 && coordinateShip.ShipId != ship.Id)
				{
					for (i--; i >= 0; i--)
					{
						coordinate -= step;
						coordinateShip = _unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
																 && cs.Coordinate == coordinate);

						coordinateShip.ShipId = 0;

						coordinateShip.Ship = null;
					}

					return false;
				}


				coordinateShip.ShipId = ship.Id;

				coordinateShip.Ship = ship;

				coordinate += step;
			}

			ship.Rotation = targetRotation;

			ship.CenterCoordinateShipId = _unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
																				&& cs.Coordinate == to).Id;

			return true;
		}

		public void Dislocate(Ship ship, int length, Board board, Coordinate from)
		{
			var step = Vector2D.Create(ship.Rotation);

			from -= (length / 2) * step;

			for (int i = length; i > 0; i--)
			{
				var cooridnateShip = _unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
														 && cs.Coordinate == from);

				cooridnateShip.ShipId = 0;

				cooridnateShip.Ship = null;

				from += step;
			}
		}

		public void Dislocate(Ship ship, int length, Board board)
		{
			var centralCoordinate = _unitOfWork.CoordinateShips.GetById(ship.CenterCoordinateShipId).Coordinate;

			Dislocate(ship, length, board, centralCoordinate);
		}

		public void Dislocate(Ship ship)
		{
			var size = _unitOfWork.Sizes.GetById(ship.SizeId);
			var player = _unitOfWork.Players.GetById(ship.PlayerId);
			var board = _unitOfWork.Boards.GetById(player.BoardId);

			var centralCoordinate = _unitOfWork.CoordinateShips.GetById(ship.CenterCoordinateShipId).Coordinate;

			Dislocate(ship, size.Length, board, centralCoordinate);
		}

		public bool Relocate(Ship ship, Coordinate to, Rotation targetRotation)
		{
			var size = _unitOfWork.Sizes.GetById(ship.SizeId);
			var player = _unitOfWork.Players.GetById(ship.PlayerId);
			var board = _unitOfWork.Boards.GetById(player.BoardId);

			var centralCoordinate = _unitOfWork.CoordinateShips.GetById(ship.CenterCoordinateShipId).Coordinate;

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

			return true;
		}

		public bool Relocate(Ship ship, Coordinate to)
		{
			return Relocate(ship, to, ship.Rotation);
		}

		public bool Rotate(Ship ship, Rotation targetRotation)
		{
			var centralCoordinate = _unitOfWork.CoordinateShips.GetById(ship.CenterCoordinateShipId).Coordinate;

			return Relocate(ship, centralCoordinate, targetRotation);
		}

		public int CalculateDistanceFromNearestPoint(Ship ship, Size shipSize, Coordinate to)
		{

			var from = _unitOfWork.CoordinateShips.GetById(ship.CenterCoordinateShipId).Coordinate;

			var step = Vector2D.Create(ship.Rotation);

			from -= (shipSize.Length / 2) * step;

			int distanceMin = int.MaxValue;

			for (int i = shipSize.Length; i > 0; i--)
			{
				distanceMin = Math.Min(from.CalculateDistance(to), distanceMin);

				from += step;
			}

			return distanceMin;
		}

	}
}

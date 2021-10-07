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

			to -= (length / 2) * step;

			for (int i = 0; i < length; i++)
			{
				var toCoordinateShip = _unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
																				&& cs.Coordinate == to);
				if (toCoordinateShip.ShipId != 0 && toCoordinateShip.ShipId != ship.Id)
				{
					for (i--; i >= 0; i--)
					{
						to -= step;
						_unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
																 && cs.Coordinate == to).ShipId = 0;
					}

					Console.WriteLine("Coordinate is not free!!!");

					return false;
				}

				_unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
														 && cs.Coordinate == to).ShipId = ship.Id;

				to += step;
			}

			ship.Coordinate = to;
			ship.Rotation = targetRotation;

			return true;
		}

		public void Dislocate(Ship ship, int length, Board board, Coordinate from)
		{
			var step = Vector2D.Create(ship.Rotation);

			from -= (length / 2) * step;

			for (int i = length; i > 0; i--)
			{
				_unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
														 && cs.Coordinate == from).ShipId = 0;

				from += step;
			}
		}

		public void Dislocate(Ship ship, int length, Board board)
		{
			Dislocate(ship, length, board, ship.Coordinate);
		}

		public void Dislocate(Ship ship)
		{
			var size = _unitOfWork.Sizes.GetById(ship.SizeId);
			var player = _unitOfWork.Players.GetById(ship.PlayerId);
			var board = _unitOfWork.Boards.GetById(player.BoardId);

			Dislocate(ship, size.Length, board, ship.Coordinate);
		}

		public bool Relocate(Ship ship, Coordinate to, Rotation targetRotation)
		{
			var size = _unitOfWork.Sizes.GetById(ship.SizeId);
			var player = _unitOfWork.Players.GetById(ship.PlayerId);
			var board = _unitOfWork.Boards.GetById(player.BoardId);


			if (ship.Coordinate.CalculateDistance(to) > size.Speed)
			{
				Console.WriteLine("It's very fast for you");
				return false;
			}

			var currentRotation = ship.Rotation;

			Dislocate(ship, size.Length, board);

			if (!Allocate(ship, size.Length, board, to, targetRotation))
			{
				Allocate(ship, size.Length, board, ship.Coordinate, currentRotation);

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
			return Relocate(ship, ship.Coordinate, targetRotation);
		}

	}
}

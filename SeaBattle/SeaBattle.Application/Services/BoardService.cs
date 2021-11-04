using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Application.Services
{
	public class BoardService : BaseService<Board>, IBoardService
	{

		private readonly IRepository<CoordinateShip> _coordinateShips;
		protected readonly IRepository<Ship> _ships;

		public BoardService(IRepository<Board> boards, IRepository<CoordinateShip> coordinateShips, IRepository<Ship> ships) : base(boards)
		{
			_coordinateShips = coordinateShips;
			_ships = ships;
		}

		public Board Create(int xAbsMax, int yAbsMax)
		{
			var board = _entities.Add(new Board(xAbsMax, yAbsMax));

			for (int q = 0; q < Board.Quadrants; q++)
			{
				for (int x = 0; x <= xAbsMax; x++)
				{
					for (int y = 0; y <= yAbsMax; y++)
					{
						var coordinateShip = new CoordinateShip(board.Id, new Coordinate(q, x, y));

						board.CoordinateShips.Add(coordinateShip);
					}
				}
			}

			_coordinateShips.Add(board.CoordinateShips);

			_entities.SaveChanges();

			return board;
		}

		public string ToString(Board board)
		{
			board = _entities.FindFirst(b => b.Id == board.Id, nameof(Board) + "." + nameof(Board.CoordinateShips),
															   nameof(CoordinateShip) + "." + nameof(CoordinateShip.Ship));

			return board.ToString();
		}

		public Ship GetShipByIndexator(Board board, Coordinate coordinate)
		{
			board = _entities.FindFirst(b => b.Id == board.Id, nameof(Board) + "." + nameof(Board.Players),
															   nameof(Board) + "." + nameof(Board.CoordinateShips),
															   nameof(CoordinateShip) + "." + nameof(CoordinateShip.Ship));
			return board[coordinate];
		}
	}
}

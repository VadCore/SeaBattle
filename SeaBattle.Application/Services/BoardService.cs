using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
			FillNavigationFields(board);

			return board.ToString();
        }

		public Ship GetShipByIndexator(Board board, Coordinate coordinate)
		{
			FillNavigationFields(board);
			return board[coordinate];
		}

		private void FillNavigationFields(Board board) 
		{
			board.CoordinateShips = _coordinateShips.FindAll(cs => cs.BoardId == board.Id).ToList();

			var shipIds = board.CoordinateShips.Select(cs => cs.ShipId).Distinct();

			var shipsById = new Dictionary<int, Ship>();

			foreach(var coordinateShip in board.CoordinateShips)
            {
				if(coordinateShip.ShipId != null)
                {
					var shipId = (int)coordinateShip.ShipId;

					if (!shipsById.TryGetValue(shipId, out Ship ship))
                    {
						shipsById.Add(shipId, _ships.GetById(shipId));
                    }

					coordinateShip.Ship = shipsById[shipId];
				}
            }
		}
	}
}

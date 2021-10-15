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

        public BoardService(IRepository<Board> boards, IRepository<CoordinateShip> coordinateShips) : base(boards)
        {
            _coordinateShips = coordinateShips;
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
						_coordinateShips.Add(coordinateShip);
						board.CoordinateShips.Add(coordinateShip);
					}
				}
			}

			_entities.SaveChanges();

			return board;
		}
	}
}

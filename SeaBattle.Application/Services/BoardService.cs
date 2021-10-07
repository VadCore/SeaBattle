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
		public BoardService(IUnitOfWork unitOfWork) : base(unitOfWork)
		{
		}

		public Board Create(int xAbsMax, int yAbsMax)
		{
			var board = _unitOfWork.Boards.Add(new Board(xAbsMax, yAbsMax));

			for (int q = 0; q < Board.quadrants; q++)
			{
				for (int x = 0; x <= xAbsMax; x++)
				{
					for (int y = 0; y <= yAbsMax; y++)
					{
						_unitOfWork.CoordinateShips.Add(new CoordinateShip(board.Id, new Coordinate(q, x, y)));
					}
				}
			}

			_unitOfWork.Commit();

			return board;
		}
	}
}

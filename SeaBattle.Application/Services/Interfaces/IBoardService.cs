using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IBoardService
	{
		public Board Create(int xAbsMax, int yAbsMax);

		public string ToString(Board board);

		public Ship GetShipByIndexator(Board board, Coordinate coordinate);
	}
}

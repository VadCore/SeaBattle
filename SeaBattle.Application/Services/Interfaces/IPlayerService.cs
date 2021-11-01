using SeaBattle.Domain.Entities;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IPlayerService
	{
		public Player Create(string nick, Board board);
	}
}

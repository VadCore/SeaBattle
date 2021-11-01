using SeaBattle.Domain.Entities;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IPlayerService
	{
		public Player Create(User user, Board board);
	}
}

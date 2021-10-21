using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IBattleAbilityService
	{
		public bool Shot(Ship ship, Coordinate coordinate);
	}
}

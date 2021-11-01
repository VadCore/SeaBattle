using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface ISupportAbilityService
	{
		public bool Repair(Ship ship, Coordinate coordinate);
	}
}

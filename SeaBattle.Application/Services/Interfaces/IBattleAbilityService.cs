using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IBattleAbilityService
	{
		public bool Shot(Ship ship, Coordinate coordinate);
	}
}

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
	public class SupportAbilityService : AbilityService<SupportAbility>, ISupportAbilityService
	{

		private readonly IRepository<SupportAbility> _supportAbilities;

		public SupportAbilityService(IRepository<Player> players,
									IRepository<Board> boards,
									IRepository<Size> sizes,
									IShipService shipService,
									IRepository<Ship> ships,
									IRepository<CoordinateShip> coordinateShips,
									IRepository<SupportAbility> supportAbilities)
										: base(supportAbilities, players, boards, sizes, shipService, ships, coordinateShips)
		{
			_supportAbilities = supportAbilities;
		}

		public bool Repair(Ship ship, Coordinate coordinate)
		{
			var supportAbility = _supportAbilities.FindFirst(ba => ba.ShipId == ship.Id);
			var size = _sizes.GetById(ship.SizeId);

			if (!StartReloading(ship, supportAbility))
			{
				return false;
			}

			var targetShip = GetTargetShip(ship, coordinate);

			if (targetShip is null)
			{
				return false;
			}

			_supportAbilities.Update(supportAbility);

			size = _sizes.GetById(ship.SizeId);

			targetShip.Heal(size.HealShot, size.HealthMax);

			_ships.Update(targetShip);

			_entities.SaveChanges();

			return true;
		}
	}
}

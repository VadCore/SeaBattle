using SeaBattle.Application.Services.Interfaces;
using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using System;

namespace SeaBattle.Application.Services
{
	[System.Runtime.InteropServices.Guid("5B0CF9D8-5663-4A84-A08E-564AB2B7A657")]
	public class BattleAbilityService : AbilityService<BattleAbility>, IBattleAbilityService
	{


		public BattleAbilityService(IRepository<Player> players,
									IRepository<Board> boards,
									IRepository<Size> sizes,
									IShipService shipService,
									IRepository<Ship> ships,
									IRepository<CoordinateShip> coordinateShips,
									IRepository<BattleAbility> battleAbilities)
										: base(battleAbilities, players, boards, sizes, shipService, ships, coordinateShips)
		{

		}

		public bool Shot(Ship ship, Coordinate coordinate)
		{
			var battleAbility = _entities.FindFirst(ba => ba.ShipId == ship.Id);

			if (!StartReloading(ship, battleAbility))
			{
				return false;
			}

			var targetShip = GetTargetShip(ship, coordinate);

			if (targetShip is null)
			{
				return false;
			}

			_entities.Update(battleAbility);

			var size = _sizes.GetById(ship.SizeId);

			targetShip.Damage(size.DamageShot);

			if (targetShip.Health <= 0)
			{
				_shipService.Kill(targetShip);
				Console.WriteLine(ship.ToString() + " HAS BEEN SUNK");
			}

			_ships.Update(targetShip);

			_entities.SaveChanges();

			return true;
		}
	}
}

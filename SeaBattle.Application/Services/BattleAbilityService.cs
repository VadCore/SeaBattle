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
	public class BattleAbilityService : AbilityService<BattleAbility>, IBattleAbilityService
	{
		private readonly IShipService _shipService;

		public BattleAbilityService(IUnitOfWork unitOfWork, IShipService shipService) : base(unitOfWork, shipService)
		{
			_shipService = shipService;
		}

		public bool Shot(Ship ship, Coordinate coordinate)
		{
			var battleAbility = _unitOfWork.BattleAbilities.FindFirst(ba => ba.ShipId == ship.Id);

			if (!StartReloading(ship, battleAbility))
			{
				return false;
			}

			var targetShip = GetTargetShip(ship, coordinate);

			if (targetShip is null)
			{
				return false;
			}

			var size = _unitOfWork.Sizes.GetById(ship.SizeId);

			targetShip.Damage(size.DamageShot);

			if (targetShip.Health <= 0)
			{
				_shipService.Dislocate(targetShip);
			}

			return true;
		}
	}
}

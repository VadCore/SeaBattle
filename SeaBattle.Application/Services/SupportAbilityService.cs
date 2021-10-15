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
		private readonly IShipService _shipService;

		public SupportAbilityService(IUnitOfWork unitOfWork, IShipService shipService) : base(unitOfWork, shipService)
		{
			_shipService = shipService;
		}

		public bool Repair(Ship ship, Coordinate coordinate)
		{
			var supportAbility = _unitOfWork.SupportAbilities.FindFirst(ba => ba.ShipId == ship.Id);
			var size = _unitOfWork.Sizes.GetById(ship.SizeId);

			if (!StartReloading(ship, supportAbility))
			{
				return false;
			}

			var targetShip = GetTargetShip(ship, coordinate);

			if (targetShip is null)
			{
				return false;
			}

			size = _unitOfWork.Sizes.GetById(ship.SizeId);

			targetShip.Heal(size.HealShot, size.HealthMax);

			return true;
		}
	}
}

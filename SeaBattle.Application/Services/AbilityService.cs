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
    public abstract class AbilityService<TAbility> : BaseService<TAbility> where TAbility : Ability
    {
        private readonly IShipService _shipService;

        protected AbilityService(IUnitOfWork unitOfWork, IShipService shipService) : base(unitOfWork)
        {
            _shipService = shipService;
        }

        protected Ship GetTargetShip(Ship shipFrom, Coordinate coordinate)
        {
            var player = _unitOfWork.Players.GetById(shipFrom.PlayerId);
            var board = _unitOfWork.Boards.GetById(player.BoardId);
            var size = _unitOfWork.Sizes.GetById(shipFrom.SizeId);

            if (_shipService.CalculateDistanceFromNearestPoint(shipFrom, size, coordinate) > size.Range)
            {
                return null;
            }

            var shipId = _unitOfWork.CoordinateShips.FindFirst(cs => cs.BoardId == board.Id
                                                                     && cs.Coordinate == coordinate).ShipId;

            if (shipId == 0)
            {
                return null;
            }

            return _unitOfWork.Ships.GetById(shipId);
        }

        protected bool StartReloading(Ship ship, Ability ability)
        {
            var player = _unitOfWork.Players.GetById(ship.PlayerId);
            var board = _unitOfWork.Boards.GetById(player.BoardId);

            if (board.Turn >= ability.ReloadTurn)
            {
                var size = _unitOfWork.Sizes.GetById(ship.SizeId);
                ability.ReloadTurn = board.Turn + size.Reloading;
            }
            else
            {
                return false;
            }

            return true;
        }
    }
}

using SeaBattle.Domain.Common;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Application.Services.Interfaces
{
	public interface IShipService
	{
        public Ship CreateBattle(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation);

        public Ship CreateSupport(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation);

        public Ship CreateUniversal(SizeId sizeId, Player player, Coordinate coordinate, Rotation rotation);

        public bool Relocate(Ship ship, Coordinate to, Rotation targetRotation);

		public bool Relocate(Ship ship, Coordinate to);

		public bool Rotate(Ship ship, Rotation targetRotation);

		public void Kill(Ship ship);

		public int CalculateDistanceFromNearestPoint(Ship ship, Size shipSize, Coordinate to);
	}
}

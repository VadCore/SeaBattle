using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class UniversalShip : Ship, IUniversalShip
    {
        public UniversalShip(int playerId, ShipLenght length, Rotation rotation) : base(playerId, length, rotation, 3)
        {
        }

        public static IUniversalShip Create(int playerId, ShipLenght length, Rotation rotation)
        {
            return new UniversalShip(playerId, length, rotation);
        }
    }
}

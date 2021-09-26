using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class WarShip : Ship, IWarShip
    {
        private WarShip(int playerId, ShipLenght length, Rotation rotation) : base(playerId, length, rotation, 5)
        {
        }

        public static IWarShip Create(int playerId, ShipLenght length, Rotation rotation)
        {
            return new WarShip(playerId, length, rotation);
        }
    }   
}

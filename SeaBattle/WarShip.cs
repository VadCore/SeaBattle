using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class WarShip : Ship, IWarShip
    {
        private WarShip(int playerId, ShipLenght shipLenght, Rotation rotation) : base(playerId, shipLenght, rotation, 5)
        {
        }

        public static IWarShip Create(int playerId, int length, bool isHorizontal)
        {
            return new WarShip(playerId, length, isHorizontal);
        }
    }
}

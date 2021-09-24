using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class SupportShip : Ship
    {
        public SupportShip(int playerId, int length, bool isHorizontal) : base(playerId, length, isHorizontal, 5)
        {
        }

        public override bool Shoot(Square square)
        {
            Console.WriteLine("Shooting is invalid!");

            return false;
        }
    }
}

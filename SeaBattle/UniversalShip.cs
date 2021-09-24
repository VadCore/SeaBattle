using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class UniversalShip : Ship
    {
        public UniversalShip(int playerId, int length, bool isHorizontal) : base(playerId, length, isHorizontal, 3)
        {
        }
    }
}

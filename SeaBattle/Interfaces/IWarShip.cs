using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IWarShip : IShip
    {
        bool Shoot(Coordinate coordinate);
    }
}

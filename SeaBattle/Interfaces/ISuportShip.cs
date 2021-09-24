using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface ISupportShip : IShip
    {
        bool Repair(Coordinate coordinate);
    }
}

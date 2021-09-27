using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IShip : IUnit, IMovable, IPossibleBeBattle, IPossibleBeSupport
    {
    }
}

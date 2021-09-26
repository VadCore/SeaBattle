using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface ISupport : IAbility
    {
        public bool Repair(IShip targetShip, int healShot);
    }
}

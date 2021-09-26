using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IBattle : IAbility
    {
        public bool Shoot(IShip targetShip, int damageShot);
    }
}

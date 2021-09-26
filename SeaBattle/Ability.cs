using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ability
    {

        public bool Shoot(IShip targetShip, int damageShot)
        {
            targetShip?.Damage(damageShot);

            return targetShip != null;
        }

        public bool Repair(IShip targetShip, int healShot)
        {
            targetShip?.Heal(healShot);

            return targetShip != null;
        }
       
    }
}

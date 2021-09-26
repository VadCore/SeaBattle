using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ability
    {
        public IShip Ship { get; set; }
        public bool Shoot(Coordinate coordinate)
        {
            var targetShip = Ship.GetTargetShip(coordinate);
            targetShip?.Damage(Ship.DamageShot);

            return targetShip != null;
        }

        public bool Repair(Coordinate coordinate)
        {
            var targetShip = Ship.GetTargetShip(coordinate);
            targetShip?.Heal(Ship.HealShot);

            return targetShip != null;
        }
       
    }
}

using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class SmallShip<TAbility> : Ship<TAbility> where TAbility : IAbility, new()
    {
        static SmallShip()
        {
            SetCharacteristics(length: 1, speed: 4, damageShot: 1, healShot: 1, healthMax: 2);
        }

        public SmallShip(int id, int playerId, Coordinate coordinate, Rotation rotation) : base(id, playerId, coordinate, rotation)
        {

        }




        //public override int Length => 1;

        //public override int Speed => 4;

        //public override int HealthMax => 2;

        //public override int DamageShot => 1;

        //public override int HealShot => 1;
    }
}

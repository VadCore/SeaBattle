using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class BigShip<TAbility> : Ship<TAbility> where TAbility : IAbility, new()
    {
        static BigShip()
        {
            SetCharacteristics(length: 5, speed: 2, damageShot: 3, healShot: 2, healthMax: 4);
        }

        public BigShip(int id, int playerId, Coordinate coordinate, Rotation rotation) : base(id, playerId, coordinate, rotation)
        {
        }




        //public override int Length => 5;

        //public override int Speed => 2;

        //public override int HealthMax => 4;

        //public override int DamageShot => 3;

        //public override int HealShot => 2;
    }
}

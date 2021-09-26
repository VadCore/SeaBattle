using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class HugeShip<TAbility> : Ship<TAbility> where TAbility : IAbility, new()
    {

        static HugeShip()
        {
            SetCharacteristics(length: 7, speed: 1, damageShot: 4, healShot: 2, healthMax: 5);
        }

        public HugeShip(int id, int playerId, Coordinate coordinate, Rotation rotation) : base(id, playerId, coordinate, rotation)
        {
        }




        //public override int Length => 7;

        //public override int Speed => 1;

        //public override int HealthMax => 5;

        //public override int DamageShot => 4;

        //public override int HealShot => 2;
    }
}

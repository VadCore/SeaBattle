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
    }
}

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
    }
}

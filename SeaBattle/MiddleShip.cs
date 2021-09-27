using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class MiddleShip<TAbility> : Ship<TAbility> where TAbility : IAbility, new()
    {
        static MiddleShip()
        {
            SetCharacteristics(length: 3, speed: 3, damageShot: 2, healShot: 1, healthMax: 3);
        }

        public MiddleShip(int id, int playerId, Coordinate coordinate, Rotation rotation) : base(id, playerId, coordinate, rotation)
        {
        }
    }
}

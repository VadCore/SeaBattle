using SeaBattle.Enums;
using SeaBattle.Extansions;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ship<TAbility> : MovableUnit<TAbility>, IShip where TAbility : IAbility, new()
    {

        private static int length;
        private static int speed;
        private static int healthMax;
        private static int damageShot;
        private static int healShot;

        public sealed override int Length => length;
        public sealed override int Speed => speed;
        public sealed override int HealthMax => healthMax;
        public int DamageShot => damageShot;
        public int HealShot => healShot;

        public Ship(int id, int playerId, Coordinate coordinate, Rotation rotation) : base(id, playerId, rotation, coordinate, healthMax)
        {
            Allocate(coordinate);
        }

        protected static void SetCharacteristics(int length, int speed, int damageShot, int healShot, int healthMax)
        {
            if (length % 2 != 1)
            {
                throw new ArgumentException("Length must be odd number");
            }

            Ship<TAbility>.length = length.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.speed = speed.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.damageShot = damageShot.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.healShot = healShot.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.healthMax = healthMax.ExceptionIfNotBetweenMinMax(0);
        }
    }
}

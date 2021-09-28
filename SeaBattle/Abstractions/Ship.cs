using SeaBattle.Enums;
using SeaBattle.Extansions;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ship<TSize> : MovableUnit, IShip where TSize : Ship<TSize>, new()
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



        public static IUnit Create<TAbility>(int id, Player player, Coordinate coordinate, Rotation rotation)
          where TAbility : IAbility, new()
        {
            return Create<TSize, TAbility>(id, player, coordinate, rotation, healthMax);
        }

        protected static void SetCharacteristics(int length, int speed, int damageShot, int healShot, int healthMax)
        {
            if (length % 2 != 1)
            {
                throw new ArgumentException("Length must be odd number");
            }

            Ship<TSize>.length = length.ExceptionIfNotBetweenMinMax(0);
            Ship<TSize>.speed = speed.ExceptionIfNotBetweenMinMax(0);
            Ship<TSize>.damageShot = damageShot.ExceptionIfNotBetweenMinMax(0);
            Ship<TSize>.healShot = healShot.ExceptionIfNotBetweenMinMax(0);
            Ship<TSize>.healthMax = healthMax.ExceptionIfNotBetweenMinMax(0);
        }

        public override string ToString()
        {
            return new StringBuilder($"{typeof(TSize).Name}, has id: {Id}").ToString();
        }
    }
}

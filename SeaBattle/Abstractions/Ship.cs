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

        public override bool Equals(object obj)
        {
            return obj is Ship<TAbility> ship &&
                   Length == ship.Length &&
                   Speed == ship.Speed &&
                   EqualityComparer<IAbility>.Default.Equals(Ability, ship.Ability);
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Id);
            hash.Add(PlayerId);
            hash.Add(Coordinate);
            hash.Add(Health);
            hash.Add(Length);
            hash.Add(Ability);
            hash.Add(Length);
            return hash.ToHashCode();
        }

        public static bool operator ==(Ship<TAbility> firstShip, Ship<TAbility> secondShip)
        {
           if(firstShip == null ||  secondShip == null)
            {
                return false;
            }

            return firstShip.Speed == secondShip.Speed
                && firstShip.Length == secondShip.Length
                && firstShip.GetType() == firstShip.GetType();
        }

        public static bool operator !=(Ship<TAbility> firstShip, Ship<TAbility> secondShip)
        {
            return !(firstShip == secondShip);
        }

        public override string ToString()
        {
            return string.Format($"My coordinate: Quadrant = {Coordinate.Quadrant}, X = {Coordinate.XAbs}, Y = {Coordinate.YAbs}"+
                 $"My lenght is {Length}, and my speed {Speed}");
        }
    }
}

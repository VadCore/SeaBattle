using SeaBattle.Enums;
using SeaBattle.Extansions;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ship<TAbility> : Unit, IShip where TAbility : IAbility, new()
    {

        private static int length;
        private static int speed;
        private static int healthMax;
        private static int damageShot;
        private static int healShot;

        public int Length => length;
        public int Speed => speed;
        public int HealthMax => healthMax;
        public int DamageShot => damageShot;
        public int HealShot => healShot;

        public IAbility Ability { get; }


        public Ship(int id, int playerId, Coordinate coordinate, Rotation rotation) : base(id, playerId, rotation, coordinate, healthMax)
        {
            Allocate(coordinate);
        }

        public IShip GetTargetShip(Coordinate coordinate)
        {
            if (CalculateDistance(coordinate) > Ability.Range)
            {
                Console.WriteLine("The target is too far");

                return null;
            }

            var targetShip = Game.Board[coordinate];

            if (targetShip is null)
            {
                Console.WriteLine("Missed");
            }

            return targetShip;
        }

        public void Damage(int damage)
        {
            Health -= damage;
            Console.WriteLine("Hit");

            if (Health <= 0)
            {
                Console.WriteLine("Sunk");
                Dislocate();
            }
        }

        public void Heal(int healShot)
        {
            Health = Math.Max(Health + healShot, HealthMax);
            Console.WriteLine("Heal");

            if (Health == HealthMax)
            {
                Console.WriteLine("Completely Healed");
            }
        }

        public int CalculateDistance(Coordinate to)
        {
            var from = Coordinate;

            var step = Vector2D.Create(Rotation);

            from -= (Length / 2) * step;

            int distanceMin = int.MaxValue;

            for (int i = Length; i > 0; i--)
            {
                distanceMin = Math.Min(from.CalculateDistance(to), distanceMin);

                from += step;
            }

            return distanceMin;
        }

        public bool Allocate(Coordinate to)
        {
            if (to.XAbs + (Rotation == Rotation.Horizontal ? Length / 2 : 0) > Game.Board.XAbsMax
            || to.YAbs + (Rotation == Rotation.Vertical ? Length / 2 : 0) > Game.Board.YAbsMax)
            {
                return false;
            }

            var step = Vector2D.Create(Rotation);

            to -= (Length / 2) * step;

            for (int i = 0; i < Length; i++)
            {
                if (Game.Board[to] != null && Game.Board[to] != this)
                {
                    for (i--; i >= 0; i--)
                    {
                        to -= step;
                        Game.Board[to] = null;
                    }

                    Console.WriteLine("Coordinate is not free!!!");

                    return false;
                }

                Game.Board[to] = this;

                to += step;
            }

            Coordinate = to;

            return true;
        }

        public void Dislocate(Coordinate from)
        {
            var step = Vector2D.Create(Rotation);

            from -= (Length / 2) * step;

            for (int i = Length; i > 0; i--)
            {
                Game.Board[from] = null;

                from += step;
            }
        }

        public void Dislocate()
        {
            Dislocate(Coordinate);
        }

        public bool Relocate(Coordinate from, Coordinate to)
        {
            if (Coordinate.CalculateDistance(to) > Speed)
            {
                Console.WriteLine("It's very fast for you");
                return false;
            }

            Dislocate(from);

            if (!Allocate(to))
            {
                Allocate(from);
                return false;
            }

            return true;
        }

        public bool Rotate(Rotation target)
        {
            var currentRotation = Rotation;
            
            Dislocate(Coordinate);

            Rotation = target;

            if (!Allocate(Coordinate))
            {
                Rotation = currentRotation;
                Allocate(Coordinate);

                return false;
            }

            return true;
        }

        protected static void SetCharacteristics(int length, int speed, int damageShot, int healShot, int healthMax)
        {
            if (length % 2 != 1)
                throw new ArgumentException("Length must be odd number");

            Ship<TAbility>.length = length.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.speed = speed.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.damageShot = damageShot.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.healShot = healShot.ExceptionIfNotBetweenMinMax(0);
            Ship<TAbility>.healthMax = healthMax.ExceptionIfNotBetweenMinMax(0);
        }
    }
}

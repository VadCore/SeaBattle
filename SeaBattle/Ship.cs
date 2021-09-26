using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Ship
    {
        public static readonly int shipLengthMax = 4;
        public static readonly int shipSpeedMax = 5;

        public int Range { get; set; } = 3;

        public int Length { get; private set; }
        public int Speed => shipSpeedMax - Length;
        public int HealthMax { get; set; }
        public int DamageShot { get; set; }
        public int HealShot { get; set; }

        public int Id { get; set; }
        public int PlayerId { get; private set; }
        public Rotation Rotation { get; set; }
        public Coordinate Coordinate { get; set; }
        
        
        public int Health { get; set; }
        

        public Ship(int playerId, ShipLenght length, Rotation rotation, int range)
        {
            PlayerId = playerId;
            Length = (int)length;
            HealthMax = Length;
            Health = HealthMax;
            DamageShot = (Length - 1) / 2 + 1;
            HealShot = (Length - 1) / 2 + 1;
            Rotation = rotation;
            Range = range;
        }

        public Ship GetTargetShip(Coordinate coordinate)
        {
            if (CalculateDistance(coordinate) > Range)
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

        public bool Shoot(Coordinate coordinate)
        {
            var targetShip = GetTargetShip(coordinate);
            targetShip?.Damage(DamageShot);

            return targetShip != null;
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

        public bool Repair(Coordinate coordinate)
        {
            var targetShip = GetTargetShip(coordinate);
            targetShip?.Heal(HealShot);

            return targetShip != null;
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
    }
}

using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Unit<TAbility> : IUnit where TAbility : IAbility, new()
    {
        public int Id { get; }
        public int PlayerId { get; }
        public Rotation Rotation { get; set; }
        public Coordinate Coordinate { get; set; }
        public int Health { get; protected set; }

        public abstract int Length { get; }
        public abstract int HealthMax { get; }

        public IAbility Ability { get; }

        public Unit(int id, int playerId, Rotation rotation, Coordinate coordinate, int health)
        {
            Id = id;
            PlayerId = playerId;
            Rotation = rotation;
            Coordinate = coordinate;
            Health = health;

            Ability = new TAbility() { Unit = this };
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
    }
}

using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class Unit : IUnit
    {
        protected Rotation rotation;
        protected Coordinate coordinate;
        protected int health;

        public int Id { get; init; }
        public int PlayerId { get; init; }
        public Rotation Rotation { get => rotation; init => rotation = value; }
        public Coordinate Coordinate { get => coordinate; init => coordinate = value; }
        public int Health { get => health; init => health = value; }

        public abstract int Length { get; }
        public abstract int HealthMax { get; }

        public IAbility Ability { get; init; }


        public static IUnit Create<TKind, TAbility>(int id, int playerId, Coordinate coordinate, Rotation rotation, int health)
          where TKind : IUnit, new()
          where TAbility : IAbility, new()
        {
            var unit = new TKind() { Id = id, PlayerId = playerId, Coordinate = coordinate, Rotation = rotation, Health = health, Ability = new TAbility() };

            unit.Allocate(coordinate);

            return unit;
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

            coordinate = to;

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
            health -= damage;
            Console.WriteLine("Hit");

            if (Health <= 0)
            {
                Console.WriteLine("Sunk");
                Dislocate();
            }
        }

        public void Heal(int healShot)
        {
            health = Math.Max(Health + healShot, HealthMax);
            Console.WriteLine("Heal");

            if (Health == HealthMax)
            {
                Console.WriteLine("Completely Healed");
            }
        }
    }
}

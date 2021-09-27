using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class MovableUnit<TAbility> : Unit<TAbility>, IMovable
    where TAbility : IAbility, new()
    {
        public abstract int Speed { get; }

        public MovableUnit(int id, int playerId, Rotation rotation, Coordinate coordinate, int health)
          : base(id, playerId, rotation, coordinate, health)
        {
        }

        public bool Relocate(Coordinate to, Rotation targetRotation)
        {
            if (Coordinate.CalculateDistance(to) > Speed)
            {
                Console.WriteLine("It's very fast for you");
                return false;
            }

            var currentRotation = Rotation;

            Dislocate(Coordinate);

            Rotation = targetRotation;

            if (!Allocate(to))
            {
                Rotation = currentRotation;
                Allocate(Coordinate);

                return false;
            }

            return true;
        }

        public bool Relocate(Coordinate to)
        {
            return Relocate(to, Rotation);
        }

        public bool Rotate(Rotation targetRotation)
        {
            return Relocate(Coordinate, targetRotation);
        }
    }
}

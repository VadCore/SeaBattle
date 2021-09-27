using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public abstract class MovableUnit : Unit, IMovable
    {
        public abstract int Speed { get; }

        public bool Relocate(Coordinate to, Rotation targetRotation)
        {
            if (Coordinate.CalculateDistance(to) > Speed)
            {
                Console.WriteLine("It's very fast for you");
                return false;
            }

            var currentRotation = Rotation;

            Dislocate(Coordinate);

            rotation = targetRotation;

            if (!Allocate(to))
            {
                rotation = currentRotation;
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

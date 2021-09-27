using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IMovable
    {
        public int Speed { get; }

        public bool Relocate(Coordinate to, Rotation targetRotation);

        public bool Relocate(Coordinate to);

        public bool Rotate(Rotation targetRotation);
    }
}

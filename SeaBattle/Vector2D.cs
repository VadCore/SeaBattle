using SeaBattle.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Vector2D
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator *(Vector2D vector, int multiplier)
        {
            vector.X *= multiplier;
            vector.Y *= multiplier;

            return vector;
        }

        public static Vector2D Create(Rotation rotation)
        {
            if(rotation == Rotation.Horizontal)
            {
                return new Vector2D(1, 0);
            }
            else
            {
                return new Vector2D(0, 1);
            }
        }
    }
}

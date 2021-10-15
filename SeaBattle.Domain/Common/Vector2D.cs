using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Common
{
    public class Vector2D : ValueObject<Vector2D>
    {
        public int X { get; }
        public int Y { get; }

        public Vector2D(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Vector2D operator *(int multiplier, Vector2D vector)
        {
            return new Vector2D(vector.X * multiplier, vector.Y * multiplier);
        }

        public static Vector2D operator -(Vector2D vector)
        {
            return -1 * vector;
        }

        public static Vector2D Create(Rotation rotation)
        {
            if (rotation == Rotation.Horizontal)
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

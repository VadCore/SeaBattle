using SeaBattle.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Common
{
    public class Coordinate : ValueObject<Coordinate>
    {

        public int Quadrant { get; }
        public int XAbs { get; }
        public int YAbs { get; }

        public Coordinate(int quadrant, int xAbs, int yAbs)
        {
            Quadrant = quadrant.ExceptionIfNotBetweenMinMax(0, 3);

            if (Quadrant % 2 == 1 && xAbs < 0)
            {
                Quadrant--;
                xAbs *= -1;
            }
            else if (Quadrant % 2 == 0 && xAbs <= 0)
            {
                Quadrant++;
                xAbs *= -1;
            }

            XAbs = xAbs.ExceptionIfNotBetweenMinMax(0);

            if (Quadrant / 2 == 0 && yAbs < 0)
            {
                Quadrant += 2;
                yAbs *= -1;
            }
            else if (Quadrant / 2 == 1 && yAbs <= 0)
            {
                Quadrant -= 2;
                yAbs *= -1;
            }

            YAbs = yAbs.ExceptionIfNotBetweenMinMax(0);
        }

        public static Coordinate operator +(Coordinate coordinate, Vector2D vector)
        {
            return new Coordinate(coordinate.Quadrant, coordinate.XAbs + vector.X, coordinate.YAbs + vector.Y);
        }

        public static Coordinate operator -(Coordinate coordinate, Vector2D vector)
        {
            return coordinate + (-vector);
        }

        public int CalculateDistance(Coordinate to)
        {
            int xDelta;
            int yDelta;

            if ((Quadrant % 2) == (to.Quadrant % 2))
            {
                xDelta = Math.Abs(XAbs - to.XAbs);
            }
            else
            {
                xDelta = XAbs + to.XAbs;
            }

            if ((Quadrant / 2) == (to.Quadrant / 2))
            {
                yDelta = Math.Abs(YAbs - to.YAbs);
            }
            else
            {
                yDelta = YAbs + to.YAbs;
            }

            return Math.Max(xDelta, yDelta);
        }
    }
}

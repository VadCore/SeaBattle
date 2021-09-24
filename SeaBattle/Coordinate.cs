using SeaBattle.Extansions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Coordinate
    {
        private int xAbs;
        private int quadrant;
        private int yAbs;
        public int Quadrant
        {
            get => quadrant;
            private set => quadrant = value.ExceptionIfNotBetweenMinMax(0, 3); // In range of four quadrants
        }
        public int XAbs
        {
            get => xAbs;
            set
            {
                if (Quadrant % 2 == 1 && value < 0)
                {
                    Quadrant--;
                    value *= -1;
                }
                else if (Quadrant % 2 == 0 && value <= 0)
                {
                    Quadrant++;
                    value *= -1;
                }

                xAbs = value.ExceptionIfNotBetweenMinMax(0, Game.Board.XAbsMax);
            }
        }
        public int YAbs
        {
            get => yAbs;
            set
            {
                if (Quadrant / 2 == 0 && value < 0)
                {
                    Quadrant += 2;
                    value *= -1;
                }
                else if (Quadrant / 2 == 1 && value <= 0)
                {
                    Quadrant -= 2;
                    value *= -1;
                }

                yAbs = value.ExceptionIfNotBetweenMinMax(0, Game.Board.YAbsMax);
            }
        }

        public Coordinate(int quadrant, int xAbs, int yAbs)
        {
            Quadrant = quadrant;
            XAbs = xAbs;
            YAbs = yAbs;
        }

        public static Coordinate operator +(Coordinate coordinate, Vector2D vector)
        {
            coordinate.XAbs += vector.X;
            coordinate.YAbs += vector.Y;

            return coordinate;
        }

        public static Coordinate operator -(Coordinate coordinate, Vector2D vector)
        {
            coordinate.XAbs -= vector.X;
            coordinate.YAbs -= vector.Y;

            return coordinate;
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

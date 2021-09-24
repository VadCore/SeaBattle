using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Coordinate
    {
        public int Quadrant { get; set; }
        public int XAbs { get; set; }
        public int YAbs { get; set; }

        public Coordinate(int quadrant, int xAbs, int yAbs)
        {
            Quadrant = quadrant;
            XAbs = xAbs;
            YAbs = yAbs;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Board
    {
        public readonly int XAbsMax;
        public readonly int YAbsMax;
        private Ship[,,] coordinateShips;

        public Board(int xAbsMax, int yAbsMax)
        {
            XAbsMax = xAbsMax;
            YAbsMax = yAbsMax;
            coordinateShips = new Ship[4, XAbsMax, YAbsMax];
        }

        public Ship this[Coordinate coordinate]
        {
            get
            {
                return coordinateShips[coordinate.Quadrant, coordinate.XAbs, coordinate.YAbs];
            }
            set
            {
                coordinateShips[coordinate.Quadrant, coordinate.XAbs, coordinate.YAbs] = value;
            }
        }


    }
}

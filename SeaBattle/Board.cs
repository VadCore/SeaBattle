using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Board
    {
        public readonly int XAbsMax;
        public readonly int YAbsMax;
        public readonly int quadrants = 4;

        private readonly IUnit[,,] coordinateShips;

        public Board(int xAbsMax, int yAbsMax)
        {
            XAbsMax = xAbsMax;
            YAbsMax = yAbsMax;
            coordinateShips = new IUnit[quadrants, XAbsMax, YAbsMax];
        }

        public IUnit this[Coordinate coordinate]
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

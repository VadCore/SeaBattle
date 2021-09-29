using Newtonsoft.Json;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeaBattle
{
    public class Board
    {
        public static readonly int XAbsMax = 15;
        public static readonly int YAbsMax = 15;
        public static readonly int quadrants = 4;
        [JsonProperty]
        private IUnit[,,] coordinateShips;

        public Board()
        {
            coordinateShips = new IUnit[quadrants, XAbsMax + 1, YAbsMax + 1];
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

        public override string ToString()
        {
            var sortedUnits = new List<IUnit>();

            for (int d = 0, dLim = Math.Max(XAbsMax, YAbsMax); d <= dLim; d++)
            {
                for (int q = 0; q < quadrants; q++)
                {
                    for (int i = 0; i <= d; i++)
                    {
                        if (d <= XAbsMax && i <= YAbsMax && coordinateShips[q, d, i] != null)
                            sortedUnits.Add(coordinateShips[q, d, i]);

                        if (i <= XAbsMax && d <= YAbsMax && coordinateShips[q, i, d] != null)
                            sortedUnits.Add(coordinateShips[q, i, d]);
                    }
                }
            }

            var distinctSortedUnits = sortedUnits.Distinct().ToList();

            return string.Join('|', distinctSortedUnits);
        }

    }
}

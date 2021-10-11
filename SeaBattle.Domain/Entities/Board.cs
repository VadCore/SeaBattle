using SeaBattle.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
    public class Board : BaseEntity
    {
        public const int Quadrants = 4;
        

        public int XAbsMax { get; set; }
        public int YAbsMax { get; set; }
        public int Turn { get; set; }
        public int TurnPlayerId { get; set; }

        //public IList<Player> Players { get; set; } = new List<Player>();
        public IList<CoordinateShip> CoordinateShips => coordinateShips ??= new List<CoordinateShip>();
        private IList<CoordinateShip> coordinateShips;
        public Board(int xAbsMax, int yAbsMax)
        {
            XAbsMax = xAbsMax;
            YAbsMax = yAbsMax;
        }

        public Board()
        {
        }

        public Ship this[Coordinate coordinate]
        {
            get
            {
                return CoordinateShips.First(cs => cs.Coordinate == coordinate).Ship;
            }
            set
            {
                CoordinateShips.First(cs => cs.Coordinate == coordinate).Ship = value;
            }
        }

        public override string ToString()
        {
            var sortedUnits = new List<Ship>();

            for (int d = 0, dLim = Math.Max(XAbsMax, YAbsMax); d <= dLim; d++)
            {
                for (int q = 0; q < Quadrants; q++)
                {
                    for (int i = 0; i <= d; i++)
                    {
                        if (d <= XAbsMax && i <= YAbsMax)
                        {
                            var ship = CoordinateShips.First(cs => cs.BoardId == Id
                                                            && cs.Coordinate == new Coordinate(q, d, i)).Ship;

                            if (ship != null)
                            {
                                sortedUnits.Add(ship);
                            }
                        }

                        if (i <= XAbsMax && d <= YAbsMax)
                        {
                            var ship = CoordinateShips.First(cs => cs.BoardId == Id
                                                            && cs.Coordinate == new Coordinate(q, i, d)).Ship;

                            if (ship != null)
                            {
                                sortedUnits.Add(ship);
                            }
                        }
                    }
                }
            }

            var distinctSortedUnits = sortedUnits.Distinct().ToList();

            return string.Join('|', distinctSortedUnits);
        }

    }
}

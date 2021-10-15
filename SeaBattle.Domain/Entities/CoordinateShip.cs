using SeaBattle.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
    public class CoordinateShip : BaseEntity
    {
        public int BoardId { get; set; }
        public Coordinate Coordinate { get; set; }
        public int ShipId { get; set; }
        public Ship Ship { get; set; }

        public CoordinateShip(int boardId, Coordinate coordinate)
        {
            BoardId = boardId;
            Coordinate = coordinate;
        }

        public CoordinateShip()
        {
        }
    }
}

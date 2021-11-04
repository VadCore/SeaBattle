using SeaBattle.Domain.Common;

namespace SeaBattle.Domain.Entities
{
	public class CoordinateShip : BaseEntity<CoordinateShip>
	{
		public int BoardId { get; set; }
		public int Quadrant { get; set; }
		public int XAbs { get; set; }
		public int YAbs { get; set; }
		public int? ShipId { get; set; }

		public Board Board { get; set; }
		public Ship Ship { get; set; }

		public CoordinateShip(int boardId, Coordinate coordinate)
		{
			BoardId = boardId;
			Quadrant = coordinate.Quadrant;
			XAbs = coordinate.XAbs;
			YAbs = coordinate.YAbs;
		}

		public CoordinateShip()
		{
		}

		public Coordinate GetCoordinate()
		{
			return new Coordinate(Quadrant, XAbs, YAbs);
		}
	}
}

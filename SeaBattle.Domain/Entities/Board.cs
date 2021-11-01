using Newtonsoft.Json;
using SeaBattle.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SeaBattle.Domain.Entities
{
	public class Board : BaseEntity<Board>
	{
		public const int Quadrants = 4;
		public int XAbsMax { get; set; }
		public int YAbsMax { get; set; }
		public int Turn { get; set; }
		public int TurnPlayerId { get; set; }

		[JsonIgnore]
		public IList<Player> Players { get; set; }  = new List<Player>();

		[JsonIgnore]
		public IList<CoordinateShip> CoordinateShips { get; set; } = new List<CoordinateShip>();

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

				return CoordinateShips.First(cs => cs.Quadrant == coordinate.Quadrant
										&& cs.XAbs == coordinate.XAbs
										&& cs.YAbs == coordinate.YAbs).Ship;
			}
			set
			{
				CoordinateShips.First(cs => cs.Quadrant == coordinate.Quadrant
										&& cs.XAbs == coordinate.XAbs
										&& cs.YAbs == coordinate.YAbs).Ship = value;
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
							var coordinate = new Coordinate(q, d, i);

							var ship = CoordinateShips.FirstOrDefault(cs => cs.BoardId == Id
															&& cs.Quadrant == coordinate.Quadrant
															&& cs.XAbs == coordinate.XAbs
															&& cs.YAbs == coordinate.YAbs)?.Ship;

							if (ship != null)
							{
								sortedUnits.Add(ship);
							}
						}

						if (i <= XAbsMax && d <= YAbsMax)
						{
							var coordinate = new Coordinate(q, i, d);

							var ship = CoordinateShips.FirstOrDefault(cs => cs.BoardId == Id
															&& cs.Quadrant == coordinate.Quadrant
															&& cs.XAbs == coordinate.XAbs
															&& cs.YAbs == coordinate.YAbs)?.Ship;

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

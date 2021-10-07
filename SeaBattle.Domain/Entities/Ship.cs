using SeaBattle.Domain.Common;
using SeaBattle.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
	public class Ship : BaseEntity
	{
		private int health;

		public int PlayerId { get; init; }
		public int SizeId { get; init; }
		public int Health { get => health; set => health = value; }
		public Coordinate Coordinate { get; set; }
		public Rotation Rotation { get; set; }
		public int NextTurn { get; set; }

		public Player Player { get; set; }
		public Size Size { get; set; }
		public IList<CoordinateShip> CoordinateShips { get; set; } = new List<CoordinateShip>();
		public BattleAbility BattleAbility { get; set; }
		public SupportAbility SupportAbility { get; set; }


		[JsonIgnore]
		public Board Board => Player.Board;


		public Ship(int sizeId, int playerId, int health)
		{
			SizeId = sizeId;
			PlayerId = playerId;
			this.health = health;
		}

		public Ship()
		{
		}

		public int CalculateDistance(Coordinate to, int length)
		{
			var from = Coordinate;

			var step = Vector2D.Create(Rotation);

			from -= (length / 2) * step;

			int distanceMin = int.MaxValue;

			for (int i = length; i > 0; i--)
			{
				distanceMin = Math.Min(from.CalculateDistance(to), distanceMin);

				from += step;
			}

			return distanceMin;
		}

		public void Damage(int damage)
		{
			health -= damage;

		}

		public void Heal(int healShot)
		{
			health = Math.Max(Health + healShot, Size.HealthMax);
		}
	}
}

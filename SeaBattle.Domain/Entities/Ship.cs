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
		public int CenterCoordinateShipId { get; set; }
		public Rotation Rotation { get; set; }
		public int NextTurn { get; set; }

		public Ship(int sizeId, int playerId, int health)
		{
			SizeId = sizeId;
			PlayerId = playerId;
			this.health = health;
		}

		public Ship()
		{
		}




		public void Damage(int damage)
		{
			health -= damage;

		}

		public void Heal(int healShot, int healthMax)
		{
			health = Math.Max(Health + healShot, healthMax);
		}
	}
}

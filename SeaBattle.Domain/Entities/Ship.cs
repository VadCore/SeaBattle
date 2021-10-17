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
	public class Ship : BaseEntity<Ship>
	{
		public int PlayerId { get; init; }
		public int SizeId { get; init; }
		public int Health { get; set; }
		public int CenterCoordinateShipId { get; set; }
		public Rotation Rotation { get; set; }
		public int NextTurn { get; set; }

		public Ship(int sizeId, int playerId, int health)
		{
			SizeId = sizeId;
			PlayerId = playerId;
			Health = health;
		}

		public Ship()
		{
		}

		public void Damage(int damage)
		{
			Health -= damage;

		}

		public void Heal(int healShot, int healthMax)
		{
			Health = Math.Max(Health + healShot, healthMax);
		}

   //     public override string ToString()
   //     {
			//return string.Format($"PlayerId: {PlayerId}, SizeId: {SizeId}, CenterCoordinateShipId: {CenterCoordinateShipId}");
   //     }
    }
}

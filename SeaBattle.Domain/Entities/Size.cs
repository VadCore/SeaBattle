using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
	public class Size : BaseEntity
	{
		public string Title { get; set; }
		public int Length { get; set; }
		public int HealthMax { get; set; }
		public int Speed { get; set; }
		public int Range { get; set; }
		public int Reloading { get; set; }
		public int DamageShot { get; set; }
		public int HealShot { get; set; }
	}
}

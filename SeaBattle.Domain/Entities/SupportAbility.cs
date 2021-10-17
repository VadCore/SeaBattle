using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
	public class SupportAbility : Ability<SupportAbility>
	{
		public SupportAbility(int shipId) : base(shipId)
		{
		}

		public SupportAbility()
		{
		}
	}
}

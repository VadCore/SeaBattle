using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
	public class BattleAbility : Ability<BattleAbility>
	{
		public BattleAbility(int shipId) : base(shipId)
		{
		}

		public BattleAbility()
		{
		}
	}
}

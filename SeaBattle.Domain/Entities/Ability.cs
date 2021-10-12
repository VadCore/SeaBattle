using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
    public abstract class Ability : BaseEntity
    {
        public int ShipId { get; set; }
        public int ReloadTurn { get; set; }

        public Ability(int shipId)
        {
            ShipId = shipId;
        }
        public Ability()
        {
        }
    }
}

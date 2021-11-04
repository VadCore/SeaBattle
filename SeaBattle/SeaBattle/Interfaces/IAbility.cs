using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle.Interfaces
{
    public interface IAbility
    {
        public int Range { get; }

        public IUnit Unit { get; set; }
    }
}

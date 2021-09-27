using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Universal : Ability, IUniversal
    {
        private static readonly int range = 3;
        public override int Range => range;
    }
}

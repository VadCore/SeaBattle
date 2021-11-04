using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Support : Ability, ISupport
    {
        private static readonly int range = 4;
        public override int Range => range;
    }
}

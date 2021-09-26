using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Battle : Ability, IBattle
    {
        public int Range => 5;
    }
}

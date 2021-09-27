﻿using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Battle : Ability, IBattle
    {
        private static readonly int range = 5;
        public override int Range => range;
    }
}

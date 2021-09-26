using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class Universal : Ability, IUniversal
    {
        public int Range => 3;
    }
}

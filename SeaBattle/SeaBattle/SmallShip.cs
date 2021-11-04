using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class SmallShip : Ship<SmallShip>
    {
        static SmallShip()
        {
            SetCharacteristics(length: 1, speed: 4, damageShot: 1, healShot: 1, healthMax: 2);
        }
    }
}

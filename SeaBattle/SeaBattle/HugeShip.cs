using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class HugeShip : Ship<HugeShip>
    {

        static HugeShip()
        {
            SetCharacteristics(length: 7, speed: 1, damageShot: 4, healShot: 2, healthMax: 5);
        }
    }
}

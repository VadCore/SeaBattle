using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class MiddleShip : Ship<MiddleShip>
    {
        static MiddleShip()
        {
            SetCharacteristics(length: 3, speed: 3, damageShot: 2, healShot: 1, healthMax: 3);
        }
    }
}

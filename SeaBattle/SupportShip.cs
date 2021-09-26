﻿using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public class SupportShip : Ship, ISupportShip
    {
        private SupportShip(int playerId, ShipLenght length, Rotation rotation) : base(playerId, length, rotation, 5)
        {
        }

        public static ISupportShip Create(int playerId, ShipLenght length, Rotation rotation)
        {
            return new SupportShip(playerId, length, rotation);
        }
    }
}
using SeaBattle.Enums;
using System;
using System.Collections.Generic;

namespace SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var ships = new List<Ship>();

            var shipSupport = SupportShip.Create(1, ShipLenght.Middle, Enums.Rotation.Horizontal);

            var newShip = ((Ship)shipSupport).Shoot(new Coordinate(1, 2, 3));
        }
    }
}

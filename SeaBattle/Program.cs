using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeaBattle
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            //var ships = new List<IShip>();

            //var coord1 = new Coordinate(0, 1, 2);

            //var battleSmallShip1 = SmallShip.Create<Support>(0, 1, coord1, Rotation.Horizontal);


            //var battleBigShip1 = BigShip.Create<Battle>(0, 1, new Coordinate(1, 2, 4), Rotation.Horizontal);

            //Console.WriteLine(battleSmallShip1.Length);

            ////Console.WriteLine("Max Health " + SmallShip<Battle>.healthMax);
            ////Console.WriteLine("Max HealthMax  " + battleBigShip1.HealthMax);

            //if (battleSmallShip1.Ability is IBattle battle)
            //{
            //    battle.Shoot(new Coordinate(1, 2, 4));
            //}

            var game = new Game(0);

            game.Run();

            var loadGame = Game.Load(0);

            loadGame.Run();

            Console.ReadKey();
        }
    }
}

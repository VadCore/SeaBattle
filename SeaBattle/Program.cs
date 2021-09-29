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
            

            var game = new Game(0);

            game.Run();

            var loadGame = Game.Load(0);

            loadGame.Run();
            

            Console.ReadKey();
        }
    }
}

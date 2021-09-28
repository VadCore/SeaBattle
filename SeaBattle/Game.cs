using Newtonsoft.Json;
using SeaBattle.Enums;
using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SeaBattle
{   [KnownType(typeof(Ship<SmallShip>)), KnownType(typeof(Ship<MiddleShip>)), KnownType(typeof(Ship<BigShip>)), KnownType(typeof(Ship<MiddleShip>))]
    [KnownType(typeof(Player))]
    public class Game
    {
        private static readonly string pathBegin = @"..\..\..\Data\Game";
        private static readonly string pathEnd = "_SaveData.json";

        public Board Board { get; } = new Board();

        public int Id { get; set; }

        public int Turn { get;}

        public Player[] Players { get; set; }

        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        };

        public Game(int id)
        {
            Id = id;
            Setup();
        }

        public Game()
        {
        }

        public void Setup()
        {
            Console.WriteLine("Setup game!");

            //var ships = new List<IShip>();

            Players = new Player[] { new Player(2, this), new Player(3, this) };

            Players[0].Units.Add(SmallShip.Create<Battle>(0, Players[0], new Coordinate(0, 1, 2), Rotation.Horizontal));
            Players[1].Units.Add(BigShip.Create<Battle>(0, Players[1], new Coordinate(1, 2, 4), Rotation.Vertical));
        }

        public void Run()
        {

            Console.WriteLine("Run Game!");

            //Console.WriteLine("Max Health " + SmallShip<Battle>.healthMax);
            //Console.WriteLine("Max HealthMax  " + battleBigShip1.HealthMax);

            if (Players[0].Units[0].Ability is IBattle battle)
            {
                battle.Shoot(new Coordinate(1, 2, 4));
            }

            Save();
        }


        public void Save()
        {

            Console.WriteLine(Board.ToString());

            string serializedGame = JsonConvert.SerializeObject(this, settings);

            File.WriteAllText(pathBegin + Id.ToString() + pathEnd, serializedGame, Encoding.UTF8);

            Console.WriteLine("Serialization process");

            Console.WriteLine("Result: " + serializedGame);
        }

        public static Game Load(int gameId)
        {
            var path = pathBegin + gameId.ToString() + pathEnd;

            string serializedGame;

            

            if (File.Exists(path))
            {
                serializedGame = File.ReadAllText(path);

            }
            else
            {
                Console.WriteLine("This game not found!");
                return null;
            }



            Console.WriteLine("Start Desirialize");

            var result = JsonConvert.DeserializeObject<Game>(serializedGame, settings);

            //var returnAfterDeserialize = result.Players[1].Units[0];
            //Console.WriteLine("My Length: " + returnAfterDeserialize.Length + " my rotation: " + returnAfterDeserialize.Rotation);


            //Console.WriteLine("End Result");

            ////string jsonString = JsonSerializer.Serialize(this, options);


            ////var result = JsonSerializer.Deserialize<Game>(jsonString);




            //if (result.Players[0].Units[0].Ability is IBattle battle)
            //{
            //    battle.Shoot(new Coordinate(1, 2, 4));
            //    Console.WriteLine("I'am shoot!");
            //}

            //Console.WriteLine(result.Board);

            return result;
        }


    }
}

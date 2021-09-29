using SeaBattle.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SeaBattle
{
    public class Player
    {
        public int Id { get; set; }
        public int GameId { get; set; }

        //[JsonIgnore]
        public Game Game { get; set; }

        public IList<IUnit> Units { get; set; } = new List<IUnit>();

        public Player(int id, Game game)
        {
            Id = id;
            GameId = game.Id;
            Game = game;
        }

        public Player()
        {
        }
    }
}

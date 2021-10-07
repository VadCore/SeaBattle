using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
    public class Player : BaseEntity
    {
        public int BoardId { get; set; }
        public string Nick { get; set; }
        public int ActiveUnitsCount { get; set; }

        public Board Board { get; set; }
        public IList<Ship> Ships { get; set; } = new List<Ship>();

        public Player(string nick, int boardId)
        {
            Nick = nick;
            BoardId = boardId;
        }

        public Player()
        {
        }
    }
}

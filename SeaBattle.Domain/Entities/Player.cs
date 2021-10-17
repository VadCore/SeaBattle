using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Domain.Entities
{
    public class Player : BaseEntity<Player>
    {
        public int BoardId { get; set; }
        public string Nick { get; set; }
        public int ActiveUnitsCount { get; set; }

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

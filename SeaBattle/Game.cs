using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public static class Game
    {
        public static Board Board { get; set; } = new Board(5, 6);
    }
}

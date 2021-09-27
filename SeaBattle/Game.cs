using System;
using System.Collections.Generic;
using System.Text;

namespace SeaBattle
{
    public static class Game
    {
        public static Board Board { get; } = new Board(15, 15);
        public static int Turn { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaBattle.Common
{
    class DataHandler<TShip, TBoard> where TShip : Ship<TShip>, new()
    {
        private static readonly string pathToShip = @"D:\.NET Nix Projects\SeaBattle1\Ships.json";
        private static readonly string pathToBoard = @"D:\.NET Nix Projects\SeaBattle1\Boards.json";

        public static void SaveAllToFileAsync()
        {
            
        }
}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.UI.Configs
{
    public class AppOptions : IAppOptions
    {
        public string JsonDataPath { get; set; }

        public string DbConnectionString { get; set; }

        public bool IsSerializable { get; set; }
    }
}

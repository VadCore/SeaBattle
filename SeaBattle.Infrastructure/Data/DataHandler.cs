using Newtonsoft.Json;
using SeaBattle.Infrastructure.Common;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Data
{
    public class DataHandler<TContext> : IDataHandler<TContext> where TContext : DataContext<TContext>
    {
        private static readonly string path = @"..\..\..\..\SeaBattle.Infrastructure\Data\SaveData.json";

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions//(JsonSerializerDefaults.Web) 

        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,

        };

        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        };


        public void SaveContext(TContext context)
        {

            //string serializedContext = JsonSerializer.Serialize(context, options);

            string serializedContext = JsonConvert.SerializeObject(context, settings);

            File.WriteAllText(path, serializedContext, Encoding.UTF8);

            Console.WriteLine("Serialization process");

            //Console.WriteLine("Result: " + serializedContext);
        }

        public TContext Load()
        {
            string serializedContext;

            if (File.Exists(path))
            {
                serializedContext = File.ReadAllText(path);
            }
            else
            {
                Console.WriteLine("This game not found!");
                return null;
            }

            Console.WriteLine("Start Desirialize");

            //var context = JsonSerializer.Deserialize<TContext>(serializedContext, options);

            var context = JsonConvert.DeserializeObject<TContext>(serializedContext, settings);

            context.DataHandler = this;

            return context;
        }
    }
}

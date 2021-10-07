using SeaBattle.Infrastructure.Common;
using SeaBattle.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.Data
{
    public class DataHandler<TContext> : IDataHandler<TContext> where TContext : DataContext<TContext>
    {
        private static readonly string path = @"..\..\..\..\SeaBattle.Infrastructure\Data\SaveData.json";

        private static readonly JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };


        public void SaveContext(TContext context)
        {

            string serializedContext = JsonSerializer.Serialize(context, options);

            File.WriteAllText(path, serializedContext, Encoding.UTF8);
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

            var context = JsonSerializer.Deserialize<TContext>(serializedContext, options);

            context.DataHandler = this;

            return context;
        }
    }
}

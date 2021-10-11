using Newtonsoft.Json;
using SeaBattle.Domain.Interfaces;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.Infrastructure.Repositories;
using SeaBattle.UI.Configs;
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
    public class DataHandler : IDataHandler
    {
        //private static readonly string path = @"..\..\..\..\SeaBattle.Infrastructure\Data\SaveData.json";

        public IAppOptions _appOptions;

        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        };

        public DataHandler(IAppOptions appOptions)
        {
            _appOptions = appOptions;
        }

        public void SaveContext(IUnitOfWork unitOfWork)
        {
            string serializedContext = JsonConvert.SerializeObject(unitOfWork, settings);

            File.WriteAllText(_appOptions.JsonDataPath, serializedContext, Encoding.UTF8);
        }

        public IUnitOfWork Load()
        {
            string serializedContext;

            if (File.Exists(_appOptions.JsonDataPath))
            {
                serializedContext = File.ReadAllText(_appOptions.JsonDataPath);
            }
            else
            {
                return null;
            }

            var unitOfWork = JsonConvert.DeserializeObject<UnitOfWork>(serializedContext, settings);

            unitOfWork.DataHandler = this;
            
            return unitOfWork;
        }
    }
}

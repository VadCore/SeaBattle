﻿using Newtonsoft.Json;
using SeaBattle.Domain.Constants;
using SeaBattle.Domain.Entities;
using SeaBattle.Infrastructure.Interfaces;
using SeaBattle.UI.Configs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SeaBattle.Infrastructure.Serialization
{
    public abstract class SerializationContext<TContext> where TContext : SerializationContext<TContext>
    {
        private static readonly FieldInfo[] fieldInfos = GetFieldInfos();
        private static readonly IDictionary<Type, FieldInfo> entityTypeFieldInfo = GetEntityTypeFieldInfos();
        
        [JsonIgnore]
        public string JsonDataPath { get; init; }

        private static JsonSerializerSettings settings = new JsonSerializerSettings
        {
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto,
            ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver()
        };

        public SerializationContext()
        {
            foreach (var field in fieldInfos)
            {
                if (field.GetValue(this) is null)
                {
                    field.SetValue(this, Activator.CreateInstance(field.FieldType));
                }
            }
        }

        public SerializationContext(string jsonDataPath) : this()
        {
            JsonDataPath = jsonDataPath;
        }

        public static TContext Load(string jsonDataPath)
        {
            string serializedContext;

            if (File.Exists(jsonDataPath))
            {
                serializedContext = File.ReadAllText(jsonDataPath);
            }
            else
            {
                return null;
            }

            return JsonConvert.DeserializeObject<TContext>(serializedContext, settings);
        }

        

        public SerializationSet<TEntity> Set<TEntity>() where TEntity : BaseEntity<TEntity>
        {
            return (SerializationSet<TEntity>)entityTypeFieldInfo[typeof(TEntity)].GetValue(this);
        }

        private static FieldInfo[] GetFieldInfos()
        {
            return typeof(TContext).GetFields(ReflectionConstants.PublicInstance).ToArray();
        }

        private static IDictionary<Type, FieldInfo> GetEntityTypeFieldInfos()
        {
            var collectionEntitiesByEntityType = new Dictionary<Type, FieldInfo>();

            foreach (var field in fieldInfos)
            {
                var type = field.FieldType.GetGenericArguments().FirstOrDefault();
                collectionEntitiesByEntityType.Add(type, field);
            }

            return collectionEntitiesByEntityType;
        }

        public void Commit()
        {
            string serializedContext = JsonConvert.SerializeObject(this, settings);

            File.WriteAllText(JsonDataPath, serializedContext, Encoding.UTF8);
        }
    }
}
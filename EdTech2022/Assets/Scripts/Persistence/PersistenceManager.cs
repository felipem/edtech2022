﻿using System.IO;
using Newtonsoft.Json;
using Persistence.Serializables;
using UnityEngine;
using World;
using World.Resource;
using World.Tiles;

namespace Persistence
{
    public class PersistenceManager : MonoBehaviour
    {

        private static JsonSerializerSettings serializationSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.None,
            NullValueHandling = NullValueHandling.Ignore
        };

      
        private ServerWorld selectedWorld;

        private static bool created = false;
        private bool initialSurveyComplete = false;
        void Awake()
        {
            if (!created)
            {
                DontDestroyOnLoad(gameObject);
                created = true;
            }
            else
            {
                // Navigated back to main menu so we can reset "selected world"
                SelectedWorld = null;
                Destroy(gameObject);
            }
        }

      
        public void SaveGameState(ServerWorld world)
        {
            APIService.Instance.UpdateWorld(world, world.id);
        }
        
        
        public void DeleteWorld(ServerWorld serverWorld)
        {
            APIService.Instance.DeleteWorld(serverWorld.id);
        }

    

        public ServerWorld SelectedWorld
        {
            get => selectedWorld;
            set => selectedWorld = value;
        }

        public bool InitialSurveyComplete
        {
            get => initialSurveyComplete;
            set => initialSurveyComplete = value;
        }
    }
}
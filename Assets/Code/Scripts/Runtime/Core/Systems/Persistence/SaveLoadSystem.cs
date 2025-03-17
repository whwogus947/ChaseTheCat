using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    public interface ISaveable
    {
        
    }

    public interface IBind<TData> where TData : ISaveable
    {
        void Bind(TData data);
    }

    [DefaultExecutionOrder(-500)]
    public class SaveLoadSystem : MonoBehaviour
    {
        [SerializeField] public FlotData gameData;
        public AbilityController controller;

        IDataService dataService;

        void Awake()
        {
            dataService = new FileDataService(new JsonSerializer());

            controller.Bind(gameData.Book);
            // Debug.Log("Bind!");
        }

        void Start()
        {
            
        }

        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Menu") return;
            Debug.Log("scene loaded");

            // Bind<PlayerCube, CubeData>(gameData.cubeData);
        }

        void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null)
            {
                if (data == null)
                {
                    // data = new TData { Id = entity.Id };
                }
                entity.Bind(data);
            }
        }

        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                // var data = datas.FirstOrDefault(d => d.Id == entity.Id);
                // if (data == null)
                // {
                //     data = new TData { Id = entity.Id };
                //     datas.Add(data);
                // }
                // entity.Bind(data);
            }
        }

        public void NewGame()
        {
            // gameData = new GameData
            // {
            //     Name = "Game",
            //     CurrentLevelName = "Demo"
            // };
            // SceneManager.LoadScene(gameData.CurrentLevelName);
        }

        public void SaveGame()
        {
            // controller.database.ToBook();
            dataService.Save(gameData);
        }

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);
            foreach (var (type, savableData) in gameData.Book.savedAbilities)
            {
                for (int i = 0; i < savableData.Count; i++)
                {
                    var savedData = savableData[i];
                    int id = savedData.id;
                }
            }
            controller.Bind(gameData.Book);
            foreach (var (type, savableData) in gameData.Book.savedAbilities)
            {
                for (int i = 0; i < savableData.Count; i++)
                {
                    var savedData = savableData[i];
                    int id = savedData.id;
                }
            }
            Dictionary<Type, List<SavableProperty>> clone = new(gameData.Book.savedAbilities);
            controller.FromSavedData(clone);
            foreach (var (type, savableData) in gameData.Book.savedAbilities)
            {
                for (int i = 0; i < savableData.Count; i++)
                {
                    var savedData = savableData[i];
                    int id = savedData.id;
                }
            }
        }

        public void ReloadGame() => LoadGame(gameData.Name);

        public void DeleteGame(string gameName) => dataService.Delete(gameName);
    }
}

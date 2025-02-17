using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    public interface ISaveable
    {
        SerializableGuid Id { get; set; }
    }

    public interface IBind<TData> where TData : ISaveable
    {
        SerializableGuid Id { get; set; }
        void Bind(TData data);
    }

    public class SaveLoadSystem : MonoBehaviour
    {
        [SerializeField] public FlotData gameData;

        IDataService dataService;

        void Awake()
        {
            dataService = new FileDataService(new JsonSerializer());
        }

        void Start() => NewGame();

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
                    data = new TData { Id = entity.Id };
                }
                entity.Bind(data);
            }
        }

        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);

            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(d => d.Id == entity.Id);
                if (data == null)
                {
                    data = new TData { Id = entity.Id };
                    datas.Add(data);
                }
                entity.Bind(data);
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

        public void SaveGame() => dataService.Save(gameData);

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);

            // if (String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
            // {
            //     gameData.CurrentLevelName = "Demo";
            // }

            // Bind<PlayerCube, CubeData>(gameData.cubeData);

            // SceneManager.LoadScene(gameData.CurrentLevelName);
        }

        public void ReloadGame() => LoadGame(gameData.Name);

        public void DeleteGame(string gameName) => dataService.Delete(gameName);
    }
}

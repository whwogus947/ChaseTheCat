using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class MapManager : UniqueSingleton<MapManager>
    {
        public MapNamePopup mapNamePopup;
        public GameObject guide;
        public VoidChannelSO mapSelectEvent;

        private MapPalette palette;
        private MapCreator creator;
        private bool showTutorial = false;

        protected override void Initialize()
        {
            creator = GetComponentInChildren<MapCreator>(true);
            palette = GetComponentInChildren<MapPalette>(true);
        }

        void Start()
        {
            palette.AddEvent(CloseUI);
            mapSelectEvent.UniqueEvent(OpenUI);
        }

        private void OnDestroy()
        {
            mapSelectEvent.RemoveEvent(OpenUI);
        }

        public void OpenUI()
        {
            Time.timeScale = 0f;
            palette.gameObject.SetActive(true);
        }

        public void CloseUI(Button btn) => palette.gameObject.SetActive(false);

        public void LoadScene(SceneHandlerSO scene)
        {
            mapNamePopup.Setup(scene.mapName, scene.mapIcon);
            LoadSceneUniTask(scene.SceneName).Forget();
            showTutorial = false;
        }

        public void LoadSceneWithTutorial(SceneHandlerSO scene)
        {
            mapNamePopup.Setup(scene.mapName, scene.mapIcon);
            LoadSceneUniTask(scene.SceneName).Forget();
            showTutorial = true;
        }

        private async UniTaskVoid LoadSceneUniTask(string sceneName)
        {
            try
            {
                AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
                load.completed += OnLoadComplete;
                await load.ToUniTask(Progress.Create<float>(x => Debug.Log(x)));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error!: {e.Message}");
            }
        }

        private void OnLoadComplete(AsyncOperation operation)
        {
            FindAnyObjectByType<PlayerBehaviour>().ResetMaxHeight();
            OnLoadProcess().Forget();
        }

        private async UniTaskVoid OnLoadProcess()
        {
            var popupClone = Instantiate(mapNamePopup);
            if (!showTutorial)
                return;

            await UniTask.WaitUntil(() => popupClone == null);
            if (gameObject != null)
                Instantiate(guide);
        }
    }
}
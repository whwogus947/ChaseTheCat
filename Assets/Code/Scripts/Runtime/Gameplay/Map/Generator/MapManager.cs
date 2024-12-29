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

        private void OnDestroy() {
            mapSelectEvent.RemoveEvent(OpenUI);
        }

        public void OpenUI()
        {
            Debug.Log("Open");
            palette.gameObject.SetActive(true);
        }

        public void CloseUI(Button btn) => palette.gameObject.SetActive(false);

        public void LoadScene(SceneHandlerSO scene)
        {
            LoadSceneUniTask(scene.SceneName).Forget();
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
            OnLoadProcess().Forget();
        }

        private async UniTaskVoid OnLoadProcess()
        {
            var popupClone = Instantiate(mapNamePopup);
            await UniTask.WaitUntil(() => popupClone == null);
            if (gameObject != null)
                Instantiate(guide);
        }
    }
}
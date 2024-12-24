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

        private MapPalette palette;
        private MapViewer viewer;
        private MapCreator creator;

        protected override void Initialize()
        {
            viewer = GetComponentInChildren<MapViewer>(true);
            creator = GetComponentInChildren<MapCreator>(true);
            palette = GetComponentInChildren<MapPalette>(true);
        }

        void Start()
        {
            AddStageSelectionButtonEvent();
            palette.AddEvent(CloseUI);
        }

        public void OpenUI()
        {
            palette.gameObject.SetActive(true);

            var rootStage = creator.GetRootStage();
            // viewer.DrawAllStages(rootStage);
        }

        public void CloseUI(Button btn) => palette.gameObject.SetActive(false);

        public void LoadScene(SceneHandlerSO scene)
        {
            LoadSceneUniTask(scene.SceneName).Forget();
        }

        private void AddStageSelectionButtonEvent()
        {
            viewer.AddOnClickButton(LoadScene);
        }

        private void LoadScene(StageData data)
        {
            LoadSceneUniTask(data.SceneName).Forget();
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
            Instantiate(guide);
        }
    }
}
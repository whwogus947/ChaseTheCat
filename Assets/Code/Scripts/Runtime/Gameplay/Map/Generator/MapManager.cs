using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    public class MapManager : MonoBehaviour
    {
        public MapNamePopup mapNamePopup;
        public GameObject guide;

        private MapViewer viewer;
        private MapCreator creator;

        private void Awake()
        {
            viewer = GetComponentInChildren<MapViewer>(true);
            creator = GetComponentInChildren<MapCreator>(true);
        }

        void Start()
        {
            AddStageSelectionButtonEvent();
        }

        public void OpenUI()
        {
            viewer.gameObject.SetActive(true);

            var rootStage = creator.GetRootStage();
            viewer.DrawAllStages(rootStage);
        }

        public void CloseUI() => viewer.gameObject.SetActive(false);

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
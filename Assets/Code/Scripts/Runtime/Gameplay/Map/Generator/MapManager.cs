using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    public class MapManager : MonoBehaviour
    {
        private MapViewer viewer;
        private MapCreator creator;

        private void Awake()
        {
            viewer = GetComponentInChildren<MapViewer>(true);
            creator = GetComponentInChildren<MapCreator>(true);
        }

        void Start()
        {
            OnSelectStage();
        }

        public void OpenUI()
        {
            viewer.gameObject.SetActive(true);

            var rootStage = creator.GetRootStage();
            viewer.DrawAllStages(rootStage);
        }

        public void CloseUI() => viewer.gameObject.SetActive(false);

        private void OnSelectStage()
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
                await load.ToUniTask(Progress.Create<float>(x => Debug.Log(x)));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error!: {e.Message}");
            }
        }
    }
}
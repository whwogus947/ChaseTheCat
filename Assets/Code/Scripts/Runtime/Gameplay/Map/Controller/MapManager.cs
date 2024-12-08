using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    public class MapManager : MonoBehaviour
    {
        private MapViewer viewer;
        private MapCreator creator;

        void Start()
        {
            viewer = GetComponentInChildren<MapViewer>(true);
            creator = GetComponentInChildren<MapCreator>(true);

            OnSelectStage();
        }

        private void OnSelectStage()
        {
            viewer.AddOnClickButton(LoadScene);
        }

        private void LoadScene(StageData data)
        {
            LoadSceneUniTask(data.scene.Name).Forget();
        }

        private async UniTaskVoid LoadSceneUniTask(string sceneName)
        {
            try
            {
                AsyncOperation load = SceneManager.LoadSceneAsync(sceneName);
                await load.ToUniTask(Progress.Create<float>(x => Debug.Log(x)));
                Debug.Log($"{sceneName} 씬 로드 완료");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"씬 로드 중 오류 발생: {e.Message}");
            }
        }
    }
}
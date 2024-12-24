using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Scene Loader", menuName = "Cum2usGameDev/Map/Scene")]
    public class SceneHandlerSO : ScriptableObject
    {
        public string SceneName => scene.Name;
        
        [SerializeField] private SceneReference scene;
        
        public void LoadScene()
        {
            SceneManager.LoadScene(scene.Name);
        }
    }
}

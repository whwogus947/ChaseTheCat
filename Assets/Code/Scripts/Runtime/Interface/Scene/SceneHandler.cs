using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Scene Loader", menuName = "Cum2usGameDev/UI/SceneLoader")]
    public class SceneHandler : ScriptableObject
    {
        [SerializeField] private SceneReference scene;
        
        public void LoadScene()
        {
            SceneManager.LoadScene(scene.Name);
        }
    }
}

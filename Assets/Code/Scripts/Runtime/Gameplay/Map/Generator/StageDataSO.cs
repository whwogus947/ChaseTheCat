using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Stage Data", menuName = "Cum2usGameDev/Map/Stage/Data")]
    public class StageDataSO : ScriptableObject
    {
        public Sprite unvisited;
        public Sprite visited;
        public Sprite visitable;
        public SceneHandlerSO scene;

        public void Load()
        {
            scene.LoadScene();
        }
    }
}

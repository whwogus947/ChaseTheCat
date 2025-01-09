using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Region Button Data", menuName = "Cum2usGameDev/Map/RegionButtonData")]
    public class RegionButtonDataSO : ScriptableObject
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

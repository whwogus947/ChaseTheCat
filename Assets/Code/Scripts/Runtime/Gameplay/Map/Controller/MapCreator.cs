using Eflatun.SceneReference;
using UnityEngine;

namespace Com2usGameDev
{
    public class MapCreator : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }

    public class StageData
    {
        public SceneReference scene;
        public (int room, int level) location;
    }
}

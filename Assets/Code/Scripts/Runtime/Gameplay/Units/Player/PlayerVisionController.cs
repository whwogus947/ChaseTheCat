using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerVisionController : MonoBehaviour
    {
        private Camera mainCam;
        
        void Start()
        {
            mainCam = Camera.main;
        }

        void Update()
        {
            var originPos = transform.position;
            var xSize = 8f / 9 * 16;
            Vector3 playerPos = new(Mathf.FloorToInt((originPos.x + xSize / 2) / xSize) * xSize, Mathf.FloorToInt((originPos.y + 4) / 8f) * 8f, 0)
            {
                z = -10
            };
            mainCam.transform.position = playerPos;
        }
    }
}

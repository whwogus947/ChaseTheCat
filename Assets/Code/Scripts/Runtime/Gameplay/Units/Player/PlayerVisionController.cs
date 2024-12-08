using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerVisionController : MonoBehaviour
    {
        public Vector2 offset;
        public Transform background;

        private Camera mainCam;
        private float ySize;
        private float xSize ;
        
        void Start()
        {
            mainCam = Camera.main;
            ySize = mainCam.orthographicSize * 2f;
            xSize = ySize / 9 * 16f;
        }

        void Update()
        {
            var originPos = transform.position;
            Vector3 playerPos = new(Mathf.FloorToInt(originPos.x / xSize + 0.5f) * xSize + offset.x, Mathf.FloorToInt(originPos.y / ySize + 0.5f) * ySize + offset.y, 0)
            {
                z = -10
            };
            mainCam.transform.position = playerPos;
            background.localPosition = playerPos.y * 0.33f * Vector3.up;
        }
    }
}

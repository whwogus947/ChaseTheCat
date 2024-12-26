using UnityEngine;

namespace Com2usGameDev
{
    public class SlingshotController : MonoBehaviour
    {
        public LineRenderer[] lines;
        public Transform handlePrefab;
        public float LineDrawingTimer {get; set;} = 0f;

        private Transform handleStorage;

        void Start()
        {
            handleStorage = GetComponentInParent<WeaponPlacer>().leftHand;
            DrawLines();
            ResetLines();
        }

        void Update()
        {
            if (LineDrawingTimer > 0)
            {
                LineDrawingTimer -= Time.deltaTime;
                DrawLines();
                if (LineDrawingTimer <= 0)
                {
                    ResetLines();
                }
            }
        }

        private void DrawLines()
        {
            if (handlePrefab.parent != handleStorage)
            {
                handlePrefab.SetParent(handleStorage);
                handlePrefab.localPosition = Vector3.zero;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetPosition(0, handleStorage.position);
                lines[i].SetPosition(1, lines[i].transform.position);
            }
        }

        private void ResetLines()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i].SetPosition(0, Vector3.zero);
            }
            for (int i = 0; i < lines.Length; i++)
            {
                Vector3 localPosition = lines[(i + 1) % 2].GetPosition(0) - lines[i].GetPosition(0);
                lines[i].SetPosition(0, Vector3.zero);
                lines[i].SetPosition(1, localPosition);
            }
            handlePrefab.SetParent(transform);
            handlePrefab.position = (lines[0].transform.position + lines[1].transform.position) / 2f;
        }
    }
}

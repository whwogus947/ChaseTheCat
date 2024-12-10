using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Vector 2D", menuName = "Cum2usGameDev/LinearValue/Vector2D")]
    public class ClampedVector2dSO : ScriptableObject
    {
        public bool isControllable;
        public Vector2 Vector2
        {
            get
            {
                return Vector2.ClampMagnitude(_vector2, 1);
            }
            set { _vector2 = value; }
        }
        public int X => Mathf.RoundToInt(_vector2.x);
        public float power;
        public LayerMask groundLayer;

        private Vector2 _vector2;
    }
}

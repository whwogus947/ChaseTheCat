using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Layers", menuName = "Cum2usGameDev/Core/LayerData")]
    public class LayersSO : ScriptableObject
    {
        public LayerMask ground;
        public LayerMask target;
        public LayerMask npc;
        public LayerMask weapon;
    }
}

using UnityEngine;

namespace Com2usGameDev
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MapPassage : MonoBehaviour, ISpawnable
    {
        public VoidChannelSO mapConverter;
        public GameObject Spawnable => gameObject;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            mapConverter.Invoke();
        }
    }
}

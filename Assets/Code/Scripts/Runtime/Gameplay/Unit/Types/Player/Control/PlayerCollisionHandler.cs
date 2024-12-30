using UnityEngine;

namespace Com2usGameDev
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private PlayerBehaviour behaviour;
        private PlayerController controller;
        
        void Start()
        {
            controller = GetComponent<PlayerController>();
            behaviour = GetComponent<PlayerBehaviour>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.CompareTag("Enemy"))
            {
                controller.enabled = false;
                behaviour.GetComponent<Rigidbody2D>().AddForce((transform.position - other.transform.position).normalized * 100f + Vector3.up * 200f);
                var fx = behaviour.pool.GetPooledObject(behaviour.monsterCollisionEffect);
                if (other.contactCount > 0)
                {
                    fx.transform.position = other.GetContact(0).point;
                    if (fx is TwinklePoolItem item)
                    {
                        item.StartTwinkle();
                    }
                    else
                    {
                        fx.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                controller.enabled = true;
            }
        }
    }
}

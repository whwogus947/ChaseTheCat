using UnityEngine;

namespace Com2usGameDev
{
    public class DTTest : MonoBehaviour
    {
        public DamageTextSO damageText;

        void OnEnable()
        {
            damageText.Emit(Random.Range(0, 9999), transform.position);
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

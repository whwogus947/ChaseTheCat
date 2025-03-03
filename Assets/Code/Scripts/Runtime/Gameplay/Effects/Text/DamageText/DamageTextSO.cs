using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Com2usGameDev
{
    [CreateAssetMenu(fileName = "Damage Text", menuName = "Cum2usGameDev/FX/DamageText")]
    public class DamageTextSO : ScriptableObject
    {
        [SerializeField] private TMP_Text damageTextPrefab;
        [SerializeField] private float offsetY;
        [SerializeField] private float transitSpeed;
        [SerializeField] private GameObject storageBase;
        [SerializeField] private Gradient gradient;

        private Transform textStorage;
        private Queue<TMP_Text> pool;

        public async void Emit(int p, Vector2 location)
        {
            SpawnClone();

            var clone = Dequeue();
            clone.transform.position = (Vector3)location + Vector3.up * offsetY;
            clone.SetText("{0}", p);

            float timer = 0f;
            float timeOut = 1f;
            while (timer < timeOut)
            {
                timer += Time.deltaTime;
                Color color = gradient.Evaluate(p / 9999f);
                color.a = (1 - timer) / timeOut;
                clone.color = color;
                await UniTask.Yield();
                if (clone == null)
                {
                    return;
                }
                clone.transform.position += Time.deltaTime * transitSpeed * Vector3.up;
            }

            Enqueue(clone);
        }

        private void SpawnClone()
        {
            if (textStorage == null)
            {
                pool = new();
                var storageBaseClone = Instantiate(storageBase);
                textStorage = storageBaseClone.GetComponentInChildren<Canvas>().transform;
            }
        }

        private TMP_Text Dequeue()
        {
            TMP_Text clone;

            if (pool.Count > 0)
            {
                clone = pool.Dequeue();
                if (clone == null)
                    clone = Instantiate(damageTextPrefab, textStorage);
            }
            else
                clone = Instantiate(damageTextPrefab, textStorage);

            clone.gameObject.SetActive(true);
            return clone;
        }

        private void Enqueue(TMP_Text clone)
        {
            clone.gameObject.SetActive(false);
            pool.Enqueue(clone);
        }
    }
}

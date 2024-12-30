using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class VibrateEffector : MonoBehaviour
    {
        public float duration;
        public float power;
        public float coolTime;

        private Vector3 originalPosition;
        private CancellationTokenSource shakeCts;
        private bool isPlayable = true;

        private void Awake()
        {
            originalPosition = transform.localPosition;
        }

        public void StartEffect()
        {
            if (!isPlayable)
                return;

            gameObject.SetActive(true);
            ShakeAsync(duration, power).Forget();
        }

        public async UniTask ShakeAsync(float duration, float strength, CancellationToken cancellationToken = default)
        {
            isPlayable = false;
            shakeCts?.Cancel();
            shakeCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            try
            {
                float elapsed = 0f;

                while (elapsed < duration)
                {
                    float currentStrength = strength * (1f - (elapsed / duration));

                    Vector3 randomPosition = originalPosition + UnityEngine.Random.insideUnitSphere * currentStrength;
                    transform.localPosition = randomPosition;

                    await UniTask.WaitForSeconds(0.05f, cancellationToken: shakeCts.Token);

                    elapsed += 0.05f;
                }
            }
            catch (OperationCanceledException)
            {

            }
            finally
            {
                transform.localPosition = originalPosition;
            }
            gameObject.SetActive(false);
            
            await UniTask.WaitForSeconds(coolTime, cancellationToken: shakeCts.Token);
            isPlayable = true;
        }

        public void StopShake()
        {
            shakeCts?.Cancel();
            shakeCts?.Dispose();
        }

        private void OnDestroy()
        {
            StopShake();
        }
    }
}

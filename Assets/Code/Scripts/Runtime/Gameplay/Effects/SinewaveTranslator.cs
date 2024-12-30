using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class SinewaveTranslator : MonoBehaviour
    {
        [SerializeField] private float amplitude = 1f;
        [SerializeField] private float frequency = 1f;
        [SerializeField] private bool autoStart = true; 

        private Vector3 startPosition;
        private CancellationTokenSource cts;

        private void Start()
        {
            startPosition = transform.position;
            if (autoStart)
            {
                StartSineMovement().Forget();
            }
        }

        private void OnDestroy()
        {
            cts?.Cancel();
            cts?.Dispose();
        }

        public async UniTaskVoid StartSineMovement()
        {
            cts?.Cancel();
            cts?.Dispose();
            cts = new CancellationTokenSource();

            try
            {
                float elapsedTime = 0f;

                while (!cts.Token.IsCancellationRequested)
                {
                    elapsedTime += Time.deltaTime;

                    float yOffset = amplitude * Mathf.Sin(frequency * elapsedTime * 2f * Mathf.PI);
                    transform.position = startPosition + new Vector3(0f, yOffset, 0f);

                    await UniTask.Yield(PlayerLoopTiming.Update, cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }

        public void StopMovement()
        {
            cts?.Cancel();
            transform.position = startPosition;
        }
    }
}

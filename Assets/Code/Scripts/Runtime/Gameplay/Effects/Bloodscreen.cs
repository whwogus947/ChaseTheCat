using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class Bloodscreen : MonoBehaviour
    {
        private Image targetImage;
        [SerializeField] private float frequency = 1f;
        [SerializeField] private float amplitude = 0.5f;
        [SerializeField] private float offset = 0.5f;

        private CancellationTokenSource _cancellationTokenSource;

        private void OnEnable()
        {
            targetImage = GetComponent<Image>();
            _cancellationTokenSource = new CancellationTokenSource();
            AnimateAlphaAsync(_cancellationTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
        }

        private async UniTaskVoid AnimateAlphaAsync(CancellationToken cancellationToken)
        {
            float elapsedTime = 0f;

            while (!cancellationToken.IsCancellationRequested)
            {
                float alpha = offset + amplitude * Mathf.Sin(2f * Mathf.PI * frequency * elapsedTime);
                alpha = Mathf.Clamp01(alpha);

                Color currentColor = targetImage.color;
                currentColor.a = alpha;
                targetImage.color = currentColor;

                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);

                elapsedTime += Time.deltaTime;
            }
        }
    }
}
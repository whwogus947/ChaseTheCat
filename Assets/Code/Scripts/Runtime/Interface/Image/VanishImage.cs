using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class VanishImage : MonoBehaviour, IVanishable
    {
        public bool hide;
        public bool shake;
        public Image main;
        public Image sub;
        public float MaxValue {get; set;}

        private CountdownTimer timer;
        private CanvasGroup canvasGroup;
        private readonly float shakeSize = 0.42f;
        private const float fadeTime = 3.5f;

        void Awake()
        {
            timer = new(0f);
            canvasGroup = GetComponent<CanvasGroup>();

            if (hide)
                SetSliderTransparency(0);
        }

        public void Open(float value)
        {
            main.fillAmount = value;
            sub.fillAmount = value;
            gameObject.SetActive(true);
        }

        void Update()
        {
            bool hasGap = Mathf.Abs(sub.fillAmount - main.fillAmount) > 0.01f;
            if (hasGap)
            {
                sub.fillAmount = Mathf.Lerp(sub.fillAmount, main.fillAmount, Time.deltaTime * 1f);
                if (sub.fillAmount - main.fillAmount < 0.01f)
                {
                    sub.fillAmount = main.fillAmount;
                    if (shake)
                        ShakePosition(0);
                }
            }

            if (!hide)
                return;

            if (!timer.IsFinished && hasGap)
            {
                timer.Tick();
                var leftover = Mathf.Min(0.2f, timer.TimeRemaining) / 0.2f;
                SetSliderTransparency(leftover);
                if (shake)
                {
                    var tempSize = timer.TimeRemaining / fadeTime;
                    float shakeStrength = Mathf.Max(0, (tempSize - 0.72f) * shakeSize * tempSize);
                    ShakePosition(shakeStrength);
                }
            }
        }

        public void SetValue(float value)
        {
            InvokeTimer();
            main.fillAmount = value / MaxValue;
        }

        private void InvokeTimer()
        {
            timer = new(fadeTime);
        }

        private void ShakePosition(float size)
        {
            transform.localPosition = Random.insideUnitCircle * size;
        }

        private void SetSliderTransparency(float alpha)
        {
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = alpha;
        }

        public void OnFadeaway()
        {
            gameObject.SetActive(false);
        }
    }
}

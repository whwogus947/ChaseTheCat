using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class VanishSlider : MonoBehaviour, IVanishable
    {
        public bool hide;
        public bool shake;
        public bool solo;

        private Slider main;
        private Slider sub;
        private CountdownTimer timer;
        private CanvasGroup canvasGroup;
        private readonly float shakeSize = 0.42f;
        private const float fadeTime = 3.5f;

        void Awake()
        {
            timer = new(0f);
            sub = transform.GetChild(0).GetComponent<Slider>();
            main = transform.GetChild(1).GetComponent<Slider>();
            canvasGroup = GetComponent<CanvasGroup>();

            if (hide)
                SetSliderTransparency(0);
        }

        public void Open(float value)
        {
            main.value = value;
            sub.value = value;
            gameObject.SetActive(true);
        }

        void Update()
        {
            bool hasGap = Mathf.Abs(sub.value - main.value) > 0.02f;
            if (hasGap)
            {
                sub.value = Mathf.Lerp(sub.value, main.value, Time.deltaTime * 1f);
                if (Mathf.Abs(sub.value - main.value) < 0.05f)
                {
                    sub.value = main.value;
                    ShakePosition(0);
                }
            }

            if (!hide)
                return;

            if (!timer.IsFinished && (hasGap || solo))
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
            main.value = value;
        }

        private void InvokeTimer()
        {
            if (timer.IsFinished)
            {
                timer = new(fadeTime);
                return;
            }
            timer.Reset();
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

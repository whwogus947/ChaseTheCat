using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class VanishSlider : MonoBehaviour
    {
        public bool hide;

        private Slider main;
        private Slider sub;
        private CountdownTimer timer;

        void Start()
        {
            timer = new(1f);
            sub = transform.GetChild(0).GetComponent<Slider>();
            main = transform.GetChild(1).GetComponent<Slider>();

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
            if (Mathf.Abs(sub.value - main.value) > 0.02f)
            {
                sub.value = Mathf.Lerp(sub.value, main.value, Time.deltaTime * 1f);
                if (sub.value - main.value < 0.05f)
                {
                    sub.value = main.value;
                }
            }

            if (!hide)
                return;

            if (!timer.IsFinished)
            {
                timer.Tick();
                var leftover = Mathf.Max(0.2f, timer.TimeRemaining) / 0.2f;
                SetSliderTransparency(leftover);
            }
        }

        public void SetValue(float value)
        {
            InvokeTimer();
            main.value = value;
        }

        private void InvokeTimer()
        {
            timer = new(0.5f);
        }

        private void SetSliderTransparency(float alpha)
        {
            CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            canvasGroup.alpha = alpha;
        }
    }
}

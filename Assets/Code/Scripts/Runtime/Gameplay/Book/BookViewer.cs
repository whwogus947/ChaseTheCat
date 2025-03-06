using PrimeTween;
using UnityEngine;

namespace Com2usGameDev
{
    public class BookViewer : MonoBehaviour
    {
        public RectTransform target;
        public float power;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
            // ApplyAdvancedBounceEffect(target);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Up()
        {
            Tween.PositionY(target, 540, duration: 0.5f, ease: Easing.Bounce(power));
        }

        public void Down()
        {
            Tween.PositionY(target, -540, duration: 0.5f, ease: Ease.Default);
        }

        public void ApplyBounceEffect(RectTransform targetRect)
        {
            // 기본 바운스 효과 (크기 변화)
            Sequence.Create()
                .Chain(Tween.Scale(targetRect, Vector3.one * 1.2f, duration: 0.1f, ease: Ease.OutBack))
                .Chain(Tween.Scale(targetRect, Vector3.one, duration: 0.1f, ease: Ease.OutBack));
        }

        public void ApplyAdvancedBounceEffect(RectTransform targetRect)
        {
            // 고급 바운스 효과 (위치와 크기 조합)
            Sequence.Create()
                .Chain(Tween.Scale(targetRect, Vector3.one * 1.2f, duration: 0.1f, ease: Ease.OutBack))
                .Chain(Tween.Position(targetRect, new Vector3(0, 10, 0), duration: 0.1f, ease: Ease.OutBounce))
                .Chain(Tween.Scale(targetRect, Vector3.one, duration: 0.1f, ease: Ease.OutBack))
                .Chain(Tween.Position(targetRect, Vector3.zero, duration: 0.1f, ease: Ease.OutBounce));
        }

        public void ApplyPulseEffect(RectTransform targetRect)
        {
            // 펄스 효과 (부드러운 확대/축소)
            Sequence.Create()
                .Chain(Tween.Scale(targetRect, Vector3.one * 1.1f, duration: 0.3f, ease: Ease.InOutSine))
                .Chain(Tween.Scale(targetRect, Vector3.one, duration: 0.3f, ease: Ease.InOutSine));
        }
    }
}

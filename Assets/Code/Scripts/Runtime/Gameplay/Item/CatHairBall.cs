using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class CatHairBall : MonoBehaviour
    {
        public float maxBrightness = 20;
        public float brightnessPower = 1f;
        public GachaHolder selectionUI;
        public GameObject particle;
        public Transform brightFX;
        public AudioChannelSO audioChannel;
        public AudioClip openSfx;

        private MaterialPropertyBlock propertyBlock;
        private Renderer render;
        private Animator animator;

        void Start()
        {
            propertyBlock = new MaterialPropertyBlock();
            render = GetComponentInChildren<Renderer>();
            animator = GetComponentInChildren<Animator>();

            SetBrightness(1);
        }

        public void Open()
        {
            audioChannel.Invoke(openSfx);
            OpenHairBallProcess().Forget();
        }

        public void InteractWithBall()
        {
            animator.Play("HairBall");
        }

        private async UniTaskVoid OpenHairBallProcess()
        {
            float brightness = 1f;
            float eventThreshold = maxBrightness * 0.4f;
            while (brightness < maxBrightness)
            {
                await UniTask.Yield();
                brightness += Time.deltaTime * brightnessPower;
                if (brightness > eventThreshold)
                {
                    particle.SetActive(true);
                    RotateAndScaleUp();
                    brightFX.gameObject.SetActive(true);
                }
                
                SetBrightness(brightness);
            }
            Instantiate(selectionUI).OpenGacha();
            Destroy(gameObject);
        }

        private void SetBrightness(float value)
        {
            render.GetPropertyBlock(propertyBlock);
            propertyBlock.SetFloat("_Power", value);
            render.SetPropertyBlock(propertyBlock);
        }

        private void RotateAndScaleUp()
        {
            var angle = brightFX.eulerAngles;
            angle.z += Time.deltaTime * 60f;
            brightFX.eulerAngles = angle;
            brightFX.localScale += 28f * Time.deltaTime * Vector3.one;
        }
    }
}

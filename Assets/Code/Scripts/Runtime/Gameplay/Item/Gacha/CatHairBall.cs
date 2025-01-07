using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Com2usGameDev
{
    public class CatHairBall : Sonorous, IInteractable
    {

        [Header("SFX")]
        [SerializeField] private GachaHolder selections;
        [SerializeField] private GameObject particle;
        [SerializeField] private Transform brightFX;

        [Header("SFX")]
        [SerializeField] private AudioClip openSound;

        [Header("Settings")]
        [SerializeField] private float maxBrightness = 20;
        [SerializeField] private float brightnessPower = 1f;

        private AbilityBundleSO abilityBundle;
        private bool isOpen = false;
        private MaterialPropertyBlock propertyBlock;
        private Renderer render;
        private Animator animator;

        void Start()
        {
            propertyBlock = new MaterialPropertyBlock();
            render = GetComponentInChildren<Renderer>();
            animator = GetComponentInChildren<Animator>();

            SetBrightness(1);
            isOpen = false;
        }

        public void SetAbility(AbilityBundleSO abilityBundle) => this.abilityBundle = abilityBundle;

        public void Open()
        {
            if (isOpen)
                return;

            isOpen = true;
            PlaySound(openSound);
            OpenHairBallProcess().Forget();
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
            Instantiate(selections).OpenGacha(abilityBundle.abilities);
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

        private void StartBallOpening()
        {
            animator.Play("Open");
        }

        public void Interact()
        {
            StartBallOpening();
        }
    }
}

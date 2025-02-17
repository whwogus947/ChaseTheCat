using UnityEngine;

namespace Com2usGameDev.Effect
{
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using Cysharp.Threading.Tasks;
    using System.Threading;

    [Serializable]
    public class ModuleSetting
    {
        public float invokeTime;
        public GameObject target;
    }

    [Serializable]
    public class CustomSetting
    {
        public float duration = 1f;
        public float invokeTime;
        public AnimationCurve curve = AnimationCurve.Linear(0, 1, 1, 0);
    }

    public class SkillFXHandler : MonoBehaviour
    {
        [SerializeField] private List<ModuleSetting> settings;
        [SerializeField] private CustomSetting scale;

        private CancellationTokenSource tokenSource;
        private float elapsedTime;

        private void OnEnable()
        {
            elapsedTime = 0;
            tokenSource = new();
            transform.localScale = Vector3.one;

            foreach (var setting in settings)
            {
                EffectTask(setting).Forget();
            }
            InactiveAfter(scale.duration).Forget();
        }

        private void Update()
        {
            elapsedTime += Time.deltaTime;
            ScaleUpProcess();
        }

        private void ScaleUpProcess()
        {
            var value = scale.curve.Evaluate(elapsedTime / scale.duration);
            transform.localScale = Vector3.one * value;
        }

        private async UniTaskVoid EffectTask(ModuleSetting setting)
        {
            await UniTask.WaitForSeconds(setting.invokeTime * scale.duration, cancellationToken: tokenSource.Token);
            setting.target.SetActive(true);
        }

        private async UniTaskVoid InactiveAfter(float time)
        {
            await UniTask.WaitForSeconds(time, cancellationToken: tokenSource.Token);
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            tokenSource?.Cancel();
        }
    }
}
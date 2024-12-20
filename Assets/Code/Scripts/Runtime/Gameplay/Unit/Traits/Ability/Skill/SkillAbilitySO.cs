using UnityEngine;

namespace Com2usGameDev
{
    public abstract class SkillAbilitySO : AbilitySO, ISkill
    {
        public override string AbilityType { get => nameof(SkillAbilitySO); }
        public Sprite grayIcon;
        public Sprite colorIcon;

        public SkillTypeSO skillType;
        public int maxLevel;
        public int epConsumption;
        public float maxCoolTime;
        public bool IsReady => coolTime <= 0;
        public float CoolTimeRatio => coolTime / maxCoolTime;

        private int level;
        private float coolTime;

        public override void OnAquire()
        {
            if (!IsMaxLevel())
            {
                Debug.Log("Skill Already Aquired");
                level++;
                PowerUp();
            }
        }

        public bool IsMaxLevel() => level == maxLevel;

        protected abstract void PowerUp();

        public override void OnDiscover()
        {
            level = 0;
            coolTime = maxCoolTime;
        }

        public void CoolDown()
        {
            if (coolTime > 0)
                coolTime = Mathf.Max(0, coolTime - Time.deltaTime);
        }

        public void Reset()
        {
            Debug.Log("Reset!");
            coolTime = maxCoolTime;
        }
    }
}
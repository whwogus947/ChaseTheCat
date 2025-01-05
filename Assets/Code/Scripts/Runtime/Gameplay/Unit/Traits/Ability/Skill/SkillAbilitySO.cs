using UnityEngine;

namespace Com2usGameDev
{
    public abstract class SkillAbilitySO : AbilitySO, ISkill
    {
        public override string AbilityType { get => nameof(SkillAbilitySO); }
        public Sprite grayIcon;
        public PoolItem fx;

        public SkillTypeSO skillType;
        public int maxLevel;
        public int epConsumption;
        public float maxCoolTime;
        public bool IsReady => coolTime <= 0;
        public float CoolTimeRatio => coolTime / maxCoolTime;

        protected int level;
        
        [SerializeField] private int originPower;
        private float coolTime;

        public override void OnAquire()
        {
            if (!IsMaxLevel())
            {
                Debug.Log("Skill Already Aquired");
                level++;
                if (IsMaxLevel())
                    IsObtainable = false;
                PowerUp();
            }
        }

        public bool IsMaxLevel() => level == maxLevel;

        protected abstract void PowerUp();

        public virtual int Power()
        {
            return originPower;
        }

        public override void OnDiscover()
        {
            base.OnDiscover();
            level = 1;
            coolTime = maxCoolTime;
        }

        public void CoolDown()
        {
            if (coolTime > 0)
                coolTime = Mathf.Max(0, coolTime - Time.deltaTime);
        }

        public void ResetCoolTime()
        {
            coolTime = maxCoolTime;
        }
    }
}
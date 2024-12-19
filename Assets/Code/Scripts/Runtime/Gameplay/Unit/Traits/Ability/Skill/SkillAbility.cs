using UnityEngine;

namespace Com2usGameDev
{
    public abstract class SkillAbility : AbilitySO, ISkill
    {
        public override string AbilityType { get => nameof(SkillAbility); }
        public Sprite foregroundIcon;
        public Sprite backgroundIcon;

        public SkillTypeSO skillType;
        public int maxLevel;
        public int epConsumption;
        public float coolTime;

        private int currentLevel;

        public override void OnAquire(AbilityController controller)
        {
            if (IsMaxLevel())
                return;

            if (!controller.HasAbility(this))
            {
                currentLevel = 0;
                OnDiscover();
                controller.AddAbility(this);
            }
            else
            {
                currentLevel++;
                PowerUp();
            }
        }

        public bool IsMaxLevel() => currentLevel == maxLevel;

        protected abstract void PowerUp();

        public virtual void OnDiscover()
        {

        }
    }
}
using UnityEngine;

namespace Com2usGameDev
{
    public class SkillViewGroup : AbilityViewGroup<SkillAbilitySO>
    {
        public Transform storage;
        public SkillType skillType;

        private SkillSlot slot;

        void Start()
        {
            slot = storage.GetComponentInChildren<SkillSlot>(true);
        }

        public override void AddAbility(SkillAbilitySO skill)
        {
            if (skill.skillType == null || skill.skillType == skillType.passive)
                return;

            SkillSlot newSlot = null;
            for (int i = 0; i < storage.childCount; i++)
            {
                if (!storage.GetChild(i).gameObject.activeSelf)
                {
                    var targetSlot = storage.GetChild(i);
                    newSlot = targetSlot.GetComponent<SkillSlot>();
                    break;
                }
            }

            if (newSlot == null)
                newSlot = Instantiate(slot, storage);

            newSlot.gameObject.SetActive(true);
            newSlot.SetSkill(skill);
        }
    }

    [System.Serializable]
    public class SkillType
    {
        public SkillTypeSO passive;
        public SkillTypeSO active;
    }
}

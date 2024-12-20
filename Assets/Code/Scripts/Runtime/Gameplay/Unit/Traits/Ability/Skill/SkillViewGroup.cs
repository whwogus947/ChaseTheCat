using UnityEngine;

namespace Com2usGameDev
{
    public class SkillViewGroup : MonoBehaviour
    {
        public Transform storage;

        private SkillSlot slot;

        void Start()
        {
            slot = storage.GetComponentInChildren<SkillSlot>(true);
        }

        public void AddSkill(SkillAbilitySO skill)
        {
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
}

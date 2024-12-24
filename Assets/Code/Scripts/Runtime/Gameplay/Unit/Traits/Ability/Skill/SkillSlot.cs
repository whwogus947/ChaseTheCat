using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class SkillSlot : MonoBehaviour
    {
        public Image grayIcon;
        public Image colorIcon;

        private SkillAbilitySO skill;

        void Update()
        {
            SkillCoolTime(skill.CoolTimeRatio);
        }

        public void SetSkill(SkillAbilitySO newSkill)
        {
            skill = newSkill;
            SetIcon(skill.grayIcon, skill.colorIcon);
        }

        private void SetIcon(Sprite grayIcon, Sprite colorIcon)
        {
            this.grayIcon.sprite = grayIcon;
            this.colorIcon.sprite = colorIcon;
        }

        private void SkillCoolTime(float progress)
        {
            grayIcon.fillAmount = progress;
        }
    }
}

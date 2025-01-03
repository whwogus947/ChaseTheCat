using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Com2usGameDev
{
    public class WeaponSlot : MonoBehaviour
    {
        public Image frame;
        public Image colorIcon;
        public TMP_Text leftCount;

        private WeaponAbilitySO weapon;

        public void SetWeapon(WeaponAbilitySO newWeapon)
        {
            weapon = newWeapon;
            SetIcon(weapon.frame, weapon.colorIcon);
        }

        private void SetIcon(Sprite frame, Sprite colorIcon)
        {
            this.frame.sprite = frame;
            this.colorIcon.sprite = colorIcon;

            bool active = weapon.IsLimited;
            if (active)
            {
                leftCount.gameObject.SetActive(true);
                leftCount.text = weapon.Count.ToString();
                weapon.onCountChanged += CountDown;
            }
            else
            {
                leftCount.gameObject.SetActive(false);
            }
        }

        private void CountDown(int count)
        {
            leftCount.text = count.ToString();
            if (count <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

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
        public WeaponAbilitySO Weapon {get; private set;}

        public void SetWeapon(WeaponAbilitySO newWeapon)
        {
            Weapon = newWeapon;
            SetIcon(Weapon.frame, Weapon.colorIcon);
        }

        private void SetIcon(Sprite frame, Sprite colorIcon)
        {
            this.frame.sprite = frame;
            this.colorIcon.sprite = colorIcon;

            bool active = Weapon.IsLimited;
            if (active)
            {
                leftCount.gameObject.SetActive(true);
                leftCount.text = Weapon.Count.ToString();
                Weapon.onCountChanged += CountDown;
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

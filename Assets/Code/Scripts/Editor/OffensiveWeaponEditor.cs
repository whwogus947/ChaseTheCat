using UnityEditor;

namespace Com2usGameDev
{
    [CustomEditor(typeof(OffensiveWeapon), true)]
    public class OffensiveWeaponEditor : Editor
    {
        private OffensiveWeapon offensiveWeapon;

        private void OnEnable()
        {
            offensiveWeapon = (OffensiveWeapon)target;
            if (offensiveWeapon.audioChannel == null)
                offensiveWeapon.audioChannel =  EditorToolset.Find<AudioChannelSO>("SFX Channel");
            if (offensiveWeapon.fXPool == null)
                offensiveWeapon.fXPool = EditorToolset.Find<VFXPool>("VFX Pool");
            if (offensiveWeapon.damageText == null)
                offensiveWeapon.damageText = EditorToolset.Find<DamageTextSO>("Damage Text");
        }
    }
}

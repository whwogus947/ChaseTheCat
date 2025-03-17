using System;
using UnityEngine;

namespace Com2usGameDev
{
    public abstract class NPCAbilitySO : AbilitySO
    {
        public override string AbilityTypeName => nameof(NPCAbilitySO);
        public override Type AbilityType => typeof(NPCAbilitySO);

        private SavableNPCData savableData;

        public override void OnAquire()
        {

        }

        public override void OnDiscover()
        {
            savableData = new(AbilityType, ID);
        }

        public override void ToSaveData(BookData book)
        {
            book.ToSaveData(savableData);
        }

        public override void ConvertDataToInstance(SavableProperty data)
        {
            
        }
    }
}

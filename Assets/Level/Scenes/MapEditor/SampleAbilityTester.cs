using System;
using System.Collections.Generic;
using UnityEngine;

namespace Com2usGameDev
{
    public class SampleAbilityTester : MonoBehaviour
    {
        public AbilityController controller;
        public List<NPCAbilitySO> npcs;
        public List<NPCAbilitySO> test;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            controller.GetContainer<NPCAbilitySO>().AddAquireListener(OnAddNPC);
            controller.GetContainer<NPCAbilitySO>().AddRemovalListener(OnRemoveNPC);

            foreach (var npc in npcs)
            {
                controller.AddAbility<NPCAbilitySO>(npc);
            }
        }

        void OnDestroy()
        {
            // controller.GetContainer<NPCAbilitySO>().AddRemovalListener(OnAddNPC);
            // controller.GetContainer<NPCAbilitySO>().AddRemovalListener(OnRemoveNPC);
        }

        private void OnAddNPC(NPCAbilitySO arg0)
        {
            test.Add(arg0);
        }

         private void OnRemoveNPC(NPCAbilitySO arg0)
        {
            test.Remove(arg0);
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

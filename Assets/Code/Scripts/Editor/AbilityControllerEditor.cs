using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    [CustomEditor(typeof(AbilityController))]
    public class AbilityControllerEditor : Editor
    {
        private AbilityController controller;

        private void OnEnable()
        {
            controller = (AbilityController)target;
            var abilities = EditorToolset.FindAll<AbilitySO>();
            foreach (var ability in abilities)
            {
                controller.database.Add(ability);
            }
        }
    }
}

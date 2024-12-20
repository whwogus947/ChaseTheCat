// #if UNITY_EDITOR
// using UnityEngine;
// #endif
// using UnityEditor;

// namespace Com2usGameDev
// {
//     public class ReadOnlyAttribute : PropertyAttribute { }

//     [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
//     public class ReadOnlyDrawer : PropertyDrawer
//     {
// #if UNITY_EDITOR
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             return EditorGUI.GetPropertyHeight(property, label, true);
//         }

//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             GUI.enabled = false;
//             EditorGUI.PropertyField(position, property, label, true);
//             GUI.enabled = true;
//         }
// #endif
//     }
// }

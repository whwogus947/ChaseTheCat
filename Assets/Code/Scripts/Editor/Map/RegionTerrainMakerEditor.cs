using System.IO;
using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    [CustomEditor(typeof(RegionTerrainMaker))]
    public class RegionTerrainMakerEditor : Editor
    {
        private RegionTerrainMaker maker;

        private void OnEnable()
        {
            maker = (RegionTerrainMaker)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (GUILayout.Button("Create Empty"))
            {
                var sampleClone = Instantiate(maker.sample, maker.transform);
                maker.SaveTempClone(sampleClone);
            }
            GUI.enabled = maker.SampleClone != null;
            if (GUILayout.Button("Remove Sample"))
            {
                maker.ResetClone();
            }
            GUI.enabled = true;

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Upperside", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("◀ Previous"))
            {
                Debug.Log("left");
            }
            if (GUILayout.Button(" [ Select ] "))
            {
                Debug.Log("Select");
            }
            if (GUILayout.Button("Next ▶"))
            {
                Debug.Log("right");
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Lowerside", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("◀ Previous"))
            {
                Debug.Log("left");
            }
            if (GUILayout.Button(" [ Select ] "))
            {
                Debug.Log("Select");
            }
            if (GUILayout.Button("Next ▶"))
            {
                Debug.Log("right");
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}

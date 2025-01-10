using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Com2usGameDev
{
    [CustomEditor(typeof(RegionTerrainMaker))]
    public class RegionTerrainMakerEditor : Editor
    {
        private RegionTerrainMaker maker;
        private SectionSiteSO[] sites;
        private string[] siteNames;
        private int selectedIndex = -1;

        private void OnEnable()
        {
            maker = (RegionTerrainMaker)target;

            sites = EditorToolset.FindAll<SectionSiteSO>();
            siteNames = sites.Select(item => item.name).ToArray();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if (maker.section != null)
            {
                selectedIndex = System.Array.IndexOf(sites, maker.section);
            }

            EditorGUI.BeginChangeCheck();
            selectedIndex = EditorGUILayout.Popup("Section", selectedIndex, siteNames);

            EditorGUILayout.Space(10);

            if (GUILayout.Button("New Template"))
            {
                var palette = maker.FindSectionPalette();

                var sampleClone = Instantiate(maker.template, palette);
                sampleClone.name = "Section " + siteNames[selectedIndex];
                Selection.activeObject = sampleClone;
                
                if (maker.TemplateClone != null)
                    maker.TemplateClone.SetActive(false);
                maker.SaveTempClone(sampleClone);
                sampleClone.gameObject.SetActive(true);
            }

            GUI.enabled = maker.TemplateClone != null;
            if (GUILayout.Button("Save Imitation") && IsValidIndex())
            {
                var path = SelectedSite.folderPath;
                maker.TemplateClone.AsPrefab(path, focus: true);
                maker.TemplateClone.SetActive(false);
            }
            if (GUILayout.Button("Remove Imitation"))
            {
                maker.ResetClone();
            }
            GUI.enabled = true;

            if (GUILayout.Button("Clear"))
            {
                maker.ClearAll();
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Explorer section changed");
                if (IsValidIndex())
                {
                    maker.section = SelectedSite;
                }
                EditorUtility.SetDirty(target);
            }
        }

        private bool IsValidIndex() => selectedIndex >= 0 && selectedIndex < sites.Length;

        private SectionSiteSO SelectedSite => sites[selectedIndex];
    }
}

using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Com2usGameDev
{
    [CustomEditor(typeof(RegionTerrainExplorer))]
    public class RegionTerrainExplorerEditor : Editor
    {
        private RegionTerrainExplorer explorer;
        private SectionSiteSO[] sites;
        private string[] siteNames;
        private int selectedIndex = -1;
        private GameObject[] tilemaps;

        private void OnEnable()
        {
            explorer = (RegionTerrainExplorer)target;

            sites = EditorToolset.FindAll<SectionSiteSO>();
            siteNames = sites.Select(item => item.name).ToArray();

            if (explorer.section != null)
                GetTileMaps();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (explorer.section != null)
            {
                selectedIndex = System.Array.IndexOf(sites, explorer.section);

                if (tilemaps == null)
                    GetTileMaps();
            }

            EditorGUI.BeginChangeCheck();
            var popupIndex = EditorGUILayout.Popup("Section", selectedIndex, siteNames);
            if (popupIndex != selectedIndex)
            {
                selectedIndex = popupIndex;
                
                Undo.RecordObject(target, "Explorer section changed");
                if (selectedIndex >= 0 && selectedIndex < sites.Length)
                {
                    explorer.section = sites[selectedIndex];
                }
                GetTileMaps();
            }

            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Showcase", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("◀ Previous"))
            {
                if (tilemaps.Length > 0)
                {
                    explorer.currentIndex = (explorer.currentIndex - 1 + tilemaps.Length) % tilemaps.Length;
                    Exhibit(explorer.currentIndex);
                }
            }
            if (GUILayout.Button(" [ Select ] "))
            {

            }
            if (GUILayout.Button("Next ▶"))
            {
                if (tilemaps.Length > 0)
                {
                    explorer.currentIndex = (explorer.currentIndex + 1) % tilemaps.Length;
                    Exhibit(explorer.currentIndex);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void GetTileMaps()
        {
            tilemaps = EditorToolset.FindAllPrefab<Tilemap>(explorer.section.folderPath);
        }

        private void Exhibit(int index)
        {
            if (selectedIndex == -1 || explorer.section == null)
                return;

            var siteName = siteNames[selectedIndex];
            var showcase = explorer.Showcase(siteName);
            for (int i = showcase.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(showcase.GetChild(i).gameObject);
            }
            var tileMap = Instantiate(tilemaps[index], showcase);
            tileMap.SetActive(true);
        }
    }
}
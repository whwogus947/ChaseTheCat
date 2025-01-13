using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

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
            EditorGUILayout.Space(10);
            var popupIndex = EditorGUILayout.Popup("Section", selectedIndex, siteNames);
            if (selectedIndex != popupIndex)
            {
                selectedIndex = popupIndex;
                SwitchSectionTarget();
            }

            EditorGUILayout.Space(10);

            if (GUILayout.Button("New Template") && IsValidIndex() && SelectedSite.template != null)
            {
                var palette = maker.FindSectionPalette();

                var sampleClone = Instantiate(SelectedSite.template, palette);
                sampleClone.name = "Section " + siteNames[selectedIndex];
                
                if (maker.TemplateClone != null)
                    maker.TemplateClone.SetActive(false);
                maker.SaveTempClone(sampleClone);
                sampleClone.gameObject.SetActive(true);
                Selection.activeObject = sampleClone;
            }

            GUI.enabled = maker.TemplateClone != null;
            if (GUILayout.Button("Save Imitation") && IsValidIndex())
            {
                var path = SelectedSite.tilemapPath;
                var clone = maker.TemplateClone.AsPrefab(path);
                maker.TemplateClone.SetActive(false);

                CreateSectionDataSO(clone.GetComponent<Tilemap>());
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

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Spawner", EditorStyles.boldLabel);

            GUI.enabled = maker.TemplateClone != null;
            if (GUILayout.Button("Player"))
            {
                GameObject spawner = Instantiate(maker.player.gameObject, GetSceneViewPivot(), Quaternion.identity, maker.TemplateClone.transform);
                Selection.activeObject = spawner;
            }
            if (GUILayout.Button("Enemy"))
            {
                GameObject spawner = Instantiate(maker.enemy.gameObject, GetSceneViewPivot(), Quaternion.identity, maker.TemplateClone.transform);
                Selection.activeObject = spawner;
            }
            if (GUILayout.Button("NPC"))
            {
                GameObject spawner = Instantiate(maker.npc.gameObject, GetSceneViewPivot(), Quaternion.identity, maker.TemplateClone.transform);
                Selection.activeObject = spawner;
            }
            if (GUILayout.Button("Cat Ball"))
            {
                GameObject spawner = Instantiate(maker.catball.gameObject, GetSceneViewPivot(), Quaternion.identity, maker.TemplateClone.transform);
                Selection.activeObject = spawner;
            }
            if (GUILayout.Button("Passage"))
            {
                GameObject spawner = Instantiate(maker.passage.gameObject, GetSceneViewPivot(), Quaternion.identity, maker.TemplateClone.transform);
                Selection.activeObject = spawner;
            }
            GUI.enabled = true;

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

        private void SwitchSectionTarget()
        {
            FindOutWorktable();
        }

        private void FindOutWorktable()
        {
            var worktables = maker.GetComponentsInChildren<ObjectHolder>(true);
            foreach (var worktable in worktables)
            {
                if (worktable.component as SectionSiteSO == SelectedSite)
                {
                    worktable.gameObject.SetActive(true);
                    continue;
                }
                worktable.gameObject.SetActive(false);
            }
        }

        private Vector3 GetSceneViewPivot()
        {
            if (SceneView.lastActiveSceneView != null)
            {
                return SceneView.lastActiveSceneView.pivot;
            }
            return Vector3.zero;
        }

        private void CreateSectionDataSO(Tilemap tilemap)
        {
            SectionDataSO asset = CreateInstance<SectionDataSO>();
            asset.tileMap = tilemap;
            asset.site = SelectedSite;

            var explorers = maker.GetComponents<RegionTerrainExplorer>();
            if (explorers != null && explorers.Length > 0)
            {
                foreach (var explorer in explorers)
                {
                    Debug.Log(explorer.data.name);
                }
                if (SelectedSite.upside != null)
                {
                    foreach (var explorer in explorers)
                    {
                        if (SelectedSite.upside == explorer.section)
                        {
                            asset.upsideData = explorer.data;
                        }
                    }
                }
                if (SelectedSite.downside != null)
                {
                    foreach (var explorer in explorers)
                    {
                        if (SelectedSite.downside == explorer.section)
                        {
                            asset.downsideData = explorer.data;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(SelectedSite.dataPath))
            {
                string fileName = "Section Data " + System.DateTime.Now.ToString("MMddHHmmss") + ".asset";
                AssetDatabase.CreateAsset(asset, SelectedSite.dataPath + "/" + fileName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
            }
        }

        private bool IsValidIndex() => selectedIndex >= 0 && selectedIndex < sites.Length;

        private SectionSiteSO SelectedSite => sites[selectedIndex];
    }
}

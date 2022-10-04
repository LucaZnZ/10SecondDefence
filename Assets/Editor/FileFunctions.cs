using System;
using System.IO;
using System.Linq;
using GameLogic;
using OneLiners;
using UnityEditor;
using UnityEngine;
using ZnZUtil;

namespace Editor
{
    public class FileFunctions : EditorWindow
    {
        // Story
        private const string story_OneDrivePath = "C:/Users/Luca/OneDrive/10SecondDefence/Story/";
        private static string story_TextPath => Application.dataPath + "/Entities/Story/";

        // Sprites
        private const string sprites_OneDrivePath = "C:/Users/Luca/OneDrive/10SecondDefence/Images/";
        private static string sprites_TextPath => Application.dataPath + "/Sprites/";

        // OneLine
        private const string oneLine_OneDrivePath = "C:/Users/Luca/OneDrive/10SecondDefence/OneLiners/";
        private static string oneLine_TextPath => Application.dataPath + "/Entities/OneLiners/TextFiles/";
        private const string oneLine_TextAssets = "Assets/Entities/OneLiners/TextFiles/";
        private const string oneLine_ScriptAssets = "Assets/Entities/OneLiners/";

        // Unit
        private const string unit_OneDrivePath = "C:/Users/Luca/OneDrive/10SecondDefence/UnitDescriptions/";
        private static string unit_TextPath => Application.dataPath + "/Entities/Units/Descriptions/";
        private const string unit_TextAssets = "Assets/Entities/Units/Descriptions/";
        private const string unit_UnitAssets = "Assets/Entities/Units/";

        // Terrain
        private const string terrain_OneDrivePath = "C:/Users/Luca/OneDrive/10SecondDefence/Terrain/";
        private static string terrain_TextPath => Application.dataPath + "/Entities/Terrains/Descriptions/";
        private const string terrain_TextAssets = "Assets/Entities/Terrains/Descriptions/";
        private const string terrain_TerrainAssets = "Assets/Entities/Terrains/";

        // Stats
        private const string stats_TextAsset = unit_UnitAssets + "Stats/UnitStats.csv";

        [MenuItem("Window/FileFunctions")]
        private static void ShowWindow()
        {
            var window = GetWindow<FileFunctions>();
            window.titleContent = new GUIContent("File Functions");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("Pull Files from OneDrive"))
            {
                CopyFiles(oneLine_OneDrivePath, oneLine_TextPath);
                CopyFiles(unit_OneDrivePath, unit_TextPath);
                CopyFiles(terrain_OneDrivePath, terrain_TextPath);
                CopyFiles(story_OneDrivePath, story_TextPath);
                CopyFiles(sprites_OneDrivePath, sprites_TextPath);
                AssetDatabase.Refresh();
            }

            if (GUILayout.Button("Push Files to OneDrive"))
            {
                CopyFiles(oneLine_TextPath, oneLine_OneDrivePath);
                CopyFiles(unit_TextPath, unit_OneDrivePath);
                CopyFiles(terrain_TextPath, terrain_OneDrivePath);
                CopyFiles(story_TextPath, story_OneDrivePath);
                CopyFiles(sprites_TextPath, sprites_OneDrivePath);
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Generate OneLine Textfiles"))
            {
                foreach (var olevent in Enum.GetNames(typeof(OneLinerEvent)))
                    File.CreateText($"{oneLine_TextPath}{olevent}.txt");

                AssetDatabase.Refresh();
            }

            GUILayout.Space(10);

            if (GUILayout.Button("Reimport OneLine Scripts"))
                ReimportObjects<OneLinerScript>(oneLine_TextAssets, oneLine_ScriptAssets, "Script");

            if (GUILayout.Button("Reimport unit descriptions"))
                ReimportObjects<Unit>(unit_TextAssets, unit_UnitAssets);

            if (GUILayout.Button("Import unit stats"))
            {
                var text = AssetDatabase.LoadAssetAtPath<TextAsset>(stats_TextAsset);
                if (text == null || text.text.Length == 0) return;
                var units = AssetUtil.LoadAssetsInFolder<Unit>(unit_UnitAssets);
                var lines = text.text.Split('\n');
                var header = lines[0].Split(';').ToList();
                for (var i = 1; i < lines.Length; i++)
                {
                    var fields = lines[i].Split(';');
                    var unit = units.Find(u => u.name == fields[header.IndexOf("Unit")]);
                    if (unit == null) continue;

                    unit.UpdateLevel(int.Parse(fields[header.IndexOf("Level")]),
                        new UnitLevel(
                            int.Parse(fields[header.IndexOf("Health")]),
                            int.Parse(fields[header.IndexOf("Damage")]),
                            FloatUtil.SmallParse(fields[header.IndexOf("Attack Speed")]),
                            int.Parse(fields[header.IndexOf("Attack Range")]),
                            int.Parse(fields[header.IndexOf("Speed")]),
                            int.Parse(fields[header.IndexOf("Cost")])
                        )
                    );
                }

                AssetDatabase.Refresh();
            }
        }

        private static void CopyFiles(string sourceDir, string targetDir, bool overwrite = true)
        {
            var files = Directory.GetFiles(sourceDir);
            foreach (var file in files.Where(f => Path.GetExtension(f) != ".meta"))
                File.Copy(file, targetDir + Path.GetFileName(file), overwrite);
            Debug.Log($"Copied {files.Length} files from {sourceDir} to {targetDir}");
        }

        private static void ReimportObjects<T>(string textAssetPath, string assetPath, string nameAppendix = "")
            where T : TextImportObject
        {
            int updated = 0, created = 0;
            var textFiles = AssetUtil.LoadAssetsInFolder<TextAsset>(textAssetPath);
            var existingUnits = AssetUtil.LoadAssetsInFolder<T>(assetPath);
            foreach (var text in textFiles)
            {
                var unit = existingUnits.Find(u => u.name == $"{text.name}{nameAppendix}");
                if (unit != null)
                {
                    updated++;
                    unit.ImportTextFile(text);
                }
                else
                {
                    created++;
                    CreateObject<T>(text, assetPath, nameAppendix);
                }
            }

            Debug.Log($"Created {created} and updated {updated} {typeof(T)}-Assets at path {assetPath}");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static T CreateObject<T>(TextAsset text, string path, string nameAppendix = "")
            where T : TextImportObject
        {
            var asset = CreateInstance<T>();
            asset.name = $"{text.name}{nameAppendix}";
            asset.ImportTextFile(text);
            AssetDatabase.CreateAsset(asset, $"{path}{asset.name}.asset");
            return asset;
        }
    }
}
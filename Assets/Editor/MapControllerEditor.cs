using GameLogic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MapController))]
    public class MapControllerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            DrawDefaultInspector();
            var mapController = (MapController) target;
            if (GUILayout.Button("Generate MapTerritories"))
            {
                mapController.GenerateTerritories();
            }
        }
    }
}
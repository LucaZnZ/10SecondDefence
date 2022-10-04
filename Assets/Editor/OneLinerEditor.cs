using OneLiners;
using UnityEditor;
using UnityEngine;
using ZnZUtil;

namespace Editor
{
    [CustomEditor(typeof(OneLinerDisplay))]
    public class OneLinerEditor : UnityEditor.Editor
    {
        private const string assetPath = "Assets/Entities/OneLiners/";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Import Assets"))
            {
                ((OneLinerDisplay) target).scriptList = AssetUtil.LoadAssetsInFolder<OneLinerScript>(assetPath);
            }
        }
    }
}
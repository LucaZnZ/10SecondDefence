using TGS;
using UnityEngine;
using ZnZUtil;

namespace GameLogic
{
    [CreateAssetMenu(menuName = "ZnZproductions/MapTerritory")]
    public class MapTerritory : TextImportObject
    {
        public new string name;
        public string description;
        public Sprite image;
        public float speedModifier;
        public int territoryIndex;
        public Color color;
        public Territory territory;

        public string fullDescription =>
            $"{description}\n" +
            $"mobility:\t{speedModifier}";

        public void SetTerritory(Territory territory, int index)
        {
            territoryIndex = index;
            this.territory = territory;
            color = territory.fillColor;
        }

        protected override void OnImport(TextAsset asset)
        {
            name = asset.name;
            description = asset.text;
        }
    }
}
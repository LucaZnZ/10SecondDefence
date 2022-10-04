using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZnZUtil;

namespace OneLiners
{
    [CreateAssetMenu(fileName = "Script", menuName = "ZnZproductions/OneLinerScript")]
    public class OneLinerScript : TextImportObject
    {
        public OneLinerEvent oneLinerEvent;
        public List<string> lines = new();

        private void UpdateLines(TextAsset asset)
        {
            if (asset == null) return;
            lines.Clear();
            lines.AddRange(asset.text.Split('\n').Where(l => l.Trim().Length > 0).ToList());
        }

        protected override void OnImport(TextAsset asset) => UpdateLines(asset);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TGS {
    public class Demo13 : MonoBehaviour {

        TerrainGridSystem tgs;

        void Start() {
            tgs = TerrainGridSystem.instance;
            tgs.OnTerritoryClick += Tgs_OnTerritoryClick;
        }

        private void Tgs_OnTerritoryClick(TerrainGridSystem sender, int territoryIndex, int buttonIndex) {
            Debug.Log("Hiding territory index " + territoryIndex);
            tgs.TerritorySetVisible(territoryIndex, false);
        }

    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TGS {
    public class Demo11 : MonoBehaviour {

        TerrainGridSystem tgs;
        GUIStyle labelStyle;
        List<int> cellIndices = new List<int>();

        void Start() {
            // setup GUI styles
            labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.normal.textColor = Color.black;

            // Get a reference to Terrain Grid System's API
            tgs = TerrainGridSystem.instance;

            ResetSurfaces();

            // Set colors for frontiers
            tgs.territoryDisputedFrontierColor = Color.yellow;
            tgs.TerritorySetFrontierColor(0, Color.red);
            tgs.TerritorySetFrontierColor(1, Color.blue);

            // Color for neutral territory
            tgs.TerritorySetNeutral(0, true);

            // listen to events
            tgs.OnCellClick += (grid, cellIndex, buttonIndex) => changeCellOwner(cellIndex, buttonIndex);
        }

        void ResetSurfaces() {
            tgs.ClearAll();
            tgs.TerritoryToggleRegionSurface(0, true, new Color(0.2f, 0.2f, 0.2f));
        }

        void OnGUI() {
            GUI.Label(new Rect(0, 5, Screen.width, 30), "Click on any cell with left or right buttons to set cell territory.", labelStyle);
            GUI.Label(new Rect(0, 20, Screen.width, 30), "(Territory in gray is marked as neutral)", labelStyle);
            GUI.Label(new Rect(0, 35, Screen.width, 30), "(Hold left shift and click to show frontier cells)", labelStyle);
            GUI.Label(new Rect(0, 50, Screen.width, 30), "(Hold left control and color random cells in region, X to clear all)", labelStyle);
        }

        void Update() {
            if (Input.GetKeyDown(KeyCode.X)) {
                ResetSurfaces();
            }
        }

        void changeCellOwner(int cellIndex, int buttonIndex) {
            // flash cells in frontier of territory
            if (Input.GetKey(KeyCode.LeftShift)) {
                // get cells on the frontier
                int territoryIndex = tgs.cells[cellIndex].territoryIndex;
                tgs.TerritoryGetFrontierCells(territoryIndex, ref cellIndices);
                tgs.CellFlash(cellIndices, Color.white, 2f);
                return;
            }

            // color cells in region
            if (Input.GetKey(KeyCode.LeftControl)) {
                int territoryIndex = tgs.CellGetTerritoryIndex(cellIndex);
                int territoryRegionIndex = tgs.CellGetTerritoryRegionIndex(cellIndex);
                List<Cell> cells = tgs.TerritoryGetCells(territoryIndex, territoryRegionIndex);
                foreach (Cell cell in cells) {
                    tgs.CellSetColor(cell, new Color(Random.value, Random.value, Random.value));
                }
                return;
            }

            // change cell owner
            tgs.CellSetTerritory(cellIndex, buttonIndex + 1);

        }


    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TGS {
	public class Demo3 : MonoBehaviour {

		TerrainGridSystem tgs;
		GUIStyle labelStyle;

		void Start () {
			// setup GUI styles
			labelStyle = new GUIStyle ();
			labelStyle.alignment = TextAnchor.MiddleCenter;
			labelStyle.normal.textColor = Color.black;

			tgs = TerrainGridSystem.instance;

			// hide all cells
			for (int k = 0; k < tgs.cells.Count; k++) {
				tgs.CellSetVisible (k, false);
			}
			tgs.Redraw(); // forces redraw immediately

            // listen to events
            tgs.OnCellClick += OnCellClick;
            tgs.OnCellDragStart += OnCellDragStart;
            tgs.OnCellDrag += OnCellDrag;
            tgs.OnCellDragEnd += OnCellDragEnd;

		}

		private void OnCellClick(TerrainGridSystem sender, int cellIndex, int buttonIndex) {
			// CellSetBorderVisible controls the visibility of the cell's border
			// CellSetVisible controls the visibility of both cell's border and surface
			tgs.CellSetVisible(cellIndex, !tgs.CellIsVisible(cellIndex));
		}

		private void OnCellDragEnd(TerrainGridSystem sender, int cellOriginIndex, int cellTargetIndex) {
			Debug.Log("Drags ends on " + cellTargetIndex);
        }

        private void OnCellDrag(TerrainGridSystem sender, int cellOriginIndex, int cellTargetIndex) {
			Debug.Log("Dragging cell " + cellOriginIndex + " over " + cellTargetIndex);
		}

		private void OnCellDragStart(TerrainGridSystem sender, int cellIndex) {
			Debug.Log("Drag starts on cell " + cellIndex);
		}


		void OnGUI () {
			GUI.Label (new Rect (0, 5, Screen.width, 30), "Click on any position to toggle cell visibility.", labelStyle);
		}

    }
}

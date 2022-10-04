using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TGS.Geom;

namespace TGS {

	public partial class Territory: AdminEntity {

		/// <summary>
        /// List of other territories sharing some border with this territory
        /// </summary>
		public List<Territory> neighbours = new List<Territory>();

		/// <summary>
        /// List of physical regions. Usually territories have only one region, but they can be split by assigning cells to other territories
        /// </summary>
		public List<Region> regions;

		public List<Cell> cells;
		public Color fillColor = Color.gray;
		public Color frontierColor = new Color(0, 0, 0, 0);

		public bool neutral { get; set; }

		public Territory() : this("") {
		}

		public Territory(string name) {
			this.name = name;
			visible = true;
			borderVisible = true;
			cells = new List<Cell>();
		}

	}

}
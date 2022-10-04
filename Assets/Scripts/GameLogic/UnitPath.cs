using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class UnitPath : Queue<UnitStep>
    {
        private readonly Transform origin;
        private readonly Vector3 goal;
        private readonly MapController mapController;

        public UnitPath(Transform origin, Vector3 goal, MapController mapController)
        {
            this.origin = origin;
            this.goal = goal;
            this.mapController = mapController;
        }

        public UnitPath UpdatePath()
        {
            Clear();
            if (mapController != null && origin != null)
                ConvertPath(mapController.GetPath(origin.position, goal));
            return this;
        }

        private UnitPath ConvertPath(List<int> cellIndizes)
        {
            foreach (var cell in cellIndizes)
            {
                Enqueue(new UnitStep(
                    mapController.GetCellPosition(cell),
                    mapController.GetCellSpeed(cell)));
            }

            return this;
        }
    }
}
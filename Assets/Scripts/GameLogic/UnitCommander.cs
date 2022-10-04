using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLogic
{
    public class UnitCommander : MonoBehaviour
    {
        [SerializeField] private MapController mapController;

        public void MoveUnitToOrigin(UnitBehaviour unit) => MoveUnit(unit, Vector3.zero);

        public void MoveUnit(UnitBehaviour unit, Vector3 goal)
        {
            Debug.Log($"Command to move unit {unit.name}");
            unit.Move(new UnitPath(unit.transform, goal, mapController).UpdatePath());
        }

        private List<UnitStep> ConvertPath(List<int> cellIndizes)
        {
            return cellIndizes.Select(
                cell => new UnitStep(
                    mapController.GetCellPosition(cell),
                    mapController.GetCellSpeed(cell))).ToList();
        }
    }
}
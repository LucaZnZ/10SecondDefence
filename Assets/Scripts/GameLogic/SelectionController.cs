using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    public class SelectionController : MonoBehaviour
    {
        private Unit selectedUnit;
        private Vector3 selectedLocation;
        private UnitBehaviour selectedMapUnit;

        public UnityEvent<Unit, Vector3> OnSpawnShopUnit;
        public UnityEvent<UnitBehaviour, Vector3> OnMoveMapUnit;
        public UnityEvent<MapTerritory> OnShowTerrain;

        public void SelectMapUnit(UnitBehaviour unit)
        {
            UnselectAll();
            selectedMapUnit = unit;
        }

        public void SelectShopUnit(Unit unit)
        {
            UnselectAll();
            selectedUnit = unit;
        }

        public void SelectMapLocation(Vector3 location, int cell)
        {
            selectedLocation = location;

            if (selectedUnit != null)
                OnSpawnShopUnit?.Invoke(selectedUnit, location);
            else if (selectedMapUnit != null && selectedMapUnit.isAlly &&
                     selectedMapUnit.unit.Level(selectedMapUnit.level).speed > 0)
                OnMoveMapUnit?.Invoke(selectedMapUnit, location);
            else
                OnShowTerrain?.Invoke(MapController.getInstance.getTerritoryForCell(cell));
        }

        public void UnselectAll()
        {
            selectedMapUnit = null;
            selectedUnit = null;
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using TGS;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using ZnZUtil;

namespace GameLogic
{
    public class MapController : SingletonBase<MapController>
    {
        [SerializeField] private TerrainGridSystem tgs;
        [SerializeField] private List<MapTerritory> territories = new();
        [SerializeField] private List<MapTerritory> enemyTerritories = new();
        private readonly List<int> enemyCells = new();
        private int originIndex;

        public UnityEvent<Vector3, int> onCellClicked = new();

        public void RaiseClickEvent()
        {
            var cellID = tgs.cellHighlightedIndex;
            if (cellID < 0) return;
            onCellClicked?.Invoke(tgs.CellGetPosition(cellID), cellID);
        }

        public MapTerritory getTerritoryForCell(int index) =>
            territories.Find(m => m.territoryIndex == tgs.CellGetTerritoryIndex(index));

        // public MapTerritory getTerritoryAtPosition(Vector3 position)
        // {
        //     return territories.Find(m => m.territory == tgs.TerritoryGetAtPosition(position, true));
        // }

        public Vector3 GetClickedCellPosition() => tgs.CellGetPosition(tgs.cellLastClickedIndex);

        public void ActivateCellSelection() => tgs.highlightMode = HIGHLIGHT_MODE.Cells;
        public void ActivateTerritorySelection() => tgs.highlightMode = HIGHLIGHT_MODE.Territories;

        public void DeactivateSelection() => tgs.highlightMode = HIGHLIGHT_MODE.None;

        public void GenerateTerritories()
        {
            return;
            // foreach (var territory in tgs.territories)
            // {
            //     var obj = ScriptableObject.CreateInstance<MapTerritory>();
            //     obj.SetTerritory(territory, tgs.TerritoryGetIndex(territory));
            //     AssetDatabase.CreateAsset(obj, $"Assets/Entities/Terrains/{territory.name}.asset");
            //     territories.Add(obj);
            // }
            //
            // AssetDatabase.SaveAssets();
        }

        private void GenerateCellCosts()
        {
            foreach (var territory in territories)
            {
                foreach (var cell in tgs.TerritoryGetCells(territory.territoryIndex).Select(c => c.index))
                {
                    if (territory.speedModifier <= 0)
                        tgs.CellSetCanCross(cell, false);
                    else if (territory.speedModifier != 1f)
                        tgs.CellSetCrossCost(cell, 1 / territory.speedModifier);
                }
            }
        }

        private void GetAllEnemyCells()
        {
            foreach (var cell in enemyTerritories.SelectMany(t => tgs.TerritoryGetCells(t.territoryIndex)))
                enemyCells.Add(cell.index);
        }

        public List<int> GetPath(Vector3 start, Vector3 goal)
        {
            var startCell = tgs.CellGetAtPosition(start, true).index;
            var goalCell = tgs.CellGetAtPosition(goal, true).index;
            var path = tgs.FindPath(startCell, goalCell, out var cost);
            // Debug.Log($"Found path from {start} to {goal} over {path.Count} cells and a cost of {cost}");
            return path;
        }

        private void Start()
        {
            onCellClicked.AddListener((v, cell) => Debug.Log($"Cell {cell} was clicked"));
            GenerateCellCosts();
            GetAllEnemyCells();
            originIndex = tgs.CellGetAtPosition(Vector3.zero).index;
        }

        public Vector3 GetCellPosition(int cell)
        {
            return tgs.CellGetPosition(cell);
        }

        public float GetCellSpeed(int cell)
        {
            return territories.Find(t => t.territoryIndex == tgs.CellGetTerritoryIndex(cell)).speedModifier;
        }

        public List<Vector3> GetEnemySpawnPositions()
        {
            var cells = tgs.CellGetNeighboursWithinRange(enemyCells.GetRandomSItem(), 1, 4);
            var pos = cells.Where(c => enemyCells.Contains(c)).Select(c => tgs.CellGetPosition(c)).ToList();
            Debug.Log($"Found {pos.Count} positions for enemies to spawn");
            return pos;
        }
    }
}
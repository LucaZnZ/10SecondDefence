using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    public class SpawnController : SingletonBase<SpawnController>
    {
        [SerializeField] private MapController mapController;
        [SerializeField] private GameObject unitPrefab;
        [SerializeField] private Transform entityParent;
        [Space] [SerializeField] private Unit castle;

        public UnityEvent<UnitBehaviour> onSpawnAlly, onSpawnEnemy;

        public void SpawnFromShop(Unit unit, Vector3 position)
        {
            if (!TimeAccount.CanAfford(unit.cost)) return;
            SpawnAlly(unit, position);
            TimeAccount.UpdateTime(t => t - unit.cost);
        }

        public UnitBehaviour SpawnAlly(Unit unit, Vector3 position)
        {
            var spawn = SpawnUnit(unit, position, true);
            onSpawnAlly?.Invoke(spawn);
            return spawn;
        }

        public UnitBehaviour SpawnEnemy(Unit unit, Vector3 position)
        {
            var spawn = SpawnUnit(unit, position, false);
            onSpawnEnemy?.Invoke(spawn);
            return spawn;
        }

        private UnitBehaviour SpawnUnit(Unit unit, Vector3 position, bool ally)
        {
            var spawn = Instantiate(unitPrefab, position, transform.rotation, entityParent);
            var behav = spawn.GetComponent<UnitBehaviour>();
            behav.Initialize(unit, ally);
            return behav;
        }

        private void Start()
        {
            SpawnUnit(castle, Vector3.zero, true);
        }
    }
}
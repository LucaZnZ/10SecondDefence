using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZnZUtil;

namespace GameLogic
{
    public class WaveController : MonoBehaviour
    {
        [SerializeField] private MapController mapController;
        [SerializeField] private SpawnController spawnController;
        [SerializeField] private List<Unit> enemyUnits = new(), minibosses = new();
        [SerializeField] private float waveSpawnMultiplier;

        private int time = 10, waveCounter;
        private Countdown countdown;

        public UnityEvent<int> onNextWaveCount;
        public UnityEvent<float> onNextWaveCountdown;
        public void SetWaveTime(int time) => this.time = time;

        private void InitiateNextWave()
        {
            IncreaseWaveCounter();
            if (time <= 0) return;
            countdown = new Countdown(time + 0.1f);
            StartCoroutine(countdown.PerformAfterRun(NextWave));
        }

        private void NextWave()
        {
            // Get Random Spawn position
            var pos = mapController.GetEnemySpawnPositions();

            // Get Random Wave of Troops
            // calculate current total damage of player at 1x mod -> pool for total enemy hitpoints
            var costPool = Mathf.Max(1, waveCounter * waveSpawnMultiplier);

            // TODO spawning of higher level units?
            List<Unit> units = new(), unitPool = new();
            unitPool.AddRange(enemyUnits);
            unitPool.AddRange(minibosses);

            while (costPool > 0)
            {
                unitPool.RemoveAll(e => e.Level(1).cost > costPool);
                var unit = unitPool.GetRandomItem();
                if (unit == null) break;
                costPool -= unit.Level(1).damage;
                minibosses.Remove(unit);
                units.Add(unit);
            }

            // Spawn troops at position
            var wave = new Wave(1);
            foreach (var unit in units)
            {
                var p = pos.GetRandomSItem();
                pos.Remove(p);
                var u = spawnController.SpawnEnemy(unit, p);
                wave.AddUnit(u);
                // moving to origin happens through spawn event
            }

            // Initiate countdown to next wave
            InitiateNextWave();
        }

        private void IncreaseWaveCounter()
        {
            waveCounter++;
            onNextWaveCount.Invoke(waveCounter);
            Debug.Log("Wave " + waveCounter);
        }

        private void Start()
        {
            InitiateNextWave();
        }

        private void Update()
        {
            if (countdown != null)
                onNextWaveCountdown.Invoke(countdown.GetTime());
        }
    }
}
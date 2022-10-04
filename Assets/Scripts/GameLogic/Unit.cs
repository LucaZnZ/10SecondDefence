using System;
using System.Collections.Generic;
using System.Linq;
using OneLiners;
using UnityEngine;
using ZnZUtil;

namespace GameLogic
{
    [CreateAssetMenu(menuName = "ZnZproductions/Unit")]
    public class Unit : TextImportObject
    {
        public string description;
        public Sprite allyImage, enemyImage;
        public int imageScale = 1;
        public bool sellable = true;
        [Header("Sounds")] public string attackSound;
        public string deathSound, upgradeSound;
        public int cost => levels.First()?.cost ?? 0;
        public int maxLevel => levels.Count;

        public OneLinerEvent onSpawn, onKill, onDeath, onUpgrade, onSell;
        [SerializeField] private List<UnitLevel> levels = new();

        public UnitLevel Level(int level)
        {
            level--;
            if (level < 0 || level > levels.Count - 1) return levels.First();
            return levels[level];
        }

        public string shopDescription =>
            $"{description}\n" +
            $"health:\t{levels.First()?.health ?? 0}\n" +
            $"damage:\t{levels.First()?.damage ?? 0}\n" +
            $"aspeed:\t{levels.First()?.attackSpeed ?? 0f}\n" +
            $"arange:\t{levels.First()?.attackRange ?? 0}\n" +
            $"speed: \t{levels.First()?.speed ?? 0}\n" +
            $"costs: \t{cost}";

        private void OnEnable()
        {
            if (levels.Count == 0)
                levels.Add(new UnitLevel());
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            AudioManager.ValidateTrack(attackSound, this);
            AudioManager.ValidateTrack(deathSound, this);
            AudioManager.ValidateTrack(upgradeSound, this);
        }

        protected override void OnImport(TextAsset asset)
        {
            name = asset.name;
            description = asset.text;
        }

        public void UpdateLevel(int level, UnitLevel stats)
        {
            while (levels.Count < level)
                levels.Add(new UnitLevel());

            levels[level - 1] = stats;
        }
    }

    [Serializable]
    public class UnitLevel
    {
        public int health, damage;
        public float attackSpeed;
        public int attackRange, speed, cost;

        public UnitLevel()
        {
        }

        public UnitLevel(int health, int damage, float attackSpeed, int attackRange, int speed, int cost)
        {
            this.health = health;
            this.damage = damage;
            this.attackSpeed = attackSpeed;
            this.attackRange = attackRange;
            this.speed = speed;
            this.cost = cost;
        }
    }
}
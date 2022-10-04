using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OneLiners;
using UnityEngine;
using ZnZUtil;

namespace GameLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class UnitBehaviour : MonoBehaviour
    {
        public static float rangeMultiplier = 5;
        [SerializeField] private new Rigidbody rigidbody;
        public Unit unit;
        public bool isAlly { get; private set; }

        private int health;

        private int damage =>
            isAlly ? (unit.Level(level).damage * TimeAccount.GetModifier()).RoundToInt() : unit.Level(level).damage;

        private float speed => path is {Count: > 0} ? path.Peek().speed * unit.Level(level).speed : 0;
        private float attackSpeed => unit.Level(level).attackSpeed;
        private float attackRange => unit.Level(level).attackRange;
        public int upgradeCosts => unit.Level(level + 1).cost;
        public int level { get; private set; } = 1;

        public int sellCosts => CalculateSellCosts();

        public event Action<UnitBehaviour> OnDeath;

        private UnitPath path;
        private bool allowWalking;

        public string upgradeDescription => level < unit.maxLevel
            ? $"{unit.description}\n" +
              $"health:\t{health} \\ {unit.Level(level).health} \t-> {unit.Level(level + 1).health}\n" +
              $"damage:\t{damage} \\ {unit.Level(level).damage} \t-> {unit.Level(level + 1).damage}\n" +
              $"aspeed:\t{attackSpeed} \t-> {unit.Level(level).attackSpeed}\n" +
              $"arange: \t{attackRange} \t-> {unit.Level(level + 1).attackRange}\n" +
              $"speed: \t{speed} \\ {unit.Level(level).speed} \t-> {unit.Level(level + 1).speed}\n" +
              $"level:\t \t{level} \\ {unit.maxLevel} \t-> {level + 1}"
            : $"{unit.description}\n" +
              $"health:\t{health} \\ {unit.Level(level).health}\n" +
              $"damage:\t{damage} \\ {unit.Level(level).damage}\n" +
              $"aspeed:\t{attackSpeed}\n" +
              $"arange: \t{attackRange}\n" +
              $"speed: \t{speed} \\ {unit.Level(level).speed}\n" +
              $"level:\t \t{level} \\ {unit.maxLevel}";

        public void Initialize(Unit unit, bool ally = true)
        {
            this.unit = unit;
            name = unit.name;
            isAlly = ally;
            health = unit.Level(1).health;

            var img = GetComponent<SpriteRenderer>();
            img.sprite = ally ? this.unit.allyImage : this.unit.enemyImage;
            img.transform.localScale = new Vector3(this.unit.imageScale, unit.imageScale, unit.imageScale);

            OneLinerEventHandler.RaiseEvent(unit.onSpawn, name);
        }

        public void Upgrade()
        {
            if (level >= unit.maxLevel || !TimeAccount.CanAfford(upgradeCosts)) return;
            health += unit.Level(level + 1).health - unit.Level(level).health;
            level++;
            TimeAccount.UpdateTime(time => time - upgradeCosts);
            OneLinerEventHandler.RaiseEvent(unit.onUpgrade, name);
            AudioManager.PlayAudioTrack(unit.upgradeSound);
        }

        public void Sell()
        {
            TimeAccount.UpdateTime(time => time + sellCosts);
            OneLinerEventHandler.RaiseEvent(unit.onSell, name);
            Destroy(gameObject);
        }

        private int CalculateSellCosts()
        {
            if (!unit.sellable) return -1;
            float costs = 0;
            for (int i = 1; i <= level; i++)
                costs += unit.Level(i).cost;
            return (int) Math.Round(costs / 2);
        }

        public void Move(UnitPath path)
        {
            // this.path.Clear();
            // path.ForEach(step => this.path.Enqueue(step));
            this.path = path;
        }

        private void Walk()
        {
            if (!allowWalking || path == null || path.Count == 0) return;
            if (path.Count > 1)
                path.UpdatePath();
            var step = path.Peek();
            var tSpeed = speed * Time.fixedDeltaTime;

            if (Vector3.Distance(transform.position, step.target) < tSpeed)
                path.Dequeue();

            // transform.Translate((step.target - transform.position).normalized * tSpeed);
            rigidbody.MovePosition(rigidbody.position + (step.target - transform.position).normalized * tSpeed);
        }

        private bool Attack()
        {
            var enemy = FindEnemiesInRange().GetRandomItem();
            if (enemy == null) return false;
            Debug.Log($"Enemy found " + enemy.name);
            if (enemy.Hurt(damage))
                OneLinerEventHandler.RaiseEvent(unit.onKill, name);
            AudioManager.PlayAudioTrack(unit.attackSound);
            return true;
        }

        private List<UnitBehaviour> FindEnemiesInRange()
        {
            var results = new Collider[10];
            var size = Physics.OverlapSphereNonAlloc(
                transform.position, attackRange * rangeMultiplier, results, LayerMask.GetMask("Units"));
            List<UnitBehaviour> list = new();
            if (size <= 0) return list;

            list.AddRange(results.Where(r => r != null)
                .Select(result => result.gameObject.GetComponent<UnitBehaviour>())
                .Where(enemy => enemy != null && enemy.isAlly != isAlly));

            return list;
        }


        /// <returns>True if enemy was killed</returns>
        private bool Hurt(int damage)
        {
            health -= damage;
            if (health >= 0) return false;
            Kill();
            return true;
        }

        private void Kill()
        {
            OnDeath?.Invoke(this);
            OneLinerEventHandler.RaiseEvent(unit.onDeath, name);
            AudioManager.PlayAudioTrack(unit.deathSound);
            Destroy(gameObject);
        }

        private void Start()
        {
            StartCoroutine(AttackRoutine());
        }

        private void FixedUpdate()
        {
            Walk();
        }

        private IEnumerator AttackRoutine()
        {
            while (attackSpeed == 0)
                yield return new WaitForEndOfFrame();

            var wait = new WaitForSeconds(1 / attackSpeed);
            while (true)
            {
                allowWalking = true;
                if (!Attack())
                    yield return new WaitForEndOfFrame();
                else
                {
                    allowWalking = false;
                    yield return wait;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, attackRange * rangeMultiplier);

            if (path == null) return;
            var lastPos = transform.position;

            foreach (var step in path)
            {
                Gizmos.DrawLine(lastPos, step.target);
                lastPos = step.target;
            }
        }
    }
}
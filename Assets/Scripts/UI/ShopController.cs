using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    public class ShopController : SingletonBase<ShopController>
    {
        [SerializeField] private SpawnController spawnController;
        [SerializeField] private GameObject unitbuttonPrefab;
        [SerializeField] private Transform shopHolder;
        [SerializeField] private Color affordableCol, expensiveCol;
        [SerializeField] private List<Unit> unitsInShop = new(), heroesInShop = new();

        public UnityEvent<Unit> onUnitSelected = new();

        private readonly HashSet<UnitButton> buttons = new();

        private void Start()
        {
            // Spawn unit buttons
            unitsInShop.ForEach(AddUnitToShop);
            heroesInShop.ForEach(AddUnitToShop);
        }

        private void AddUnitToShop(Unit unit)
        {
            var obj = Instantiate(unitbuttonPrefab, shopHolder).GetComponent<UnitButton>();
            obj.Initialize(unit, this);
            buttons.Add(obj);
        }

        public void RemoveHeroFromShop(Unit unit) => heroesInShop.Remove(unit);

        public void SelectUnitItem(Unit unit)
        {
            onUnitSelected?.Invoke(unit);
        }

        public void UpdatePriceLimit(int price)
        {
            foreach (var unitButton in buttons)
            {
                unitButton.ColorizeBackground(unitButton.unit.cost <= price ? affordableCol : expensiveCol);
            }
        }
    }
}
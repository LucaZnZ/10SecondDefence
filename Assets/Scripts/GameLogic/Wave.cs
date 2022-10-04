using System;
using System.Collections.Generic;
using OneLiners;

namespace GameLogic
{
    public class Wave
    {
        private readonly HashSet<UnitBehaviour> units = new();
        private int price;

        public event Action OnDefeat;

        public Wave(int price)
        {
            this.price = price;
            OnDefeat += () =>
            {
                TimeAccount.UpdateTime(time => time += price);
                OneLinerEventHandler.RaiseEvent(OneLinerEvent.EnemyWaveDefeated, "Knight Commander");
            };
        }

        public void AddUnit(UnitBehaviour unit)
        {
            units.Add(unit);
            unit.OnDeath += RemoveUnit;
        }

        private void RemoveUnit(UnitBehaviour unit)
        {
            units.Remove(unit);
            if (units.Count == 0)
                OnDefeat?.Invoke();
        }
    }
}
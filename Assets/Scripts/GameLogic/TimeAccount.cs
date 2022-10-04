using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace GameLogic
{
    public class TimeAccount : SingletonBase<TimeAccount>
    {
        [Serializable]
        private class OffsetToModifier
        {
            public int offset;
            public float modifier;
        }

        private int currentTime;
        private float minimumModifier = 0.1f, currentModifier;

        [SerializeField] private int centerTime = 10;

        [SerializeField] private List<OffsetToModifier> modifierMap = new();

        public UnityEvent<int> onTimeChange = new();
        public UnityEvent<float> onModifierChange = new();

        private void Start()
        {
            onTimeChange.AddListener(UpdateModifier);
            minimumModifier = modifierMap.Min(m => m.modifier);
            UpdateTime(t => centerTime);
        }

        private void UpdateModifier(int time)
        {
            currentModifier = GetModifierFromTimeoffset(math.abs(time - centerTime));
            onModifierChange.Invoke(currentModifier);
        }

        private float GetModifierFromTimeoffset(int timeoffset)
        {
            return modifierMap.Find(m => m.offset.Equals(timeoffset))?.modifier ?? minimumModifier;
        }

        private void UpdateCurrentTime(Func<int, int> update)
        {
            currentTime = Math.Max(0, update(currentTime));
            onTimeChange.Invoke(currentTime);
        }

        public static void UpdateTime(Func<int, int> update) => instance.UpdateCurrentTime(update);
        public static int GetTime() => instance.currentTime;
        public static float GetModifier() => instance.currentModifier;
        public static bool CanAfford(int time) => instance.currentTime >= time;
    }
}
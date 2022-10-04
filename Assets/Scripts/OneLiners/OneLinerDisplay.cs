using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZnZUtil;
using Random = UnityEngine.Random;

namespace OneLiners
{
    public class OneLinerDisplay : MonoBehaviour
    {
        private class EventMessage
        {
            public readonly int cost;
            public readonly OneLinerEvent oneLinerEvent;
            public readonly string name;

            public EventMessage(OneLinerEvent oneLinerEvent, string name)
            {
                this.oneLinerEvent = oneLinerEvent;
                this.name = name;
                cost = (int) oneLinerEvent;
            }
        }

        private static readonly Dictionary<OneLinerEvent, OneLinerScript> scripts = new();
        public List<OneLinerScript> scriptList = new();

        [SerializeField] private TMP_Text text;
        [SerializeField] private RectTransform layoutRoot;

        [SerializeField] private float timeOffsetMin, timeOffsetMax;

        private readonly List<EventMessage> messages = new();

        public void AddEvent(OneLinerEvent oneLinerEvent, string name)
        {
            messages.Add(new EventMessage(oneLinerEvent, name));
        }

        private void DisplayMessage(EventMessage message)
        {
            if (!scripts.ContainsKey(message.oneLinerEvent)) return;
            var line = scripts[message.oneLinerEvent].lines.GetRandomItem();
            if (string.IsNullOrEmpty(line)) return;
            scripts[message.oneLinerEvent].lines.Remove(line);
            DisplayText(message.name, line);
        }

        private void DisplayText(string name, string text)
        {
            this.text.text += $"{name}: {text}\n";
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
        }

        private IEnumerator<WaitForSeconds> MessageRoutine()
        {
            while (true)
            {
                Debug.Log(messages.Count);
                if (messages.Count > 0)
                {
                    DisplayMessage(messages.Count == 1
                        ? messages.First()
                        : messages
                            .OrderBy(m => m.cost).ToList()
                            .GetRange(0, Math.Min(5, messages.Count - 1))
                            .GetRandomItem());
                    messages.Clear();
                }

                yield return new WaitForSeconds(Random.Range(timeOffsetMin, timeOffsetMax));
            }
        }

        private void Start()
        {
            StartCoroutine(MessageRoutine());
            Debug.Log(scripts.Keys.Count);
            foreach (var script in scriptList) scripts.TryAdd(script.oneLinerEvent, script);
        }
    }
}
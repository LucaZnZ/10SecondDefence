using UnityEngine;
using UnityEngine.Events;

namespace OneLiners
{
    public class OneLinerEventHandler : SingletonBase<OneLinerEventHandler>
    {
        public UnityEvent<OneLinerEvent, string> onEvent = new();

        public static void RaiseEvent(OneLinerEvent oneLinerEvent, string name) =>
            instance.onEvent?.Invoke(oneLinerEvent, name);

        private void Start()
        {
            onEvent.AddListener((e, n) => Debug.Log($"Event: \"{e}\" was raised by {n}"));
        }
    }
}
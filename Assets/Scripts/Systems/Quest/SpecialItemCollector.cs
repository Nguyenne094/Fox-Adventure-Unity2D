using Nguyen.Event;
using UnityEngine;

namespace Quest
{
    public class SpecialItemCollector : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO specialItemCollectedEvent;
        public int SpecialItemCount { get; private set; }

        void OnEnable()
        {
            specialItemCollectedEvent.OnEventRaised += OnSpecialItemCollected;
        }

        void OnDisable()
        {
            specialItemCollectedEvent.OnEventRaised -= OnSpecialItemCollected;
        }

        private void OnSpecialItemCollected()
        {
            SpecialItemCount++;
        }

        public void ResetSpecialItemCount()
        {
            SpecialItemCount = 0;
        }
    }
}
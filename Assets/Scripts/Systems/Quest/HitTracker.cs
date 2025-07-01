using Nguyen.Event;
using UnityEngine;

namespace Quest
    {
    public class HitTracker : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private VoidEventChannelSO playerGetHitEvent;

        public int HitCount { get; private set; }

        private void Awake()
        {
            if (playerController == null)
            {
                playerController = PlayerController.Instance;
            }
            if (playerGetHitEvent == null)
            {
                Debug.LogError("Player Get Hit Event is not assigned in HitTracker.");
            }
        }

        private void OnEnable()
        {
            playerGetHitEvent.OnEventRaised += InscreaseHitCount;
        }

        private void OnDisable()
        {
            playerGetHitEvent.OnEventRaised -= InscreaseHitCount;
        }

        public void InscreaseHitCount()
        {
            HitCount++;
        }

        public void DecreaseHitCount()
        {
            if (HitCount > 0)
            {
                HitCount--;
            }
        }

        public void ResetHitCount()
        {
            HitCount = 0;
        }
    }
}
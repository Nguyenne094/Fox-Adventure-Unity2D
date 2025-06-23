using System;
using System.Collections;
using Nguyen.Event;
using UnityEngine;
using UnityEngine.Events;

namespace Bap.EventChannel
{
    public class VoidEventListener : MonoBehaviour
    {
        public VoidEventChannelSO EventChannel;
        public UnityEvent Response;
        
        [SerializeField] private float _Delay;

        private void OnEnable()
        {
            if (EventChannel != null)
                EventChannel.OnEventRaised += OnEventRaised;
        }

        private void OnDisable()
        {
            if (EventChannel != null)
                EventChannel.OnEventRaised -= OnEventRaised;
        }

        // Raises an event after a delay
        public void OnEventRaised()
        {
            StartCoroutine(RaiseEventDelayed(_Delay));
        }

        private IEnumerator RaiseEventDelayed(float delay)
        {
            yield return new WaitForSeconds(delay);
            Response.Invoke();
        }
    }
}
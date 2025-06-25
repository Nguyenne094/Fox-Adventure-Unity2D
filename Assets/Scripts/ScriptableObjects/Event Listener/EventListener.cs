using Nguyen.Event;
using UnityEngine.Events;
using UnityEngine;

namespace Bap.EventChannel
{
    public abstract class EventListener<T> : MonoBehaviour
    {
        public GenericEventChannelSO<T> EventChannel;
        public UnityEvent<T> Response;

        public void Invoke(T value)
        {
            Response?.Invoke(value);
        }
    }
}
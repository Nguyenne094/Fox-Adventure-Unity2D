using UnityEngine;
using Nguyen.Event;

namespace Quest
{

    [RequireComponent(typeof(Collider2D))]
    public class TriggerEnterEvent : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onTriggerColliderEvent;
        [SerializeField] private LayerMask layerMask;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((layerMask & (1 << collision.gameObject.layer)) != 0)
            {
                onTriggerColliderEvent.RaiseEvent();
            }
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class OnGameObjectActive : MonoBehaviour
    {
        [SerializeField] private UnityEvent onGameObjectIsActived;

        private void OnEnable()
        {
            onGameObjectIsActived?.Invoke();
        }
    }
}
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public bool Persist;
        
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (!_instance)
                {
                    SetupInstance();
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            RemoveDuplicate();
        }

        private static void SetupInstance()
        {
            _instance = FindFirstObjectByType<T>();

            if (_instance == null)
            {
                var obj = new GameObject(typeof(T).Name);
                _instance = obj.AddComponent<T>();
            }
        }

        private void RemoveDuplicate()
        {
            if (_instance == null)
            {
                _instance = this as T;

                if (Persist)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
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
                if (_instance == null)
                {
                    SetupInstance();
                    Debug.Log(_instance);
                }
                return _instance;
            }
        }

        public virtual void Awake()
        {
            RemoveDuplucate();
            Debug.Log(_instance);
        }

        private static void SetupInstance()
        {
            _instance = Object.FindFirstObjectByType<T>();

            if (_instance == null)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = typeof(T).Name;
                _instance = gameObj.AddComponent<T>();
            }
        }

        private void RemoveDuplucate()
        {
            if (_instance != null)
            {
                // Destroy(gameObject);
            }
            else
            {
                _instance = this as T;
            }

            if (Persist)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
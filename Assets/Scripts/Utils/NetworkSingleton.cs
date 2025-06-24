using Unity.Netcode;
using UnityEngine;

namespace Utils
{
    public class NetworkSingleton<T> : NetworkBehaviour where T : Component
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
            _instance = Object.FindFirstObjectByType<T>();

            if (!_instance)
            {
                GameObject gameObj = new GameObject();
                gameObj.name = typeof(T).Name;
                _instance = gameObj.AddComponent<T>();
            }
        }

        private void RemoveDuplicate()
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
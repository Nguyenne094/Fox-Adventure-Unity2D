using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nguyen.Event;
using UnityEditor;
using UnityEngine;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Bap.DependencyInjection {
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : PropertyAttribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProvideAttribute : PropertyAttribute { }
    
    public class Injector : MonoBehaviour {
        [SerializeField] private VoidEventChannelSO _onSceneGroupLoaded;
        [SerializeField] private bool _canOverride;
        
        private static Dictionary<Type, object> _dependencyContainer = new();
        private static Dictionary<Scene, Injector> _sceneContainer = new ();
        private static string k_globalInjectorName = "Injector [Global]";
        private static string k_sceneInjectorName = "Injector [Scene]";

        private const BindingFlags k_BindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        public bool ValidateOnSceneLoaded { get; set; } = true;
        
        private static Injector global;
        
        public void ConfigAsGlobal()
        {
            if (global == this) {
                Debug.LogWarning("[DI] ConfigAsGlobal: Already configured as global", this);
            } else if (global != null) {
                Debug.LogError("[DI] ConfigAsGlobal: Another Injector is already configured as global", this);
            } else {
                Debug.Log("[DI] ConfigAsGlobal: Configured as global", this);
                global = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public void ConfigForScene()
        {
            Scene scene = gameObject.scene;

            if (_sceneContainer.ContainsKey(scene)) {
                Debug.Log($"[DI] ConfigForScene: Already config for {scene.name}. Destroy this Injector");
                Destroy(this.gameObject);
                return;
            }
            
            _sceneContainer.Add(scene, this);
        }
        
        private void Start()
        {
            StartInjection();
        }

        public void Awake()
        {
            if (_onSceneGroupLoaded != null)
            {
                _onSceneGroupLoaded.OnEventRaised += StartInjection;
            }
        }

        private void OnDestroy()
        {
            if (_onSceneGroupLoaded != null)
            {
                _onSceneGroupLoaded.OnEventRaised -= StartInjection;
            }
        }

        #region Injection Processing

        private void StartInjection(){
            var monoBehaviours = GetAllMonoBehaviours();
            
            foreach (var behaviour in monoBehaviours)
            {
                RegisterProvidedDependencies(behaviour);
            }
            if(ValidateOnSceneLoaded) Validate();

            foreach (var behaviour in monoBehaviours)
            {
                InjectDependencies(behaviour);
            }
        }
        
        private void InjectDependencies(MonoBehaviour behaviour)
        {
            InjectFields(behaviour);
            InjectMethods(behaviour);
        }

        public void RegisterProvidedDependencies(MonoBehaviour behaviour)
        {
            var methods = behaviour.GetType()
                .GetMethods(k_BindingFlags)
                .Where(m => Attribute.IsDefined(m, typeof(ProvideAttribute)));

            foreach (var method in methods)
            {
                if(method.GetParameters().Length == 0)
                    RegisterDependency(method, behaviour);
            }
        }

        private void RegisterDependency(MethodInfo method, MonoBehaviour behaviour)
        {
            var returnType = method.ReturnType;
            if (returnType == typeof(void)) return;

            var instance = method.Invoke(behaviour, null);
            if(instance == null)
                Debug.LogError($"[DI] {behaviour.name} cannot provide {returnType.Name}. Because instanse is null");
            
            if (!_dependencyContainer.ContainsKey(returnType) || _canOverride)
            {
                _dependencyContainer[returnType] = instance;
            }
        }

        private void InjectFields(MonoBehaviour behaviour)
        {
            var fields = behaviour.GetType()
                .GetFields(k_BindingFlags)
                .Where(f => Attribute.IsDefined(f, typeof(InjectAttribute)));

            foreach (var field in fields)
            {
                if (field.GetValue(behaviour) != null) continue;

                var resolvedInstance = ResolveDependency(field.FieldType);
                if (resolvedInstance != null)
                {
                    field.SetValue(behaviour, resolvedInstance);
                }
            }
        }

        public void InjectMethods(MonoBehaviour behaviour)
        {
            var methods = behaviour.GetType()
                .GetMethods(k_BindingFlags)
                .Where(m => Attribute.IsDefined(m, typeof(InjectAttribute)));

            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (method.ReturnType != typeof(void)) continue;
                if (parameters.Length == 0)
                {
                    Debug.LogWarning($"{method.Name} is missing a parameter for {behaviour.name}");
                }

                var args = parameters.Select(p => ResolveDependency(p.ParameterType)).ToArray();
                if (args.All(arg => arg != null))
                {
                    method.Invoke(behaviour, args);
                }
            }
        }

        public object ResolveDependency(Type type)
        {
            try
            {
                if (_dependencyContainer.TryGetValue(type, out var instance))
                {
                    return instance;
                }
                else if(global != this && global != null)
                {
                    return global.ResolveDependency(type);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[DI] Fail to Resolve {type.Name}: {e.Message}", this);
                throw;
            }

            return null;
        }

        /// <summary>
        /// Find all GameObject in all active scenes
        /// </summary>
        /// <returns></returns>
        private MonoBehaviour[] GetAllMonoBehaviours()
        {
            if (global == this)
            {
                return FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID);
            }
            return  GetAllMonoBehavioursInScene();
        }

        private MonoBehaviour[] GetAllMonoBehavioursInScene()
        {
            List<MonoBehaviour> monoBehaviours = new();
            
            foreach (var go in gameObject.scene.GetRootGameObjects())
            {
                monoBehaviours.AddRange(go.GetComponentsInChildren<MonoBehaviour>());
            }

            return monoBehaviours.ToArray();
        }

        #endregion

        [ContextMenu("Log Dependencies")]
        public void LogDependencies()
        {
            Debug.Log("Logging dependencies:");
            foreach (var dependency in _dependencyContainer)
            {
                Debug.Log($"[DI] {dependency.Key.Name} : {dependency.Value}");
            }
        }
        
        /// <summary>
        /// Check for missing dependencies, throw exception
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void Validate()
        {
            foreach (var dependency in _dependencyContainer)
            {
                if (dependency.Value == null || dependency.Value.GetType() != dependency.Key)
                {
                    if (dependency.Value != null)
                        throw new Exception(
                            $"[DI] Exception: Invalid dependency {dependency.Key} for {dependency.Value.GetType().Name}");
                }
            }

            Debug.Log("[DI] Validate Done! No missing dependency");
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void ResetStatic()
        {
            _dependencyContainer.Clear();
            _sceneContainer.Clear();
            global = null;
        }

#if UNITY_EDITOR
        [MenuItem("GameObject/Dependency Injector/Add Global Injector")]
        public static void CreateGlobalInjector()
        {
            var injector = new GameObject(k_globalInjectorName).AddComponent<GlobalInjectorBootstrapper>();
        }

        [MenuItem("GameObject/Dependency Injector/Add Scene Injector")]
        public static void CreateSceneInjector()
        {
            var injector = new GameObject(k_sceneInjectorName).AddComponent<SceneInjectorBootstrapper>();
        }
#endif
    }
}
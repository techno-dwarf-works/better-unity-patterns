using System;
using System.Collections.Generic;
using System.Linq;
using Better.UnityPatterns.Runtime.InjectionModule.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.UnityPatterns.Runtime.InjectionModule
{
    public static class IoCContainer
    {
        private static Dictionary<Scene, Dictionary<Type, IInjectable>> _objects =
            new Dictionary<Scene, Dictionary<Type, IInjectable>>();

        private static List<Scene> _loadedScenes = new List<Scene>();

        [RuntimeInitializeOnLoadMethod]
        private static void OnLoad()
        {
            var scene = SceneManager.GetActiveScene();
            OnSceneLoaded(scene, LoadSceneMode.Single);
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            UnregisterScene(scene);
            _loadedScenes.Remove(scene);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            IterateInjectableThroughScene(scene, Register);
            _loadedScenes.Add(scene);
            foreach (var loadedScene in _loadedScenes)
            {
                IterateInjectThroughScene(loadedScene);
            }
        }

        private static void IterateInjectThroughScene(Scene scene)
        {
            var genericType = typeof(IInject<>);
            foreach (var gameObject in scene.GetRootGameObjects())
            {
                foreach (var (neededType, injectable) in _objects.Values.SelectMany(x => x))
                {
                    var type = genericType.MakeGenericType(neededType);
                    var method = type.GetMethod("Inject");
                    if (method == null) continue;
                    var components = gameObject.GetComponentsInChildren(type);

                    foreach (var component in components)
                    {
                        method.Invoke(component, new object[] { injectable });
                    }
                }
            }
        }

        private static void IterateInjectableThroughScene(Scene scene, Action<Scene, IInjectable> iterateAction)
        {
            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var gameObject in rootGameObjects)
            {
                var injectables = gameObject.GetComponentsInChildren<IInjectable>();
                foreach (var injectable in injectables)
                {
                    iterateAction?.Invoke(scene, injectable);
                }
            }
        }

        public static void Register<T>(Scene scene, T obj) where T : IInjectable
        {
            Dictionary<Type, IInjectable> injectables = null;
            if(_objects.TryGetValue(scene, out var o))
            {
                injectables = o;
            }
            else
            {
                injectables = new Dictionary<Type, IInjectable>();
                _objects.Add(scene, injectables);
            }
            injectables.Add(obj.GetType(), obj);
        }

        public static void Unregister<T>(Scene scene, T obj) where T : IInjectable
        {
            if(_objects.TryGetValue(scene, out var o))
            {
                if (!o.ContainsKey(typeof(T))) return;
                o.Remove(typeof(T));
            }
        }
        
        public static void UnregisterScene(Scene scene)
        {
            if(_objects.ContainsKey(scene))
            {
                _objects.Remove(scene);
            }
        }

        public static bool Resolve<T>(out T value) where T : IInjectable
        {
            value = default;
            foreach (var o in _objects.Values)
            {
                if (!o.TryGetValue(typeof(T), out var obj)) continue;
                value = (T)obj;
                return true;
            }
            return false;
        }
    }
}
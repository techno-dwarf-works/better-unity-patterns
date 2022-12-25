using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Better.UnityPatterns.Runtime.InjectionModule.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Better.UnityPatterns.Runtime.InjectionModule
{
    public static class IoCContainer
    {
        private class Container
        {
            public Container(object obj)
            {
                Obj = obj;
            }

            public object Obj { get; }
            public bool IsSet { get; private set; } = false;

            public void MarkSet()
            {
                IsSet = true;
            }
        }

        private static Dictionary<Scene, Dictionary<Type, IInjectable>> _objects =
            new Dictionary<Scene, Dictionary<Type, IInjectable>>();

        private static List<Scene> _loadedScenes = new List<Scene>();
        private static List<Container> _injectList = new List<Container>();
        private static Type _genericType = typeof(IInject<>);

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
            IterateInjectThroughList();
            _loadedScenes.Add(scene);
            foreach (var loadedScene in _loadedScenes)
            {
                IterateInjectThroughScene(loadedScene);
            }
        }

        private static void IterateInjectThroughScene(Scene scene)
        {
            var rootGameObjects = scene.GetRootGameObjects();
            foreach (var gameObject in rootGameObjects)
            {
                foreach (var keyValuePair in _objects.Values.SelectMany(x => x))
                {
                    if (!FindMethod(keyValuePair.Key, out var type, out var method)) continue;
                    var components = gameObject.GetComponentsInChildren(type);
                    foreach (var component in components)
                    {
                        Invoke(method, component, keyValuePair.Value);
                    }
                }
            }
        }

        private static bool FindMethod(Type generic, out Type type, out MethodInfo method)
        {
            type = _genericType.MakeGenericType(generic);
            method = type.GetMethod("Inject");
            return method != null;
        }

        private static void Invoke(MethodInfo method, object obj, IInjectable value)
        {
            method.Invoke(obj, new object[] { value });
        }

        private static void IterateInjectThroughList()
        {
            foreach (var obj in _injectList.Where(x => !x.IsSet))
            {
                foreach (var keyValuePair in _objects.Values.SelectMany(x => x))
                {
                    if (!FindMethod(keyValuePair.Key, out var type, out var method)) continue;

                    var t = obj.Obj.GetType()
                        .GetInterfaces()
                        .Any(i => i.IsGenericType &&
                                  i.GetGenericTypeDefinition() == _genericType &&
                                  i.GenericTypeArguments.Any(x => x == keyValuePair.Key));
                    if (!t) continue;
                    obj.MarkSet();
                    Invoke(method, obj.Obj, keyValuePair.Value);
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

        public static void Register<T>(IInject<T> inject) where T : IInjectable
        {
            _injectList.Add(new Container(inject));
        }

        public static void UnRegister<T>(IInject<T> inject) where T : IInjectable
        {
            _injectList.RemoveAll((x) => Equals(x.Obj, inject));
        }

        public static void Register<T>(Scene scene, T obj) where T : IInjectable
        {
            Dictionary<Type, IInjectable> injectables = null;
            if (_objects.TryGetValue(scene, out var o))
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

        public static void Unregister<T>(Scene scene) where T : IInjectable
        {
            if (_objects.TryGetValue(scene, out var o))
            {
                if (!o.ContainsKey(typeof(T))) return;
                o.Remove(typeof(T));
            }
        }

        public static void UnregisterScene(Scene scene)
        {
            if (_objects.ContainsKey(scene))
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
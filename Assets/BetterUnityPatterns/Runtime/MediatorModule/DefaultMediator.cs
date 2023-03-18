using System;
using System.Collections.Generic;
using Better.UnityPatterns.Runtime.MediatorModule.Interfaces;
using Better.UnityPatterns.Runtime.MediatorModule.Models;
using UnityEngine;

namespace Better.UnityPatterns.Runtime.MediatorModule
{
    [Serializable]
    public class DefaultMediator : IMediator
    {
        private List<IMediatorComponent> _components;

        private bool _isInitialized = false;

        public void SetComponent(List<IMediatorComponent> components)
        {
            if (ValidateInitialization()) return;
            _components = components;
        }

        public void AddComponent(IMediatorComponent mediatorComponent)
        {
            if (ValidateInitialization()) return;
            _components.Add(mediatorComponent);
        }

        public void RemoveComponent(IMediatorComponent mediatorComponent)
        {
            if (ValidateInitialization()) return;
            _components.Remove(mediatorComponent);
        }

        private bool ValidateInitialization()
        {
            if (!_isInitialized) return true;
            Debug.LogError($"Mediator is not Initialized. Call {nameof(Initialize)} before accessing other functionality");
            return false;
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            _components = new List<IMediatorComponent>();
            foreach (var component in _components)
            {
                component.Initialize(this);
            }

            _isInitialized = true;
        }

        public void Notify(MediatorEventArgs eventArgs)
        {
            if (ValidateInitialization()) return;
            for (var index = 0; index < _components.Count; index++)
            {
                if (RunComponent(eventArgs, index)) continue;

                if (eventArgs != null && eventArgs.IsUsed)
                {
                    return;
                }
            }
        }

        private bool RunComponent(MediatorEventArgs eventArgs, int index)
        {
            var mediatorComponent = _components[index];

            if (ValidateNullReference(mediatorComponent, index)) return true;

            if (eventArgs != null)
            {
                if (ValidateSender(eventArgs, mediatorComponent)) return true;
            }

            mediatorComponent.Notify(eventArgs);
            return false;
        }

        private static bool ValidateSender(MediatorEventArgs eventArgs, IMediatorComponent mediatorComponent)
        {
            if (eventArgs.ShouldIgnoreSender && mediatorComponent == eventArgs.Sender)
            {
                return true;
            }

            return false;
        }

        private static bool ValidateNullReference(IMediatorComponent mediatorComponent, int index)
        {
            if (mediatorComponent == null)
            {
                Debug.LogWarning($"Null reference captured at position {index}. This iteration will be ignored.");
                return true;
            }

            return false;
        }
    }
}
using System;
using System.Collections.Generic;
using Better.UnityPatterns.Runtime.StepMachine.Interfaces;

namespace Better.UnityPatterns.Runtime.StepMachine
{
    public abstract class BaseStepManager<T> where T : class, IBaseStep
    {
        private protected T _currentStep;
        private protected Queue<T> _registeredSteps = new Queue<T>();
        public event Action StepChanged;
        public event Action StepsDone;

        public void Initialize()
        {
            _currentStep = _registeredSteps.Dequeue();
            _currentStep.OnStepEnter();
        }
        
        public void RegisterStep(T step)
        {
            _registeredSteps.Enqueue(step);
        }

        protected virtual void OnStepChanged()
        {
            StepChanged?.Invoke();
        }

        protected virtual void OnStepsDone()
        {
            _currentStep.OnStepExit();
            _currentStep = null;
            StepsDone?.Invoke();
        }
    }
}
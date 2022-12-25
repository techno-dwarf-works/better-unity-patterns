using System;
using System.Threading.Tasks;
using Better.UnityPatterns.Runtime.StepMachine.Interfaces;

namespace Samples.StepMachineSamples.StepModels
{
    public class StringStep : IBaseStep
    {
        private readonly Action<string> _onConditionSatisfied;
        private readonly Func<string, bool> _condition;

        public StringStep(Action<string> onConditionSatisfied, Func<string, bool> condition)
        {
            _onConditionSatisfied = onConditionSatisfied;
            _condition = condition;
        }
        
        public bool TrySatisfy<T>(T data)
        {
            if (data is string value)
            {
                var isConditionValid = _condition.Invoke(value);
                if (!isConditionValid) return false;
                _onConditionSatisfied?.Invoke(value);
                return true;

            }
            return false;
        }

        public void OnStepEnter()
        {
        }

        public void OnStepExit()
        {
        }
    }
    
    public class LoadFileStep<TData> : IAsyncStep
    {
        private readonly Action<TData> _onConditionSatisfied;
        private readonly Func<string, Task<(TData, bool)>> _condition;

        public LoadFileStep(Action<TData> onConditionSatisfied, Func<string, Task<(TData, bool)>> condition)
        {
            _onConditionSatisfied = onConditionSatisfied;
            _condition = condition;
        }

        public async Task<bool> TrySatisfy<T>(T data)
        {
            if (data is string value)
            {
                var (parsedData, isValid) = await _condition.Invoke(value);
                if (!isValid) return false;
                _onConditionSatisfied?.Invoke(parsedData);
                return true;

            }
            return false;
        }

        public void OnStepEnter()
        {
            
        }

        public void OnStepExit()
        {
            
        }
    }
}
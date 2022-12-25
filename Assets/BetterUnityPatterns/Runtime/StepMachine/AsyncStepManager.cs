using System.Threading.Tasks;
using Better.UnityPatterns.Runtime.StepMachine.Interfaces;

namespace Better.UnityPatterns.Runtime.StepMachine
{
    public class AsyncStepManager : BaseStepManager<IAsyncStep>
    {
        public async Task<bool> TrySatisfy<T>(T data)
        {
            var isSatisfied = _currentStep != null && await _currentStep.TrySatisfy(data);
            if (!isSatisfied) return false;
            if (_registeredSteps.Count <= 0)
            {
                OnStepsDone();
                return false;
            }
            _currentStep.OnStepExit();
            _currentStep = _registeredSteps.Dequeue();
            _currentStep.OnStepEnter();
            OnStepChanged();
            return true;
        }
    }
}
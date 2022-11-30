using Better.UnityPatterns.Runtime.StepMachine.Interfaces;

namespace Better.UnityPatterns.Runtime.StepMachine
{
    public class StepManager : BaseStepManager<IStep>
    {
        public bool TrySatisfy<T>(T data)
        {
            var isSatisfied = _currentStep != null && _currentStep.TrySatisfy(data);
            if (!isSatisfied) return false;
            if (_registeredSteps.Count <= 0)
            {
                OnStepsDone();
                return false;
            }
            _currentStep = _registeredSteps.Dequeue();
            OnStepChanged();
            return true;
        }
    }
}
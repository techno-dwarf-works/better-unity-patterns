using System.Threading.Tasks;

namespace Better.UnityPatterns.Runtime.StepMachine.Interfaces
{
    public interface IBaseStep
    {
        public void OnStepEnter();
        public void OnStepExit();
    }

    public interface IStep : IBaseStep
    {
        public bool TrySatisfy<T>(T data);
    }

    public interface IAsyncStep : IBaseStep
    {
        public Task<bool> TrySatisfy<T>(T data);
    }
}
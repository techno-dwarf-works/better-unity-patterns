using Better.UnityPatterns.Runtime.MediatorModule.Models;

namespace Better.UnityPatterns.Runtime.MediatorModule.Interfaces
{
    public interface IMediatorComponent
    {
        public void Initialize(IMediator mediator);

        public void Notify(MediatorEventArgs eventArgs);
    }
}
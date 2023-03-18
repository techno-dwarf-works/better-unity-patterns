using Better.UnityPatterns.Runtime.MediatorModule.Models;

namespace Better.UnityPatterns.Runtime.MediatorModule.Interfaces
{
    public interface IMediator
    {
        public void Notify(MediatorEventArgs eventArgs);
    }
}
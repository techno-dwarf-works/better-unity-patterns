using JetBrains.Annotations;

namespace Better.UnityPatterns.Runtime.InjectionModule.Interfaces
{
    public interface IInject<in T> where T : IInjectable
    {
        [UsedImplicitly]
        public void Inject(T reference);
    }
}
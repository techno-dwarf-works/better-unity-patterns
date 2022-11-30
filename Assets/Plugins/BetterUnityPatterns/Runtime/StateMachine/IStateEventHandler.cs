using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine
{
    public interface IStateEventHandler<TState>
        where TState : BaseState
    {
        public void Construct(StateMachine<TState> stateMachine);
    }
}
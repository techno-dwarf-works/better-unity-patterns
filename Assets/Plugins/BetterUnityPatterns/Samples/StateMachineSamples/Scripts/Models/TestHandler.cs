using Better.UnityPatterns.Runtime.StateMachine;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Samples.StateMachineSamples.Models
{
    public class TestHandler<TState> : IStateEventHandler<TState> where TState : BaseState
    {
        private StateMachine<TState> _stateMachine;

        private TState _jumpState;

        public void Construct(StateMachine<TState> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void RegisterJumpState(TState jumpState)
        {
            _jumpState = jumpState;
        }

        public void TryJump()
        {
            _stateMachine.ChangeState(_jumpState);
        }
    }
}
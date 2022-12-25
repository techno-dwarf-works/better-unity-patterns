using System;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine.Transitions
{
    public class AnyToTransition<TState> : Transition<TState> where TState : BaseState
    {
        private Func<bool> predicate;

        public AnyToTransition(TState to, Func<bool> predicate) : base(to)
        {
            this.predicate = predicate;
        }

        public override bool Validate(TState current)
        {
            if (current == To)
            {
                return false;
            }

            return predicate.Invoke();
        }
    }
}
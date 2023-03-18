using System;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine.Transitions
{
    public class AnyToTransition<TState> : Transition<TState> where TState : BaseState
    {
        private readonly ITransitionCondition _predicate;

        public AnyToTransition(TState to, ITransitionCondition predicate) : base(to)
        {
            _predicate = predicate;
        }

        public override bool Validate(TState current)
        {
            if (current == To)
            {
                return false;
            }

            var condition = _predicate.GetCondition();
            if (condition)
            {
                _predicate.Reset();
            }

            return condition;
        }
    }
}
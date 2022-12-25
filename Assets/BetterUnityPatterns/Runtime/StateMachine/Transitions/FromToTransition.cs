using System;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine.Transitions
{
    public class FromToTransition<TState> : Transition<TState> where TState : BaseState
    {
        private readonly Func<bool> _predicate;
        public TState From { get; }

        public FromToTransition(TState from, TState to, Func<bool> predicate) : base(to)
        {
            From = from;
            _predicate = predicate;
        }

        public override bool Validate(TState current)
        {
            if (current != From)
            {
                return false;
            }

            return _predicate.Invoke();
        }
    }
}
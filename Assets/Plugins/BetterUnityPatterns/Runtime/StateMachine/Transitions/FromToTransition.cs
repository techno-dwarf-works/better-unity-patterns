using System;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine.Transitions
{

    public class FromToTransition<TState> : Transition<TState> where TState : BaseState
    {

        private Func<bool> predicate;
        public TState From { get; }

        public FromToTransition(TState from, TState to, Func<bool> predicate) : base(to)
        {
            From = from;
            this.predicate = predicate;
        }

        public override bool Validate(TState current)
        {
            if (current != From)
            {
                return false;
            }

            return predicate.Invoke();
        }

    }

}

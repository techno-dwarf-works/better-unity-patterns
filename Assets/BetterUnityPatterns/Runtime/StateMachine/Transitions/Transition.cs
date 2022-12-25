using System;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine.Transitions
{
    public abstract class Transition<TState> where TState : BaseState
    {
        public TState To { get; }
        Predicate<TState> predicate;

        public Transition(TState to)
        {
            To = to;
        }

        public abstract bool Validate(TState current);
    }
}
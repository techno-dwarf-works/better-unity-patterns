using System;
using Better.UnityPatterns.Runtime.StateMachine.States;

namespace Better.UnityPatterns.Runtime.StateMachine.Transitions
{
    public interface ITransitionCondition
    {
        public bool GetCondition();
        public void Reset();
    }

    public class DelegateCondition : ITransitionCondition
    {
        private readonly Func<bool> _predicate;

        public DelegateCondition(Func<bool> predicate)
        {
            _predicate = predicate;
        }

        public bool GetCondition()
        {
            return _predicate.Invoke();
        }

        public void Reset()
        {
        }
    }
    
    public class TriggerCondition : ITransitionCondition
    {
        private bool _trigger;
        
        public TriggerCondition()
        {
        }

        public void Trigger()
        {
            _trigger = true;
        }
        
        public bool GetCondition()
        {
            return _trigger;
        }

        public void Reset()
        {
            _trigger = false;
        }
    }
    
    public class FromToTransition<TState> : Transition<TState> where TState : BaseState
    {
        public TState From { get; }
        private readonly ITransitionCondition _predicate;

        public FromToTransition(TState from, TState to, ITransitionCondition predicate) : base(to)
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

            var condition = _predicate.GetCondition();
            if (condition)
            {
                _predicate.Reset();
            }
            return condition;
        }
    }
}
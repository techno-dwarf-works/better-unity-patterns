using System;
using System.Collections.Generic;
using Better.UnityPatterns.Runtime.StateMachine.States;
using Better.UnityPatterns.Runtime.StateMachine.Transitions;

namespace Better.UnityPatterns.Runtime.StateMachine
{
    public class DefaultTransitionManager<TState> : ITransitionManager<TState> where TState : BaseState
    {
        private readonly Dictionary<TState, List<Transition<TState>>> _outfromingTransitions;
        private readonly List<Transition<TState>> _anyToTransitions;
        private List<Transition<TState>> _currentTransitions;

        public DefaultTransitionManager()
        {
            _outfromingTransitions = new Dictionary<TState, List<Transition<TState>>>();
            _anyToTransitions = new List<Transition<TState>>();
            _currentTransitions = new List<Transition<TState>>();
        }

        public void AddTransition(TState from, TState to, Func<bool> predicate)
        {
            var transition = new FromToTransition<TState>(from, to, predicate);
            var key = transition.From;

            if (!_outfromingTransitions.TryGetValue(key, out var transitions))
            {
                transitions = new List<Transition<TState>>();
                _outfromingTransitions.Add(key, transitions);
            }
            transitions.Add(transition);
        }

        public void AddTransition(TState to, Func<bool> predicate)
        {
            var transition = new AnyToTransition<TState>(to, predicate);
            _anyToTransitions.Add(transition);
        }

        public bool FindTransition(TState currentState, out TState nextState)
        {
            nextState = null;
            foreach (var transition in _currentTransitions)
            {
                if (transition.Validate(currentState))
                {
                    nextState = transition.To;
                    return true;
                }
            }

            return false;
        }

        public void RunTransitionFor(TState state)
        {
            if (_outfromingTransitions.TryGetValue(state, out _currentTransitions))
            {
                _currentTransitions.AddRange(_anyToTransitions);
            }
            else
            {
                _currentTransitions = _anyToTransitions;
            }
        }
    }
}
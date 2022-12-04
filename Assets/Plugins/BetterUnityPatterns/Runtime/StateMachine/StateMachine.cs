using System;
using Better.UnityPatterns.Runtime.StateMachine.States;
using UnityEngine;

namespace Better.UnityPatterns.Runtime.StateMachine
{
    public class StateMachine<TState> : ITransitionManager<TState> where TState : BaseState
    {
        private float _timeNextTick;
        private readonly float _tickTimestep;

        private TState _currentState;
        private ITransitionManager<TState> _transitionManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tickStep">0 means unrestricted</param>
        public StateMachine(float tickStep = 0)
        {
            _tickTimestep = tickStep;
            _transitionManager = new DefaultTransitionManager<TState>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transitionManager"></param>
        /// <param name="tickStep">0 means unrestricted</param>
        public StateMachine(ITransitionManager<TState> transitionManager, float tickStep = 0)
        {
            _tickTimestep = tickStep;
            _transitionManager = transitionManager;
        }

        public void Tick(float tickTime)
        {
            if (_tickTimestep > _timeNextTick) return;

            if (_transitionManager.FindTransition(_currentState, out var nextState))
            {
                ChangeState(nextState);
                _timeNextTick = 0;
            }
            else
            {
                _timeNextTick += tickTime;
                _currentState.Tick(tickTime);
            }
        }

        public void ChangeState(TState newState)
        {
            if (newState == _currentState)
            {
                return;
            }

            _currentState?.Exit();
            _currentState = newState;
            newState.Enter();

            _transitionManager.RunTransitionFor(newState);
        }

        void ITransitionManager<TState>.RunTransitionFor(TState newState)
        {
            _transitionManager.RunTransitionFor(newState);
        }

        bool ITransitionManager<TState>.FindTransition(TState currentState, out TState nextState)
        {
            return _transitionManager.FindTransition(currentState, out nextState);
        }

        public void AddTransition(TState @from, TState to, Func<bool> predicate)
        {
            _transitionManager.AddTransition(@from, to, predicate);
        }

        public void AddTransition(TState to, Func<bool> predicate)
        {
            _transitionManager.AddTransition(to, predicate);
        }
    }

    public class StateMachine<TState, TEventHandler> : StateMachine<TState> where TState : BaseState
        where TEventHandler : IStateEventHandler<TState>, new()
    {
        public TEventHandler EventHandler { get; }

        /// <inheritdoc>
        ///     <cref>base.StateMachine()</cref>
        /// </inheritdoc>
        public StateMachine(float tickStep = 0) : base(tickStep)
        {
            EventHandler = new TEventHandler();
            EventHandler.Construct(this);
        }
        
        /// <inheritdoc>
        ///     <cref>base.StateMachine()</cref>
        /// </inheritdoc>
        public StateMachine(ITransitionManager<TState> transitionManager, float tickStep = 0) : base(transitionManager, tickStep)
        {
            EventHandler = new TEventHandler();
            EventHandler.Construct(this);
        }
    }
}
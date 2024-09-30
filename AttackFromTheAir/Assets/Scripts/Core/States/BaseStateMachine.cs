using System.Collections.Generic;
using DG.Tweening;

namespace Core.States
{
    public interface IStateMachine<T>
    {
        IState<T> Current { get; }
        IState<T> Previous { get; }
        void SwitchToState(T state);
        void SwitchToStateDelayed(T state, float delay);
        void InitiateStateMachine(params IState<T>[] states);
    }

    public class BaseStateMachine<T> : IStateMachine<T>
    {
        protected IStateMachine<T> _stateMachine;
        private Dictionary<T, IState<T>> _states = new Dictionary<T, IState<T>>();
        private IState<T> _current;
        private IState<T> _previous;
        public IState<T> Current => _current;
        public IState<T> Previous => _previous;

        public BaseStateMachine(params IState<T>[] states)
        {
            InitiateStateMachine(states);
        }

        public void SwitchToStateDelayed(T state, float delay)
        {
            DOTween.Sequence().AppendInterval(delay).OnComplete(()=> { SwitchToState(state); });
        }

        public void SwitchToState(T state)
        {
            _previous = _current;
            var nextState = _states[state];
            _current?.Exit();
            _current = nextState;
            _current.Prepare();
            _current.Enter();
        }

        public bool HasState(T state)
        {
            return _states.ContainsKey(state);
        }

        public void InitiateStateMachine(params IState<T>[] states)
        {
            _current?.Exit();
            _states.Clear();
            foreach (var state in states)
            {
                _states[state.State] = state;
                state.stateMachine = this;
            }
            _stateMachine = this;
        }
    }
}
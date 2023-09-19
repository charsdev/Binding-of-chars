using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StateMachine
{

    public class StateMachineController
    {
        private readonly List<Transition> _transitions = new List<Transition>();
        private readonly Dictionary<IState, List<Transition>> _states = new Dictionary<IState, List<Transition>>();
        private List<Transition> _currentTransitions;
        public Blackboard Blackboard { get; private set; } = new Blackboard();

        public IState CurrentState { get; private set; }

        public void AddTransition(Transition newTransition)
        {
            if (newTransition.From == null)
            {
                _transitions.Add(newTransition);
                return;
            }

            if (!_states.ContainsKey(newTransition.From))
            {
                _states.Add(newTransition.From, new List<Transition>());
            }

            _states[newTransition.From].Add(newTransition);
        }

        public void SetState(IState state)
        {
            CurrentState = state;

            CurrentState.OnEnter();
            _currentTransitions = new List<Transition>();
            _currentTransitions.AddRange(_transitions);

            if (!_states.ContainsKey(CurrentState)) return;

            _currentTransitions.AddRange(_states[CurrentState]);
        }

        public void Tick()
        {
            if (CurrentState == null) return;

            var transitionsThatSatisfy = _currentTransitions.Where(transition => transition.Predicate()).ToArray();

            if (transitionsThatSatisfy.Length == 0)
            {
                CurrentState.OnTick();
                return;
            }

            if (transitionsThatSatisfy.Length > 1)
            {
                Debug.LogError("More than one transition satisfies");
                return;
            }

            ChangeState(transitionsThatSatisfy.First());
        }

        private void ChangeState(Transition transition)
        {
            CurrentState?.OnExit();
            SetState(transition.To);
        }
    }
}





using UnityEngine;

namespace StateMachine
{
    public class AIStateMachine : MonoBehaviour
    {
        private readonly StateMachineController _stateMachine = new();

        private void Start()
        {
            var idleState = new IdleState();
            var walkState = new WalkState();
            var deadState = new DeadState();

            var idleToWalk = new Transition(idleState, walkState, () => _stateMachine.Blackboard.Get<bool>("IsMoving"));
            _stateMachine.AddTransition(idleToWalk);

            var walkToIdle = new Transition(walkState, idleState, () => !_stateMachine.Blackboard.Get<bool>("IsMoving"));
            _stateMachine.AddTransition(walkToIdle);

            var anyToDead = new Transition(null, deadState, () => _stateMachine.Blackboard.Get<int>("PlayerHealth") <= 0);
            _stateMachine.AddTransition(anyToDead);
        }

        private void Update()
        {
            _stateMachine.Tick();
        }
    }
}





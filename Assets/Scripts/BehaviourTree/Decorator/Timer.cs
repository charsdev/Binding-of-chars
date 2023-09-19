namespace BehaviourTree
{
    public class Timer : Node
    {
        private readonly Node _child;
        private readonly float _delay;
        private float _time;
        private System.Action OnTimerEnd;

        public Timer(Node child, float delay, System.Action onTimerEnd = null)
        {
            _delay = delay;
            _child = child;
            _time = delay;
            OnTimerEnd = onTimerEnd;
        }

        public override NodeState Evaluate()
        {
            if (_child == null)
            {
                return NodeState.FAILURE;
            }

            if (_time <= 0)
            {
                _time = _delay;
                _child.Evaluate();
                OnTimerEnd?.Invoke();
                return NodeState.SUCCESS;
            }

            _time -= UnityEngine.Time.deltaTime;
            return NodeState.RUNNING;
        }
    }
}

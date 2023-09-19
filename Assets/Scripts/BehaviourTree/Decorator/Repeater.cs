namespace BehaviourTree
{
    public class Repeater : Node
    {
        private readonly Node _child;
        private readonly int _maxRepeats;
        private int _currentRepeat;

        public Repeater(Node child, int repeatCount, int currentRepeat)
        {
            _child = child;
            _maxRepeats = repeatCount;
            _currentRepeat = currentRepeat;
        }

        public override NodeState Evaluate()
        {
            if (_child.Evaluate() == NodeState.FAILURE)
            {
                return NodeState.FAILURE;
            }

            if (_currentRepeat < _maxRepeats)
            {
                _currentRepeat++;
                return NodeState.RUNNING;
            }

            _currentRepeat = 0;
            return NodeState.SUCCESS;
        }
    }
}

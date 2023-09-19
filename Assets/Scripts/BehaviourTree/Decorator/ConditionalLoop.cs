using System;

namespace BehaviourTree
{
    public class ConditionalLoop : Node
    {
        private readonly Node _child;
        private readonly Func<NodeState, bool> _condition;

        public ConditionalLoop(Node child, Func<NodeState, bool> condition)
        {
            _child = child;
            _condition = condition;
        }

        public override NodeState Evaluate()
        {
            var result = NodeState.RUNNING;

            while (_condition(result))
            {
                result = _child.Evaluate();
            }

            return result;
        }
    }
}

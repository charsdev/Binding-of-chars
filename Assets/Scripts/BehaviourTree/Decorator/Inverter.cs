using System.Collections.Generic;

namespace BehaviourTree
{
    public class Inverter : Node
    {
        private readonly Node _child;

        private Dictionary<NodeState, NodeState> _invertStates = new Dictionary<NodeState, NodeState>()
        {
            { NodeState.SUCCESS, NodeState.FAILURE },
            { NodeState.FAILURE, NodeState.SUCCESS },
            { NodeState.RUNNING, NodeState.RUNNING }
        };

        public Inverter(Node child)
        {
            _child = child;
        }

        public override NodeState Evaluate()
        {
            if (_child == null)
            {
                return NodeState.FAILURE;
            }

            return _invertStates[_child.Evaluate()];
        }
    }
}

using System.Collections.Generic;

namespace BehaviourTree
{
    public class Sequence : Node
    {
        private readonly List<Node> _nodes = new List<Node>();

        public Sequence(List<Node> nodes)
        {
            _nodes = nodes;
        }

        public override NodeState Evaluate()
        {
            bool childRunning = false;

            foreach (var node in _nodes)
            {
                switch (node.Evaluate())
                {
                    case NodeState.RUNNING:
                        childRunning = true;
                        continue;
                    case NodeState.SUCCESS:
                        continue;
                    case NodeState.FAILURE:
                        return NodeState.FAILURE;
                }
            }

            return childRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }
    }

}
using System.Collections.Generic;

namespace BehaviourTree
{
    public class Parallel : Node
    {
        private readonly List<Node> _nodes = new List<Node>();

        public override NodeState Evaluate()
        {
            bool childRunning = false;
            int failedNodes = 0;

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
                        failedNodes++;
                        continue;
                }
            }

            if (failedNodes == _nodes.Count)
            {
                return NodeState.FAILURE;
            }

            return childRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        }
    }
}
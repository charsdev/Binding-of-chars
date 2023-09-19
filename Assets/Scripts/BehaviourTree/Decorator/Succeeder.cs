namespace BehaviourTree
{
    public class Succeeder : Node
    {
        private readonly Node _child;

        public Succeeder(Node child)
        {
            _child = child;
        }

        public override NodeState Evaluate()
        {
            _child.Evaluate();
            return NodeState.SUCCESS;
        }
    }
}

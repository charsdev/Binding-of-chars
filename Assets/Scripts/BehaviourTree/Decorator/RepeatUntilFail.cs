namespace BehaviourTree
{
    public class RepeatUntilFail : Node
    {
        private readonly Node _child;

        public RepeatUntilFail(Node child)
        {
            _child = child;
        }

        public override NodeState Evaluate()
        {
            while (_child.Evaluate() != NodeState.FAILURE) { }
            return NodeState.SUCCESS;
        }
    }
}

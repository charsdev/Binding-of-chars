namespace StateMachine
{
    public interface IState
    {
        public void OnEnter();
        public void OnTick();
        public void OnExit();
    }
}
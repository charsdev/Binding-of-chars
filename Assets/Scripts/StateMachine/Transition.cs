using System;

namespace StateMachine
{
    public class Transition
    {
        public Transition(IState from, IState to, Func<bool> predicate)
        {
            From = from;
            To = to;
            Predicate = predicate;
        }

        public IState From { get; set; }
        public IState To { get; set; }
        public Func<bool> Predicate { get; set; }
    }
}
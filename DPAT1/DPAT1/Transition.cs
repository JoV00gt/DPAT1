using DPAT1.Interfaces;

namespace DPAT1
{
    public class Transition
    {
        private string id;
        private IState? source;
        private IState? target;

        public Transition(string id, IState? source, IState? target)
        {
            this.id = id;
            this.source = source;
            this.target = target;
        }

        internal void Add(Transition transition)
        {
            throw new NotImplementedException();
        }

        internal void SetGuardCondition(string guard)
        {
            throw new NotImplementedException();
        }

        internal void SetTrigger(Trigger t)
        {
            throw new NotImplementedException();
        }
    }
}
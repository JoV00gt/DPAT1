namespace DPAT1
{
    internal class Transition
    {
        private string id;
        private State? source;
        private State? target;

        public Transition(string id, State? source, State? target)
        {
            this.id = id;
            this.source = source;
            this.target = target;
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
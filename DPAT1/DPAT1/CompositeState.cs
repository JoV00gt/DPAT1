namespace DPAT1
{
    internal class CompositeState : State
    {
        private string id;
        private string name;

        public CompositeState(string id, string name)
        {
            this.id = id;
            this.name = name;
        }

        internal void Add(State state)
        {
            throw new NotImplementedException();
        }
    }
}
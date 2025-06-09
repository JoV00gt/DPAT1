namespace DPAT1
{
    internal class InitialState : State
    {
        private string id;
        private string name;

        public InitialState(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
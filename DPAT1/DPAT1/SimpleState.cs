namespace DPAT1
{
    internal class SimpleState : State
    {
        private string id;
        private string name;

        public SimpleState(string id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
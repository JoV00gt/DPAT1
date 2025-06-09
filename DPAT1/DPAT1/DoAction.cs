namespace DPAT1
{
    internal class DoAction : Action
    {
        private string id;
        private string description;

        public DoAction(string id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
}
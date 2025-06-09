namespace DPAT1.Actions
{
    internal class ExitAction : Action
    {
        private string id;
        private string description;

        public ExitAction(string id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
}
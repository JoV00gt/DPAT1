namespace DPAT1.Actions
{
    internal class TransitionAction : Action
    {
        private string id;
        private string description;

        public TransitionAction(string id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
}
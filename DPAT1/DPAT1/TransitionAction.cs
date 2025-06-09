namespace DPAT1
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
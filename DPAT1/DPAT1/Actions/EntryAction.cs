namespace DPAT1.Actions
{
    internal class EntryAction : Action
    {
        private string id;
        private string description;

        public EntryAction(string id, string description)
        {
            this.id = id;
            this.description = description;
        }
    }
}
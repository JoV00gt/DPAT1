namespace DPAT1
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
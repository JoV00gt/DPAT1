namespace DPAT1
{
    public class Trigger
    {
        private string id;
        private string description;

        public Trigger(string id, string description)
        {
            this.id = id;
            this.description = description;
        }

        public object Id { get; internal set; }
    }
}
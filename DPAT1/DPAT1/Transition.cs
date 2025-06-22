using DPAT1.Interfaces;

namespace DPAT1
{
    public class Transition
    {
        public string Id { get; set; }
        public IState Source { get; set; }
        public IState Target { get; set; }

        public Trigger? Trigger { get; set; }
        public string? Guard { get; set; }
        public Action? Effect { get; set; }


        public Transition(string id, IState source, IState target, Trigger? trigger = null, string? guard = null, Action? effect = null)
        {
            this.Id = id;
            this.Source = source;
            this.Target = target;
            this.Trigger = trigger;
            this.Guard = guard;
            this.Effect = effect;
        }
            
        public string GetGuard()
        {
            return Guard;
        }

        public IState GetSource() { return Source; }
        public IState GetTarget() { return Target; }

    }
}
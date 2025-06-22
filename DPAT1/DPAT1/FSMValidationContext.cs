using DPAT1.Interfaces;

namespace DPAT1
{
    public class FSMValidationContext
    {
        private IFSMValidator strategy;

        public void SetStrategy(IFSMValidator strategy)
        {
            this.strategy = strategy;
        }

        public bool Validate(FSM fsm)
        {
            return strategy.IsValid(fsm);
        }
    }
}

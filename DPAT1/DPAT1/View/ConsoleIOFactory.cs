
using DPAT1.Interfaces;

namespace DPAT1.View
{
    public class ConsoleIOFactory : IUIFactory
    {
        private readonly FSM fsm;

        public ConsoleIOFactory(FSM fsm)
        {
            this.fsm = fsm;
        }

        public IOutputRenderer CreateRenderer()
        {
            return new ConsoleRenderer(fsm);
        }

        public IVisitor CreateVisitor()
        {
            return new PrintVisitor();
        }
    }
}

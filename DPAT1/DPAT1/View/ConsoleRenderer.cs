using DPAT1.Interfaces;


namespace DPAT1.View
{
    public class ConsoleRenderer : IOutputRenderer
    {
        public FSM FSM { get; }

        public ConsoleRenderer(FSM fsm)
        {
            FSM = fsm;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}

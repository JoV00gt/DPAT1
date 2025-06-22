

namespace DPAT1.Interfaces
{
    public interface IUIFactory
    {
        IOutputRenderer CreateRenderer(); 
        IVisitor CreateVisitor();
    }
}

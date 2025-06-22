using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.Interfaces
{
    internal interface IOutputRenderer
    {
        void Accept(IVisitor visitor); // Renderer accepts a visitor
    }
}

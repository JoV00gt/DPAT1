using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1.Interfaces
{
    public interface IOutputRenderer
    {
        void Accept(IVisitor visitor);
    }
}

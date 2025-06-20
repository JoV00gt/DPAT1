using DPAT1.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DPAT1
{
    public class Action
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public ActionType Type { get; set; }

        public string GetDescription()
        {
            return Description;
        }

        public ActionType GetActionType()
        {
            return Type;
        }
    }
}

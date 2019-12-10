using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class AgentCategory : EntityBase
    {
        public string CategoryName { get; set; }
        public int Commission { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class ERPAccounts : EntityBase 
    {
        public int Type { get; set; }
        public string AccountCode { get; set; }
    }
}

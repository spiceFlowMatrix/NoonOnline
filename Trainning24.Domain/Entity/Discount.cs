using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public  class Discount : EntityBase
    {
        public string PackageName { get; set; }
        public int OffTotalPrice { get; set; }
        public int OffSubscriptions { get; set; }
        public int FreeSubscriptions { get; set; }
    }
}

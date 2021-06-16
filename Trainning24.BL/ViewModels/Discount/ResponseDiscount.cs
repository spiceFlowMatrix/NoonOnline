using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Discount
{
    public class ResponseDiscount
    {
        public long Id { get; set; }
        public string PackageName { get; set; }
        public int OffTotalPrice { get; set; }
        public int OffSubscriptions { get; set; }
        public int FreeSubscriptions { get; set; }
    }
}

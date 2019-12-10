using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Deposit
{
    public class AddDeposit
    {
        public long id { get; set; }
        public string depositdate { get; set; }
        public decimal depositamount { get; set; }
        public long salesagentid { get; set; }
        public string  documentids { get; set; }
    }
}

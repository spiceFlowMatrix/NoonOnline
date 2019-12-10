using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.IndividualDetails;

namespace Trainning24.BL.ViewModels.Deposit
{
    public  class ResponseDeposit
    {
        public long id { get; set; }
        public string depositdate { get; set; }
        public decimal depositamount { get; set; }
        public long salesagentid { get; set; }
        public string salesgentname { get; set; }
        public bool isrevoke { get; set; }
        public bool isconfirm { get; set; }
        //public long documentid { get; set; }
        public List<DocumentDetailsResponse> documentid { get; set; }
    }

    public class AgentAccountSummary
    {
        public long agentid { get; set; }
        public string agentname { get; set; }
        public decimal latestdeposit { get; set; }
        public decimal currentbalance { get; set; }
        public decimal totalexpenditure { get; set; }
        public decimal totaldeposit { get; set; }
        public int purchasecount { get; set; }
        public int depositcount { get; set; }
    }


    //public class AgentAccountSummary
    //{
    //    public long agentid { get; set; }
    //    public string agentname { get; set; }
    //    public decimal latestdeposit { get; set; }
    //    public decimal currentbalance { get; set; }
    //    public decimal totalexpenditure { get; set; }
    //    public decimal totaldeposit { get; set; }
    //    public int purchasecount { get; set; }
    //    public int depositcount { get; set; }
    //}

}

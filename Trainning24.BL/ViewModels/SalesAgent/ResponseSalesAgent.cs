using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.SalesAgent
{
    public class ResponseSalesAgent
    {
        public long SalesAgentId { get; set; }
        public string AgentName { get; set; }
        public long AgentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        //New fields
        public string PartnerBackgroud { get; set; }
        public string FocalPoint { get; set; }
        public string Position { get; set; }
        public string PhysicalAddress { get; set; }
        //public long CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public int AgreedMonthlyDeposit { get; set; }
        public bool IsActive { get; set; }

        // Added for user on sales panel
        public string username { get; set; }
        public string phonenumber { get; set; }
    }


    public class CurrencyResponse 
    {
        public long id { get; set; }
        public string currency { get; set; }
        public string abbreviation { get; set; }
        public string symbol { get; set; }
    }
}

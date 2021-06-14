using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.AddtionalServices;
using Trainning24.BL.ViewModels.CourseDefination;

namespace Trainning24.BL.ViewModels.SubscriptionMetadata
{
    public class SubscriptionMetadataModel
    {
        public long id { get; set; }
        public List<ResponseCourseDefination> courseids { get; set; }
        public long discountpackageid { get; set; }
        public string discountpackagename { get; set; }
        public long salesagentid { get; set; }
        public string saleagentname { get; set; }
        public string enrollmentfromdate { get; set; }
        public string enrollmenttodate { get; set; }
        public int subscriptiontypeid { get; set; }
        public int noofmonths { get; set; }
        public long packageid { get; set; }
        public List<int> serviceids { get; set; }
        public string purchasedate { get; set; }
        public string lastmodifieddate { get; set; }
    }

    public class InputSubscriptionMetadata
    {
        public long id { get; set; }
        public List<int> courseids { get; set; }
        public long discountpackageid { get; set; }
        public long salesagentid { get; set; }
        public string enrollmentfromdate { get; set; }
        public string enrollmenttodate { get; set; }
        public int subscriptiontypeid { get; set; }
        public int noofmonths { get; set; }
        public long packageid { get; set; }
        public List<int> serviceids { get; set; }
    }

    public class PurchaseList
    {
        public long id { get; set; }                        
        public string purchaseid { get; set; }
        public string purchasedate { get; set; }
        public string status { get; set; }
        public string lastmodifieddate { get; set; }
    }

    public class PurchaseListFilterModel
    {
        public bool isfilterneeded { get; set; }
        public string courseid { get; set; }
        public long discountpackageid { get; set; }
        public long salesagentid { get; set; }
        public string enrollmentfromdate { get; set; }
        public string enrollmenttodate { get; set; }
        public string status { get; set; }
    }

    public class AboutAllPurchase
    {
        public int noofpurchase { get; set; }
        public int noofsubscription { get; set; }
        public decimal totalamountcompleted { get; set; }
        public decimal totalamountpending { get; set; }
        public decimal totalamountcompletedpending { get; set; }
    }

}

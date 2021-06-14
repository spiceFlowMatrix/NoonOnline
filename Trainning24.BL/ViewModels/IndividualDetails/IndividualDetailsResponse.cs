using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.SubscriptionMetadata;
using Trainning24.BL.ViewModels.Subscriptions;

namespace Trainning24.BL.ViewModels.IndividualDetails
{
    public class IndividualDetailsResponse
    {
        public long id { get; set; }
        public string studentcode { get; set; }
        public string studentname { get; set; }
        public string schoolname { get; set; }
        public string fathernumber { get; set; }
        public string idcardnumber { get; set; }
        public string permanentaddress { get; set; }
        public string village { get; set; }
        public string provinceid { get; set; }
        public string cityid { get; set; }
        public string phone { get; set; }
        public string countryid { get; set; }
        public string refferedby { get; set; }
        public string email { get; set; }
        public string passportnumber { get; set; }
        public string nationality { get; set; }
        public string sexid { get; set; }
        public string dateofbirth { get; set; }
        public string placeofbirth { get; set; }
        public string currentaddress { get; set; }
        public string maritalstatusid { get; set; }
        public string remarks { get; set; }
        public List<DocumentDetailsResponse> studenttazkira { get; set; }        
        public List<DocumentDetailsResponse> parenttazrika { get; set; }
        public List<DocumentDetailsResponse> previousmarksheets { get; set; }
    }

    public class DocumentDetailsResponse
    {
        public long id { get; set; }
        public string documenturl { get; set; }
        public string name { get; set; }
    }

    public class SchoolDetailsResponse
    {
        public long id { get; set; }
        public string registernumber { get; set; }
        public string schooltypeid { get; set; }
        public string schoolname { get; set; }
        public string schooladdress { get; set; }
        public string sectiontypeid { get; set; }
        public int numberofteachermale { get; set; }
        public int numberofteacherfemale { get; set; }
        public int numberofstudentmale { get; set; }
        public int numberofstudentfemale { get; set; }
        public int numberofstaffmale { get; set; }
        public int numberofstafffemale { get; set; }
        public string poweraddressid { get; set; }
        public string internetaccessid { get; set; }
        public string buildingownershipid { get; set; }
        public string computers { get; set; }
        public string monitors { get; set; }
        public string routers { get; set; }
        public string dongles { get; set; }
        public List<DocumentDetailsResponse> schoollicense { get; set; }
        public List<DocumentDetailsResponse> registerationpaper { get; set; }        
    }


    public class PurchaseDetailResponse
    {
        public SubscriptionMetadataModel metadatadetails { get; set; }
        public List<IndividualDetailsResponse> individualdetails { get; set; }
        public SchoolDetailsResponse schooldetails { get; set; }
        public SubscriptionsModel Subscriptions { get; set; }
    }
}

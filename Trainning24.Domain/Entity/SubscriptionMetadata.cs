using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class SubscriptionMetadata : EntityBase
    {
        public string CourseId { get; set; }
        public long DiscountPackageId { get; set; }
        public long SalesAgentId { get; set; }
        public string EnrollmentFromDate { get; set; }
        public string EnrollmentToDate { get; set; }
        public int SubscriptionTypeId { get; set; }
        public string Status { get; set; }
        public string PurchageId { get; set; }
        public int NoOfMonths { get; set; }
        public long PackageId { get; set; }
        public string ServiceId { get; set; }
        public int Tax { get; set; }
    }
}

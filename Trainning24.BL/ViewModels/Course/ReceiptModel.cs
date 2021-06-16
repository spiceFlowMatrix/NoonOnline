using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Course
{
    public class ReceiptModel
    {
        public string Course { get; set; }
        public string Grade { get; set; }
        public string Package { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string SalesPartner { get; set; }
        public string AdditionalGradePurchased { get; set; }
        public string Total { get; set; }
        public string PackageDiscount { get; set; }
        public string Tax { get; set; }
        public string Paid { get; set; }
        //public List<StudentInfo> students { get; set; }
        public string students { get; set; }
    }

    public class StudentInfo
    {
        public long StudentId { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
    } 

}

using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class IndividualDetails : EntityBase
    {
        public string StudentCode { get; set; }
        public string StudentName { get; set; }
        public string SchoolName { get; set; }
        public string FatherNumber { get; set; }
        public string IdCardNumber { get; set; }
        public string PermanentAddress { get; set; }
        public string Village { get; set; }
        public string ProvinceId { get; set; }
        public string CityId { get; set; }
        public string Phone { get; set; }
        public string CountryId { get; set; }
        public string RefferedBy { get; set; }
        public string Email { get; set; }
        public string PassportNumber { get; set; }
        public string Nationality { get; set; }
        public string SexId { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string CurrentAddress { get; set; }
        public string MaritalStatusId { get; set; }
        public string Remarks { get; set; }
        public string StudentTazkira { get; set; }
        public string ParentTazrika { get; set; }
        public string PreviousMarksheets { get; set; }
        public long? GradeId { get; set; }
        public string SoicalMediaLinked { get; set; }
        public string SocialMediaAccount { get; set; }
        public long? UserId { get; set; }
    }

    public class DocumentDetails : EntityBase
    {
        public string name { get; set; }
        public string DocumentUrl { get; set; }
    }

    public class SchoolDetails : EntityBase
    {
        public string RegisterNumber { get; set; }
        public string SchoolTypeId { get; set; }
        public string SchoolName { get; set; }
        public string SchoolAddress { get; set; }
        public string SectionTypeId { get; set; }
        public int NumberOfTeacherMale { get; set; }
        public int NumberOfTeacherFemale { get; set; }
        public int NumberOfStudentMale { get; set; }
        public int NumberOfStudentFemale { get; set; }
        public int NumberOfStaffMale { get; set; }
        public int NumberOfStaffFemale { get; set; }
        public string PowerAddressId { get; set; }
        public string InternetAccessId { get; set; }
        public string BuildingOwnershipId { get; set; }
        public string Computers { get; set; }
        public string Monitors { get; set; }
        public string Routers { get; set; }
        public string Dongles { get; set; }
        public string SchoolLicense { get; set; }
        public string RegisterationPaper { get; set; }
    }


    public class PriceSummaryModel
    {
        public long metadataid { get; set; }
        public string totalsubscriptions { get; set; }
        public string totalbaseprice { get; set; }
        public string finalprice { get; set; }
        public string status { get; set; }
        public string receipturl { get; set; }
    }


    public class UploadSignedReceiptModel
    {
        public long metadataid { get; set; }
        public string status { get; set; }
    }

}

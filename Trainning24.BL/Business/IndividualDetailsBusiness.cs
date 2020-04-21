using System;
using System.Collections.Generic;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using System.Linq;
using System.Web.Helpers;
using Trainning24.BL.ViewModels.Users;
using System.Threading.Tasks;

namespace Trainning24.BL.Business
{
    public class IndividualDetailsBusiness
    {
        private readonly EFIndividualDetails EFIndividualDetails;

        public IndividualDetailsBusiness(
            EFIndividualDetails EFIndividualDetails
        )
        {
            this.EFIndividualDetails = EFIndividualDetails;
        }


        public string ReturnCode()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            if ("1" == "1")
            {
                characters += alphabets + small_alphabets + numbers;
            }

            string otp = string.Empty;
            for (int i = 0; i < 5; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }


            return Crypto.Hash(otp, "MD5");
        }


        public PurchagePdf CreatePurchagePdf(PurchagePdf PurchagePdf, int id)
        {
            PurchagePdf.CreatorUserId = id;
            PurchagePdf.CreationTime = DateTime.Now.ToString();
            EFIndividualDetails.Insert(PurchagePdf);
            return PurchagePdf;
        }


        public PurchageUpload CreatePurchageUpload(PurchageUpload PurchageUpload, int id)
        {
            PurchageUpload.CreatorUserId = id;
            PurchageUpload.CreationTime = DateTime.Now.ToString();
            EFIndividualDetails.Insert(PurchageUpload);
            return PurchageUpload;
        }


        public IndividualDetails Create(IndividualDetails IndividualDetails, int id)
        {
            IndividualDetails.IsDeleted = false;
            IndividualDetails.CreationTime = DateTime.Now.ToString();
            IndividualDetails.CreatorUserId = id;

            EFIndividualDetails.Insert(IndividualDetails);

            return IndividualDetails;
        }

        public IndividualDetails Update(IndividualDetails ind, int id)
        {
            IndividualDetails obj = GetIndividualDetailById(ind.Id);

            obj.CityId = ind.CityId;
            obj.CountryId = ind.CountryId;
            obj.CurrentAddress = ind.CurrentAddress;
            obj.DateOfBirth = ind.DateOfBirth;
            obj.Email = ind.Email;
            obj.FatherNumber = ind.FatherNumber;
            obj.IdCardNumber = ind.IdCardNumber;
            obj.LastModificationTime = DateTime.Now.ToString();
            obj.LastModifierUserId = id;
            obj.Village = ind.Village;
            obj.MaritalStatusId = ind.MaritalStatusId;
            obj.Nationality = ind.Nationality;
            obj.ParentTazrika = ind.ParentTazrika;
            obj.PassportNumber = ind.PassportNumber;
            obj.PermanentAddress = ind.PermanentAddress;
            obj.Phone = ind.Phone;
            obj.PlaceOfBirth = ind.PlaceOfBirth;
            obj.PreviousMarksheets = ind.PreviousMarksheets;
            obj.ProvinceId = ind.ProvinceId;
            obj.RefferedBy = ind.RefferedBy;
            obj.Remarks = ind.Remarks;
            obj.SchoolName = ind.SchoolName;
            obj.SexId = ind.SexId;
            obj.StudentCode = ind.StudentCode;
            obj.StudentName = ind.StudentName;
            obj.StudentTazkira = ind.StudentTazkira;

            EFIndividualDetails.Update(obj);

            return obj;
        }

        public object GetIndividualList(string search)
        {
            var userListwithp = EFIndividualDetails.ListQuery(u => u.IsDeleted != true).Select(s => new
            {
                s.Id,
                s.StudentName,
                s.StudentCode,
                s.Email,
                s.Phone
            }).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                userListwithp = userListwithp.Where(
                                b => b.StudentCode.ToLower().Any(k => b.StudentCode.Contains(search.ToLower())) ||
                                b.StudentName.ToLower().Any(k => b.StudentName.Contains(search.ToLower()))).ToList();
            }
            return userListwithp;
        }

        public List<IndividualDetails> GetIndiviualDetails(string search)
        {
            return EFIndividualDetails.GetIndiviualDetails(search);
        }

        public List<SchoolDetails> GetSchoolDetails(string search)
        {
            return EFIndividualDetails.GetSchoolDetails(search);
        }

        public IndividualDetails GetIndividualDetailById(long id)
        {
            return EFIndividualDetails.GetIndividualDetailById(id);
        }

        public SchoolDetails GetSchoolDetailById(long id)
        {
            return EFIndividualDetails.GetSchoolDetailById(id);
        }

        public DocumentDetails CreateDocumentDetails(DocumentDetails DocumentDetails, int id)
        {
            DocumentDetails.IsDeleted = false;
            DocumentDetails.CreationTime = DateTime.Now.ToString();
            DocumentDetails.CreatorUserId = id;

            EFIndividualDetails.InsertDocumentDetails(DocumentDetails);

            return DocumentDetails;
        }

        public SchoolDetails CreateSchoolDetails(SchoolDetails SchoolDetails, int id)
        {
            SchoolDetails.IsDeleted = false;
            SchoolDetails.CreationTime = DateTime.Now.ToString();
            SchoolDetails.CreatorUserId = id;

            EFIndividualDetails.InsertSchoolDetails(SchoolDetails);

            return SchoolDetails;
        }

        public SchoolDetails UpdateSchoolDetails(SchoolDetails sd, int id)
        {
            SchoolDetails obj = GetSchoolDetailById(sd.Id);

            obj.BuildingOwnershipId = sd.BuildingOwnershipId;
            obj.Computers = sd.Computers;
            obj.Dongles = sd.Dongles;
            obj.InternetAccessId = sd.InternetAccessId;
            obj.LastModificationTime = DateTime.Now.ToString();
            obj.LastModifierUserId = id;
            obj.Monitors = sd.Monitors;
            obj.NumberOfStaffFemale = sd.NumberOfStaffFemale;
            obj.NumberOfStaffMale = sd.NumberOfStaffMale;
            obj.NumberOfStudentFemale = sd.NumberOfStudentFemale;
            obj.NumberOfStudentMale = sd.NumberOfStudentMale;
            obj.NumberOfTeacherFemale = sd.NumberOfTeacherFemale;
            obj.NumberOfTeacherMale = sd.NumberOfTeacherMale;
            obj.PowerAddressId = sd.RegisterationPaper;
            obj.RegisterationPaper = sd.RegisterationPaper;
            obj.RegisterNumber = sd.RegisterNumber;
            obj.Routers = sd.Routers;
            obj.SchoolAddress = sd.SchoolAddress;
            obj.SchoolLicense = sd.SchoolLicense;
            obj.SchoolName = sd.SchoolName;
            obj.SchoolTypeId = sd.SchoolTypeId;
            obj.SectionTypeId = sd.SectionTypeId;

            EFIndividualDetails.UpdateSchoolDetails(obj);

            return obj;
        }

        public MetaDataDetail CreateMetaDataDetail(MetaDataDetail MetaDataDetail, int id)
        {
            MetaDataDetail.IsDeleted = false;
            MetaDataDetail.CreationTime = DateTime.Now.ToString();
            MetaDataDetail.CreatorUserId = id;

            EFIndividualDetails.InsertMetaDataDetail(MetaDataDetail);

            return MetaDataDetail;
        }

        public string GetDocumentUrl(long ids)
        {
            return EFIndividualDetails.GetDocumentUrl(ids);
        }

        public DocumentDetails GetDocumentById(long id)
        {
            return EFIndividualDetails.GetDocumentById(id);
        }

        public List<MetaDataDetail> GetMetadataDetail(long metadataid)
        {
            return EFIndividualDetails.GetMetadataDetail(metadataid);
        }

        public MetaDataDetail GetMetadataById(long metadataid)
        {
            return EFIndividualDetails.GetMetadataById(metadataid);
        }

        public object GetSchoolList(string search)
        {
            return EFIndividualDetails.GetSchoolList(search);
        }

        public async Task<IndividualDetails> CreateTrialUser(CreateTrialUser dto)
        {
            IndividualDetails individualDetails = new IndividualDetails
            {
                StudentName = dto.StudentName,
                FatherNumber = dto.FatherName,
                Email = dto.Email,
                SexId = dto.Gender,
                DateOfBirth = dto.DateOfBirth,
                PlaceOfBirth = dto.PlaceOfBirth,
                ProvinceId = dto.Province,
                CityId = dto.City,
                Village = dto.Village,
                SchoolName = dto.SchoolName,
                GradeId = dto.GradeId,
                StudentTazkira = dto.TazkiraNo,
                Phone = dto.Phone,
                SoicalMediaLinked = dto.SoicalMediaLinked,
                SocialMediaAccount = dto.SocialMediaAccount,
                RefferedBy = dto.Reference,
                UserId = dto.UserId,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 1
            };
            await EFIndividualDetails.InsertAsync(individualDetails);
            return individualDetails;
        }
    }
}

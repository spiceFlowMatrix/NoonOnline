using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFIndividualDetails : IGenericRepository<IndividualDetails>
    {

        private readonly EFGenericRepository<IndividualDetails> _context;
        private readonly EFGenericRepository<PurchagePdf> _contextPurchagePdf;
        private readonly EFGenericRepository<PurchageUpload> _contextPurchageUpload;
        private readonly EFGenericRepository<DocumentDetails> _contextDocumentDetails;
        private readonly EFGenericRepository<SchoolDetails> _contextSchoolDetails;
        private readonly EFGenericRepository<MetaDataDetail> _contextMetaDataDetail;


        public EFIndividualDetails
        (
            EFGenericRepository<IndividualDetails> context,
            EFGenericRepository<PurchagePdf> contextPurchagePdf,
            EFGenericRepository<PurchageUpload> contextPurchageUpload,
            EFGenericRepository<DocumentDetails> contextDocumentDetails,
            EFGenericRepository<MetaDataDetail> contextMetaDataDetail,
            EFGenericRepository<SchoolDetails> contextSchoolDetails
        )
        {
            _context = context;
            _contextPurchageUpload = contextPurchageUpload;
            _contextPurchagePdf = contextPurchagePdf;
            _contextDocumentDetails = contextDocumentDetails;
            _contextMetaDataDetail = contextMetaDataDetail;
            _contextSchoolDetails = contextSchoolDetails;
        }

        public DocumentDetails GetDocumentById(long id)
        {
            DocumentDetails documentDetails = new DocumentDetails();
            documentDetails = _contextDocumentDetails.GetById(b => b.Id == id);
            return documentDetails;
        }

        public int Delete(IndividualDetails obj)
        {
            throw new NotImplementedException();
        }

        public List<IndividualDetails> GetAll()
        {
            return _context.GetAll();
        }

        public List<IndividualDetails> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public IndividualDetails GetById(Expression<Func<IndividualDetails, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(PurchagePdf obj)
        {
            return _contextPurchagePdf.Insert(obj);
        }


        public int Insert(PurchageUpload obj)
        {
            return _contextPurchageUpload.Insert(obj);
        }


        public int Insert(IndividualDetails obj)
        {
            return _context.Insert(obj);
        }

        public int InsertDocumentDetails(DocumentDetails obj)
        {
            return _contextDocumentDetails.Insert(obj);
        }

        public int InsertSchoolDetails(SchoolDetails obj)
        {
            return _contextSchoolDetails.Insert(obj);
        }

        public int UpdateSchoolDetails(SchoolDetails obj)
        {
            return _contextSchoolDetails.Update(obj);
        }

        public int InsertMetaDataDetail(MetaDataDetail obj)
        {
            return _contextMetaDataDetail.Insert(obj);
        }

        public List<MetaDataDetail> GetMetadataDetail(long metadataid)
        {
            return _contextMetaDataDetail.ListQuery(b => b.MetadataId == metadataid).ToList();
        }

        public MetaDataDetail GetMetadataById(long metadataid)
        {
            return _contextMetaDataDetail.GetById(b => b.MetadataId == metadataid);
        }

        public IQueryable<IndividualDetails> ListQuery(Expression<Func<IndividualDetails, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(IndividualDetails obj)
        {
            return _context.Update(obj);
        }

        public string GetDocumentUrl(long id)
        {
            return _contextDocumentDetails.ListQuery(b => b.Id == id).Select(b => b.DocumentUrl).SingleOrDefault();
        }

        public List<IndividualDetails> GetIndiviualDetails(string search)
        {
            List<IndividualDetails> individualDetails = new List<IndividualDetails>();

            if (string.IsNullOrEmpty(search))
            {
                individualDetails = _context.GetAll();
            }
            else
            {
                individualDetails = _context.ListQuery(
                                                        b => b.StudentCode.Any(k => b.StudentCode.ToLower().Contains(search.ToLower())) ||
                                                        b.StudentName.Any(k => b.StudentName.ToLower().Contains(search.ToLower()))
                                                       ).ToList();
            }
            return individualDetails;
        }


        public List<SchoolDetails> GetSchoolDetails(string search)
        {
            List<SchoolDetails> SchoolDetails = new List<SchoolDetails>();

            if (string.IsNullOrEmpty(search))
            {
                SchoolDetails = _contextSchoolDetails.GetAll();
            }
            else
            {
                SchoolDetails = _contextSchoolDetails.ListQuery(
                                                                 b => b.RegisterNumber.Any(k => b.RegisterNumber.ToLower().Contains(search.ToLower())) ||
                                                                 b.SchoolName.Any(k => b.SchoolName.ToLower().Contains(search.ToLower()))
                                                                ).ToList();
            }
            return SchoolDetails;
        }

        public IndividualDetails GetIndividualDetailById(long id)
        {
            IndividualDetails individualDetails = _context.ListQuery(
                                                             b => b.Id == id
                                                         ).SingleOrDefault();

            return individualDetails;
        }

        public SchoolDetails GetSchoolDetailById(long id)
        {
            SchoolDetails SchoolDetails = _contextSchoolDetails.ListQuery(
                                                            b => b.Id == id
                                                        ).SingleOrDefault();

            return SchoolDetails;
        }

        public object GetSchoolList(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                var userListwithp =  _contextSchoolDetails.GetAll().Select(s => new {
                    s.Id,
                    s.RegisterNumber,
                    s.SchoolName,
                    s.SchoolAddress
                }).ToList();
                return userListwithp;
            }
            else
            {
                var userListwithp = _contextSchoolDetails.ListQuery(b => b.IsDeleted != true).Select(s => new
                {
                    s.Id,
                    s.RegisterNumber,
                    s.SchoolName,
                    s.SchoolAddress
                }).ToList();

                userListwithp = userListwithp.Where(b => b.RegisterNumber.ToLower().Contains(search.ToLower()) ||
                    !string.IsNullOrEmpty(b.SchoolName) && b.SchoolName.ToLower().Contains(search.ToLower())).ToList();
                return userListwithp;
            }
        }
    }
}

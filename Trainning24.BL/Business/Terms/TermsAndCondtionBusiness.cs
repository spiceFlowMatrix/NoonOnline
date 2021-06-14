using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class TermsAndCondtionBusiness
    {
        private readonly EFTermsAndConditionRepo _EFTermsAndConditionRepo;
        public TermsAndCondtionBusiness(
            EFTermsAndConditionRepo EFTermsAndConditionRepo
        )
        {
            _EFTermsAndConditionRepo = EFTermsAndConditionRepo;
        }

        public int AddRecord(TermsAndConditions obj)
        {
            return _EFTermsAndConditionRepo.Insert(obj);
        }

        public int UpdateRecord(TermsAndConditions obj)
        {
            return _EFTermsAndConditionRepo.Update(obj);
        }

        public TermsAndConditions GetTerms()
        {
            return _EFTermsAndConditionRepo.GetAll().FirstOrDefault();
        }
    }
}

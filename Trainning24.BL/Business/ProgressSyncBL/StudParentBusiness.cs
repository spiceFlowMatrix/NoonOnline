using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class StudParentBusiness
    {
        private readonly EFStudParentRepository _eFStudParentRepository;

        public StudParentBusiness(
            EFStudParentRepository eFStudParentRepository
        )
        {
            _eFStudParentRepository = eFStudParentRepository;
        }

        public List<StudParent> GetStudentListByParentId(long parentId)
        {
            return _eFStudParentRepository.ListQuery(b => b.ParentId == parentId && b.DeletionTime == null).ToList();
        }
    }
}

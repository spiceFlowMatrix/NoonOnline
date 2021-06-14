using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class QuestionTypeBusiness
    {
        private readonly EFQuestionTypeRepository _EFQuestionTypeRepository;

        public QuestionTypeBusiness
        (
            EFQuestionTypeRepository EFQuestionTypeRepository
        )
        {
            _EFQuestionTypeRepository = EFQuestionTypeRepository;
        }

        public QuestionType Create(QuestionType QuestionType, string Id)
        {
            QuestionType newQuestionType = new QuestionType
            {
                Code = QuestionType.Code,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            newQuestionType.Id = _EFQuestionTypeRepository.Insert(newQuestionType);

            return newQuestionType;
        }

        public QuestionType Update(QuestionType QuestionType, string Id)
        {
            QuestionType newQuestionType = new QuestionType
            {
                Id = QuestionType.Id,
                Code = QuestionType.Code,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 0,
                IsDeleted = false,
                DeleterUserId = int.Parse(Id),
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionTypeRepository.Update(newQuestionType);

            return newQuestionType;
        }

        public QuestionType Delete(QuestionType obj, string Id)
        {
            _EFQuestionTypeRepository.Delete(obj, Id);
            return obj;
        }

        public QuestionType QuestionTypeExistance(QuestionType QuestionType)
        {
            QuestionType _QuestionType = new QuestionType
            {
                Code = QuestionType.Code
            };

            return _EFQuestionTypeRepository.QuestionTypeExist(_QuestionType);
        }

        public QuestionType QuestionTypeExistanceById(int id)
        {
            QuestionType _QuestionType = new QuestionType
            {
                Id = id
            };

            return _EFQuestionTypeRepository.QuestionTypeExistById(_QuestionType);
        }

        public List<QuestionType> QuestionTypeList(PaginationModel paginationModel)
        {
            return _EFQuestionTypeRepository.GetAllPageWise(paginationModel.pagenumber, paginationModel.perpagerecord);
        }

        public int TotalQuestionTypeCount()
        {
            return _EFQuestionTypeRepository.GetAll().Count();
        }
    }
}

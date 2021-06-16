using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Quiz;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class QuizQuestionBusiness
    {
        private readonly EFQuizQuestionRepository _EFQuizQuestionRepository;

        public QuizQuestionBusiness
        (
            EFQuizQuestionRepository EFQuizQuestionRepository
        )
        {
            _EFQuizQuestionRepository = EFQuizQuestionRepository;
        }

        public QuizQuestion QuizQuestionExistanceById(long id)
        {
            return _EFQuizQuestionRepository.QuizQuestionExistByQuizQuestionId(id);
        }

        public QuizQuestion Delete(QuizQuestion obj, string Id)
        {
            _EFQuizQuestionRepository.Delete(obj, Id);
            return obj;
        }


        public List<QuizQuestion> GetQuizQuestions(long id) {
            return _EFQuizQuestionRepository.ListQuery(b => b.QuizId == id).ToList();
        }
    }
}

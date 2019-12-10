using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFQuizRepository : IGenericRepository<Quiz>
    {
        private readonly EFGenericRepository<Quiz> _context;
        private readonly EFGenericRepository<QuizSummary> _contextQuizSummary;
        private readonly EFGenericRepository<QuestionFile> _contextQuestionFile;
        private readonly EFGenericRepository<AnswerFile> _contextAnswerFile;
        public EFQuizRepository
        (
            EFGenericRepository<Quiz> context,
            EFGenericRepository<QuizSummary> contextQuizSummary,
            EFGenericRepository<QuestionFile> contextQuestionFile,
            EFGenericRepository<AnswerFile> contextAnswerFile
        )
        {
            _context = context;
            _contextQuizSummary = contextQuizSummary;
            _contextQuestionFile = contextQuestionFile;
            _contextAnswerFile = contextAnswerFile;
        }

        public int InsertQuizSummary(QuizSummary obj)
        {
            return _contextQuizSummary.Insert(obj);
        }


        public QuizSummary GetQuizSummary(Expression<Func<QuizSummary, bool>> ex)
        {
            return _contextQuizSummary.GetById(ex);
        }


        public int UpdateQuizSummary(QuizSummary obj)
        {
            QuizSummary QuizSummary = _contextQuizSummary.GetById(b => b.Id == obj.Id);
            QuizSummary.Attempts = obj.Attempts;
            QuizSummary.QSummary = obj.QSummary;
            QuizSummary.LastModificationTime = DateTime.Now.ToString();
            QuizSummary.LastModifierUserId = 1;
            return _contextQuizSummary.Update(QuizSummary);
        }

        public int Insert(Quiz obj)
        {
            return _context.Insert(obj);
        }

        public int Update(Quiz obj)
        {
            Quiz Quiz = _context.GetById(b => b.Id == obj.Id);

            Quiz.Name = obj.Name;
            Quiz.Code = obj.Code;
            Quiz.TimeOut = obj.TimeOut;
            Quiz.NumQuestions = obj.NumQuestions;
            Quiz.PassMark = obj.PassMark;
            Quiz.LastModificationTime = DateTime.Now.ToString();
            Quiz.LastModifierUserId = 1;

            return _context.Update(Quiz);
        }

        public int Delete(Quiz obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public Quiz QuizExist(Quiz Quiz)
        {
            Quiz result = null;

            try
            {
                result = _context.ListQuery(b => b.Name == Quiz.Name && b.IsDeleted != true).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public Quiz QuizExistById(Quiz Quiz)
        {
            Quiz result = null;

            try
            {
                result = _context.ListQuery(b => b.Id == Quiz.Id && b.IsDeleted != true).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<Quiz> GetAll()
        {
            return _context.GetAll().Where(b => b.IsDeleted == false).ToList();
        }

        public List<Quiz> GetAllPageWise(int pagenumber, int perpagerecord, string search)
        {
            List<Quiz> quizList = _context.ListQuery(b => b.IsDeleted == false).OrderByDescending(b => b.CreationTime).ToList();



            if (pagenumber != 0 && perpagerecord != 0)
            {
                if (!string.IsNullOrEmpty(search))
                    quizList = quizList.Where(b => b.Name.Any(k => b.Id.ToString().Contains(search)
                                                                || b.Name.ToLower().Contains(search.ToLower())
                                                                || b.Code.ToLower().Contains(search.ToLower())
                                                                || b.NumQuestions.ToString().Contains(search)
                                                                || b.PassMark.ToString().Contains(search)
                                                                || b.TimeOut.ToString().Contains(search)
                                                                )).ToList();

                return quizList.Skip(perpagerecord * (pagenumber - 1)).
                        Take(perpagerecord).ToList();
            }

            return quizList;
        }

        public List<Quiz> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Quiz> ListQuery(Expression<Func<Quiz, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public IQueryable<QuestionFile> ListQuestionFile(Expression<Func<QuestionFile, bool>> where)
        {
            return _contextQuestionFile.ListQuery(where);
        }

        public IQueryable<AnswerFile> ListAnswerFile(Expression<Func<AnswerFile, bool>> where)
        {
            return _contextAnswerFile.ListQuery(where);
        }

        public Quiz GetById(Expression<Func<Quiz, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Delete(Quiz obj)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }
    }
}

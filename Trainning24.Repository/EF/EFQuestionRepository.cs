using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFQuestionRepository
    {
        private readonly EFGenericRepository<Question> _context;
        private static Training24Context _updateContext;
        private readonly EFGenericRepository<QuestionFile> _contextLessonFile;

        public EFQuestionRepository
        (
            EFGenericRepository<Question> context,
            Training24Context training24Context,
            EFGenericRepository<QuestionFile> eFGenericRepository
        )
        {
            _context = context;
            _updateContext = training24Context;
            _contextLessonFile = eFGenericRepository;
        }

        public int Insert(Question obj)
        {
            return _context.Insert(obj);
        }

        public int Update(Question obj)
        {
            Question Question = _updateContext.Question.SingleOrDefault(b => b.Id == obj.Id);

            Question.QuestionTypeId = obj.QuestionTypeId;
            Question.QuestionText = obj.QuestionText;
            Question.Explanation = obj.Explanation;
            Question.IsMultiAnswer = obj.IsMultiAnswer;
            Question.LastModificationTime = DateTime.Now.ToString();
            Question.LastModifierUserId = 1;

            return _updateContext.SaveChanges();
        }

        public int Delete(Question obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public Question QuestionExist(Question Question)
        {
            Question result = null;

            try
            {
                result = _context.ListQuery(b => b.QuestionText == Question.QuestionText).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public Question QuestionExistById(Question Question)
        {
            Question result = null;

            try
            {
                result = _context.ListQuery(b => b.Id == Question.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<Question> GetAll()
        {
            return _context.GetAll().Where(b => b.IsDeleted == false).ToList();
        }

        public List<Question> GetAllPageWise(int pagenumber, int perpagerecord,string search)
        {
            List<Question> questions = _updateContext.Question.Include("QuestionType").
                Where(b => b.IsDeleted == false).OrderByDescending(b => b.CreationTime).ToList();

            if (pagenumber != 0 && perpagerecord != 0)
            {
                if (!string.IsNullOrEmpty(search))
                    questions = questions.Where(b => b.QuestionText.Any(k => b.Id.ToString().Contains(search)
                                                                          || b.QuestionText.ToLower().Contains(search.ToLower())
                                                                          || b.Explanation.ToLower().Contains(search.ToLower())
                                                                          )).ToList();

                return  questions.Skip(perpagerecord * (pagenumber - 1)).
                        Take(perpagerecord).ToList();
            }
            return questions;
        }

        public List<Question> GetQuestionsByQuizId(long QuizId)
        {

            List<Question> allQuestions = _updateContext.QuizQuestion.Include("Question").Where(f => f.QuizId == QuizId && f.IsDeleted != true).Select(f => new Question()
            {
                Id = f.Question.Id,
                QuestionTypeId = f.Question.QuestionTypeId,
                QuestionText = f.Question.QuestionText,
                Explanation = f.Question.Explanation,
                IsMultiAnswer = f.Question.IsMultiAnswer,
                QuestionType = f.Question.QuestionType
            }).ToList();

            return allQuestions;
        }

        public List<Question> GetQuestionsByQuizId(long QuizId, DateTime? lastModifiedDate)
        {
            var context = _updateContext.QuizQuestion.Include("Question").Where(f => f.QuizId == QuizId);
            if (lastModifiedDate.HasValue == true)
            {
                context = context.Where(f => (f.LastModificationTime != null && DateTime.Parse(f.LastModificationTime) > lastModifiedDate) || (f.IsDeleted == true && DateTime.Parse(f.DeletionTime) > lastModifiedDate));
            }
            else
            {
                context = context.Where(f => f.IsDeleted != true);
            }

            List<Question> allQuestions = context.Select(f => new Question()
            {
                Id = f.Question.Id,
                QuestionTypeId = f.Question.QuestionTypeId,
                QuestionText = f.Question.QuestionText,
                Explanation = f.Question.Explanation,
                IsMultiAnswer = f.Question.IsMultiAnswer,
                QuestionType = f.Question.QuestionType,
                IsDeleted = f.Question.IsDeleted,
            }).ToList();

            return allQuestions;
        }


        public List<Question> GetQuestionsByQuizId1(long QuizId)
        {

            List<Question> allQuestions = _updateContext.QuizQuestion.Include("Question").Where(f => f.QuizId == QuizId && f.IsDeleted != true).Select(f => new Question()
            {
                Id = f.Question.Id,
                QuestionTypeId = f.Question.QuestionTypeId,
                QuestionText = f.Question.QuestionText,
                Explanation = f.Question.Explanation,
                IsMultiAnswer = f.Question.IsMultiAnswer,
                QuestionType = f.Question.QuestionType
            }).ToList();


           allQuestions = allQuestions.Distinct().OrderBy(s => Guid.NewGuid()).Take(10).ToList();

            return allQuestions;
        }


        public List<QuestionFile> GetLessionFile(long Id)
        {
            return _contextLessonFile.ListQuery(b => b.QuestionId == Id).ToList();
        }
    }
}

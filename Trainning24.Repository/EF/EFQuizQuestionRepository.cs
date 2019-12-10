using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFQuizQuestionRepository : IGenericRepository<QuizQuestion>
    {
        private readonly EFGenericRepository<QuizQuestion> _context;
        //private static Training24Context _updateContext;

        public EFQuizQuestionRepository
        (
            EFGenericRepository<QuizQuestion> context
            //Training24Context training24Context
        )
        {
            _context = context;
            //_updateContext = training24Context;
        }

        public int Update(QuizQuestion obj, string uId)
        {
            QuizQuestion QuizQuestion = _context.GetById(b => b.Id == obj.Id);

            QuizQuestion.Id = obj.Id;
            QuizQuestion.QuizId = obj.QuizId;
            QuizQuestion.QuestionId = obj.QuestionId;
            QuizQuestion.CreationTime = DateTime.Now.ToString();
            QuizQuestion.CreatorUserId = int.Parse(uId);
            QuizQuestion.LastModificationTime = DateTime.Now.ToString();
            QuizQuestion.LastModifierUserId = 1;
            QuizQuestion.IsDeleted = false;

            return _context.Update(QuizQuestion);
        }

        public int Insert(QuizQuestion obj)
        {
            return _context.Insert(obj);
        }

        public int Delete(QuizQuestion obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public void DeleteAllByQuizId(long QuizId, string Id)
        {
            try
            {
                var Quizzes = _context.ListQuery(f => f.QuizId == QuizId).ToList();
                Quizzes.ForEach(a => { a.IsDeleted = true; a.DeletionTime = DateTime.Now.ToString(); a.DeleterUserId = int.Parse(Id); });
                _context.UpdateList(Quizzes);
            }
            catch { }
        }

        public QuizQuestion QuizQuestionExist(QuizQuestion QuizQuestion)
        {
            QuizQuestion result = null;

            try
            {
                result = _context.ListQuery(b => b.QuizId == QuizQuestion.QuizId && b.QuestionId == QuizQuestion.QuestionId).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public QuizQuestion QuizQuestionExistById(long QuizId, long QuestionId)
        {
            try
            {
                return _context.ListQuery(b => b.QuizId == QuizId && b.QuestionId == QuestionId).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public QuizQuestion QuizQuestionExistByQuizQuestionId(long QuizId)
        {
            QuizQuestion result = null;
            try
            {
                result = _context.ListQuery(b => b.Id == QuizId).FirstOrDefault();
            }
            catch 
            {
            }
            return result;
        }

        public List<QuizQuestion> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<QuizQuestion> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public IQueryable<QuizQuestion> ListQuery(Expression<Func<QuizQuestion, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public QuizQuestion GetById(Expression<Func<QuizQuestion, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Update(QuizQuestion obj)
        {
            throw new NotImplementedException();
        }

        public int Delete(QuizQuestion obj)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }
    }
}

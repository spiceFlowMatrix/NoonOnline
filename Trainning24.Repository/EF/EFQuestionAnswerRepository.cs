using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFQuestionAnswerRepository
    {
        private readonly EFGenericRepository<QuestionAnswer> _context;
        private readonly EFGenericRepository<AnswerFile> _answerfileContext;

        public EFQuestionAnswerRepository
        (
            EFGenericRepository<QuestionAnswer> context, EFGenericRepository<AnswerFile> answerfileContext
        )
        {
            _context = context;
            _answerfileContext = answerfileContext;
        }

        public int Insert(QuestionAnswer obj)
        {
            return _context.Insert(obj);
        }

        public int Update(QuestionAnswer obj)
        {
            QuestionAnswer QuestionAnswer = _context.GetById(b => b.Id == obj.Id);

            QuestionAnswer.Answer = obj.Answer;
            QuestionAnswer.ExtraText = obj.ExtraText;
            QuestionAnswer.IsCorrect = obj.IsCorrect;
            QuestionAnswer.QuestionId = obj.QuestionId;
            QuestionAnswer.LastModificationTime = DateTime.Now.ToString();
            QuestionAnswer.LastModifierUserId = 1;
            QuestionAnswer.IsDeleted = false;

            return _context.Update(QuestionAnswer);
        }

        public int UpdateTest(QuestionAnswer obj)
        {
            return _context.Update(obj);
        }

        public int InsertAnswerFile(AnswerFile obj)
        {
            return _answerfileContext.Insert(obj);
        }

        public IQueryable<AnswerFile> AnswerFileListQuery(Expression<Func<AnswerFile, bool>> where)
        {
            return _answerfileContext.ListQuery(where);
        }

        public int DeleteAnswerFile(AnswerFile obj)
        {
            return _answerfileContext.DeleteRow(obj);
        }

        public int Delete(QuestionAnswer obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public void DeleteAllByQuestionId(long QuestionId, string Id)
        {
            try
            {
                var Ques = _context.ListQuery(f => f.QuestionId == QuestionId).ToList();
                Ques.ForEach(a => { a.IsDeleted = true; a.DeletionTime = DateTime.Now.ToString(); a.DeleterUserId = int.Parse(Id); });
                _context.UpdateList(Ques);
            }
            catch { }
        }

        public List<QuestionAnswer> GetAnswersByQuestionId(long QuestionId)
        {
            try
            {
                return _context.ListQuery(b => b.QuestionId == QuestionId && b.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public List<long> GetAnswersIDsByQuestionId(long QuestionId)
        {
            try
            {
                return _context.ListQuery(b => b.QuestionId == QuestionId && b.IsDeleted == false).Select(f => f.Id).ToList();
            }
            catch (Exception ex)
            {
            }

            return null;
        }

        public QuestionAnswer QuestionAnswerExistById(QuestionAnswer QuestionAnswer)
        {
            QuestionAnswer result = null;

            try
            {
                result = _context.ListQuery(b => b.Id == QuestionAnswer.Id && b.Answer == QuestionAnswer.Answer).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public QuestionAnswer QuestionAnswerExistByIdName(QuestionAnswer QuestionAnswer)
        {
            QuestionAnswer result = null;

            try
            {
                result = _context.ListQuery(b => b.QuestionId == QuestionAnswer.Id && b.Answer == QuestionAnswer.Answer).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<QuestionAnswer> GetAll()
        {
            return _context.GetAll().Where(b => b.IsDeleted == false).ToList();
        }

        public List<QuestionAnswer> GetAllPageWise(int pagenumber, int perpagerecord)
        {
            if (pagenumber != 0 && perpagerecord != 0)
            {
                return _context.ListQuery(b => b.IsDeleted == false).
                        OrderByDescending(b => b.CreationTime).Skip(perpagerecord * (pagenumber - 1)).
                        Take(perpagerecord).ToList();
            }
            return _context.GetAll().OrderBy(b => b.CreationTime).ToList();
        }
    }
}

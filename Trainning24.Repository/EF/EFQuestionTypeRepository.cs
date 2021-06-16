using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFQuestionTypeRepository
    {
        private readonly EFGenericRepository<QuestionType> _context;
        //private static Training24Context _updateContext;

        public EFQuestionTypeRepository
        (
            EFGenericRepository<QuestionType> context
            //Training24Context training24Context
        )
        {
            _context = context;
            //_updateContext = training24Context;
        }

        public int Insert(QuestionType obj)
        {
            return _context.Insert(obj);
        }

        public int Update(QuestionType obj)
        {
            QuestionType QuestionType = _context.GetById(b => b.Id == obj.Id);

            QuestionType.Code = obj.Code;
            QuestionType.LastModificationTime = DateTime.Now.ToString();
            QuestionType.LastModifierUserId = obj.LastModifierUserId;

            return _context.Update(QuestionType);
        }

        public int Delete(QuestionType obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public QuestionType QuestionTypeExist(QuestionType QuestionType)
        {
            QuestionType result = null;

            try
            {
                result = _context.ListQuery(b => b.Code == QuestionType.Code).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public QuestionType QuestionTypeExistById(QuestionType QuestionType)
        {
            QuestionType result = null;

            try
            {
                result = _context.ListQuery(b => b.Id == QuestionType.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
            }

            return result;
        }

        public List<QuestionType> GetAll()
        {
            return _context.GetAll().Where(b => b.IsDeleted == false).ToList();
        }

        public List<QuestionType> GetAllPageWise(int pagenumber, int perpagerecord)
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

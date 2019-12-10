using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Question;
using Trainning24.BL.ViewModels.QuestionAnswer;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class QuestionAnswerBusiness
    {
        private readonly EFQuestionAnswerRepository _EFQuestionAnswerRepository;
        private static Training24Context _training24Context;

        public QuestionAnswerBusiness
        (
            EFQuestionAnswerRepository EFQuestionAnswerRepository,
            Training24Context training24Context
        )
        {
            _EFQuestionAnswerRepository = EFQuestionAnswerRepository;
            _training24Context = training24Context;
        }

        public void Create(QuestionModel objModel, string uId)
        {
            try
            {
                DeleteAllByQuestionId(objModel.id, uId);
                foreach (var item in objModel.answers)
                {
                    var res = QuestionAnswerExistanceByIdName(objModel.id, item.answer);
                    if (res != null)
                    {
                        QuestionAnswer newQuestionAnswer = new QuestionAnswer
                        {
                            Id = res.Id,
                            Answer = item.answer,
                            ExtraText = item.extratext,
                            IsCorrect = item.iscorrect,
                            QuestionId = objModel.id,
                        };
                        Update(newQuestionAnswer, uId);
                    }
                    else
                    {
                        QuestionAnswer newQuestionAnswer = new QuestionAnswer
                        {
                            Answer = item.answer,
                            ExtraText = item.extratext,
                            IsCorrect = item.iscorrect,
                            QuestionId = objModel.id,
                            CreationTime = DateTime.Now.ToString(),
                            CreatorUserId = int.Parse(uId),
                            IsDeleted = false,
                            DeleterUserId = 0,
                            DeletionTime = DateTime.Now.ToString(),
                            LastModificationTime = DateTime.Now.ToString(),
                            LastModifierUserId = 0
                        };
                        _EFQuestionAnswerRepository.Insert(newQuestionAnswer);
                    }
                }
            }
            catch (Exception ex) { }
        }

        public QuestionAnswer CreateTest(QuestionAnswerModel QuestionAnswer, string Id)
        {
            QuestionAnswer newQuestionAnswer = new QuestionAnswer
            {
                Answer = QuestionAnswer.answer,
                ExtraText = QuestionAnswer.extratext,
                IsCorrect = QuestionAnswer.iscorrect,
                QuestionId = QuestionAnswer.questionid,                
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionAnswerRepository.Insert(newQuestionAnswer);

            return newQuestionAnswer;
        }


        public QuestionAnswer UpdateTest(QuestionAnswerModel QuestionAnswer, string Id)
        {
            QuestionAnswer newQuestionAnswer = new QuestionAnswer
            {
                Id = QuestionAnswer.id,
                Answer = QuestionAnswer.answer,
                ExtraText = QuestionAnswer.extratext,
                IsCorrect = QuestionAnswer.iscorrect,
                QuestionId = QuestionAnswer.questionid,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionAnswerRepository.UpdateTest(newQuestionAnswer);

            return newQuestionAnswer;
        }


        public int AnswerFileMapping(long answserId, long fileId)
        {
            AnswerFile newAnswerFile = new AnswerFile
            {
                FileId = fileId,
                AnswerId = answserId,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 1,
                IsDeleted = false
            };

            _training24Context.AnswerFile.Add(newAnswerFile);

            return _training24Context.SaveChanges();
        }


        public int AnswerFileRemoving(long answerId)
        {
            List<AnswerFile> answerFiles = _training24Context.AnswerFile.Where(b => b.AnswerId == answerId).ToList();

            if (answerFiles.Count != 0)
            {
                foreach (var file in answerFiles)
                {
                    _training24Context.AnswerFile.Remove(file);
                    _training24Context.SaveChanges();
                }
            }

            return _training24Context.SaveChanges();
        }

        public QuestionAnswer Update(QuestionAnswer QuestionAnswer, string Id)
        {
            QuestionAnswer newQuestionAnswer = new QuestionAnswer
            {
                Id = QuestionAnswer.Id,
                Answer = QuestionAnswer.Answer,
                ExtraText = QuestionAnswer.ExtraText,
                IsCorrect = QuestionAnswer.IsCorrect,
                QuestionId = QuestionAnswer.QuestionId,                
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 0,
                IsDeleted = false,
                DeleterUserId = int.Parse(Id),
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionAnswerRepository.Update(newQuestionAnswer);

            return newQuestionAnswer;
        }

        public QuestionAnswer Delete(QuestionAnswer obj, string Id)
        {
            _EFQuestionAnswerRepository.Delete(obj, Id);
            return obj;
        }

        public void DeleteAllByQuestionId(long Qid, string Id)
        {
            _EFQuestionAnswerRepository.DeleteAllByQuestionId(Qid, Id);
        }

        public List<QuestionAnswer> GetAnswersByQuestionId(long id)
        {
            return _EFQuestionAnswerRepository.GetAnswersByQuestionId(id);
        }

        public QuestionAnswer QuestionAnswerExistanceById(long id)
        {
            QuestionAnswer _QuestionAnswer = new QuestionAnswer
            {
                Id = id
            };

            return _EFQuestionAnswerRepository.QuestionAnswerExistById(_QuestionAnswer);
        }

        public QuestionAnswer QuestionAnswerExistanceByIdName(long id, string question)
        {
            QuestionAnswer _QuestionAnswer = new QuestionAnswer
            {
                Id = id,
                Answer = question
            };

            return _EFQuestionAnswerRepository.QuestionAnswerExistByIdName(_QuestionAnswer);
        }

        public List<QuestionAnswer> QuestionAnswerList(PaginationModel paginationModel)
        {
            return _EFQuestionAnswerRepository.GetAllPageWise(paginationModel.pagenumber, paginationModel.perpagerecord);
        }

        public int TotalQuestionAnswerCount()
        {
            return _EFQuestionAnswerRepository.GetAll().Count();
        }
    }
}

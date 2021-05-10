using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Question;
using Trainning24.BL.ViewModels.QuestionAnswer;
using Trainning24.BL.ViewModels.QuestionFile;
using Trainning24.BL.ViewModels.Quiz;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class QuizBusiness
    {
        private readonly EFQuizRepository _EFQuizRepository;
        private readonly EFQuestionRepository EFQuestionRepository;
        private readonly QuizQuestionBusiness QuizQuestionBusiness;
        private readonly QuestionAnswerBusiness QuestionAnswerBusiness;
        private readonly EFQuizQuestionRepository _EFQuizQuestionRepository;
        private readonly EFQuestionAnswerRepository EFQuestionAnswerRepository;
        private readonly FilesBusiness FilesBusiness;
        private readonly EFChapterRepository EFChapterRepository;
        private readonly ChapterQuizBusiness ChapterQuizBusiness;
        private readonly LogObjectBusiness _logObjectBusiness;

        public QuizBusiness
        (
            EFQuizRepository EFQuizRepository,
            EFQuizQuestionRepository EFQuizQuestionRepository,
            QuizQuestionBusiness QuizQuestionBusiness,
            EFQuestionRepository EFQuestionRepository,
            EFQuestionAnswerRepository EFQuestionAnswerRepository,
            QuestionAnswerBusiness QuestionAnswerBusiness,
            EFChapterRepository EFChapterRepository,
            FilesBusiness FilesBusiness,
            ChapterQuizBusiness ChapterQuizBusiness,
            LogObjectBusiness logObjectBusiness
        )
        {
            _EFQuizRepository = EFQuizRepository;
            _EFQuizQuestionRepository = EFQuizQuestionRepository;
            this.QuizQuestionBusiness = QuizQuestionBusiness;
            this.EFQuestionRepository = EFQuestionRepository;
            this.EFQuestionAnswerRepository = EFQuestionAnswerRepository;
            this.QuestionAnswerBusiness = QuestionAnswerBusiness;
            this.EFChapterRepository = EFChapterRepository;
            this.FilesBusiness = FilesBusiness;
            this.ChapterQuizBusiness = ChapterQuizBusiness;
            _logObjectBusiness = logObjectBusiness;
        }

        public Quiz Create(AddQuizModel Quiz, string Id)
        {
            Quiz newQuiz = new Quiz
            {
                Name = Quiz.Name,
                Code = Quiz.Code,
                NumQuestions = Quiz.NumQuestions,
                PassMark = Quiz.PassMark,
                TimeOut = Quiz.TimeOut,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuizRepository.Insert(newQuiz);
            _logObjectBusiness.AddLogsObject(13, newQuiz.Id, int.Parse(Id));
            try
            {
                ChapterQuiz chapterQuiz = new ChapterQuiz();
                chapterQuiz.QuizId = newQuiz.Id;
                chapterQuiz.ChapterId = Quiz.chapterid;
                chapterQuiz.ItemOrder = Quiz.itemorder;
                chapterQuiz.CreatorUserId = int.Parse(Id);
                chapterQuiz.IsDeleted = false;
                chapterQuiz.DeleterUserId = 0;
                chapterQuiz.DeletionTime = DateTime.Now.ToString();
                chapterQuiz.LastModificationTime = DateTime.Now.ToString();
                chapterQuiz.LastModifierUserId = 0;
                ChapterQuizBusiness.AddNewQuiz(chapterQuiz);
            }
            catch (Exception ex)
            {
                return null;
            }

            return newQuiz;
        }

        public QuizSummary Create(QuizSummary Quiz)
        {
            _EFQuizRepository.InsertQuizSummary(Quiz);
            return _EFQuizRepository.GetQuizSummary(b => b.Id == Quiz.Id);
        }

        public void AddQuizQuestions(QuizQuestionModel objModel, string Id)
        {
            _EFQuizQuestionRepository.DeleteAllByQuizId(objModel.QuizId, Id);
            foreach (var item in objModel.lstQuestionId)
            {
                QuizQuestion res = _EFQuizQuestionRepository.QuizQuestionExistById(objModel.QuizId, item);
                if (res != null)
                {
                    QuizQuestion newQuizQuestion = new QuizQuestion
                    {
                        Id = res.Id,
                        QuizId = objModel.QuizId,
                        QuestionId = item,
                        CreationTime = DateTime.Now.ToString(),
                        CreatorUserId = int.Parse(Id),
                    };
                    _EFQuizQuestionRepository.Update(newQuizQuestion, Id);
                }
                else
                {
                    QuizQuestion newQuizQuestion = new QuizQuestion
                    {
                        QuizId = objModel.QuizId,
                        QuestionId = item,
                        CreationTime = DateTime.Now.ToString(),
                        CreatorUserId = int.Parse(Id),
                        IsDeleted = false,
                        DeleterUserId = 0,
                        DeletionTime = DateTime.Now.ToString(),
                        LastModificationTime = DateTime.Now.ToString(),
                        LastModifierUserId = 0
                    };
                    _EFQuizQuestionRepository.Insert(newQuizQuestion);
                }
            }
        }

        public Quiz Update(Quiz Quiz, string Id)
        {
            Quiz newQuiz = new Quiz
            {
                Id = Quiz.Id,
                Name = Quiz.Name,
                Code = Quiz.Code,
                NumQuestions = Quiz.NumQuestions,
                PassMark = Quiz.PassMark,
                TimeOut = Quiz.TimeOut,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 0,
                IsDeleted = false,
                DeleterUserId = int.Parse(Id),
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuizRepository.Update(newQuiz);
            _logObjectBusiness.AddLogsObject(14, newQuiz.Id, int.Parse(Id));
            return newQuiz;
        }

        public Quiz Delete(Quiz obj, string Id)
        {
            _EFQuizRepository.Delete(obj, Id);
            _logObjectBusiness.AddLogsObject(15, obj.Id, int.Parse(Id));
            return obj;
        }

        public Quiz QuizExistance(Quiz Quiz)
        {
            Quiz _Quiz = new Quiz
            {
                Name = Quiz.Name
            };

            return _EFQuizRepository.QuizExist(_Quiz);
        }

        public Quiz QuizExistanceById(int id)
        {
            Quiz _Quiz = new Quiz
            {
                Id = id
            };

            return _EFQuizRepository.QuizExistById(_Quiz);
        }

        public List<Quiz> QuizList(PaginationModel paginationModel)
        {
            return _EFQuizRepository.GetAllPageWise(paginationModel.pagenumber, paginationModel.perpagerecord, paginationModel.search);
        }

        public int TotalQuizCount()
        {
            return _EFQuizRepository.GetAll().Count();
        }

        public List<QuestionPreviewModel> getQuestions(long id)
        {
            List<QuestionPreviewModel> questionPreviews = new List<QuestionPreviewModel>();
            List<Question> quizQuestions = EFQuestionRepository.GetQuestionsByQuizId(id);

            foreach (var quizQuestion in quizQuestions)
            {
                QuestionPreviewModel questionPreviewModel = new QuestionPreviewModel();
                questionPreviewModel.id = quizQuestion.Id;
                questionPreviewModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionPreviewModel.questiontext = quizQuestion.QuestionText;
                questionPreviewModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionPreviewModel.explanation = quizQuestion.Explanation;
                List<QuestionAnswer> questionAnswers = QuestionAnswerBusiness.GetAnswersByQuestionId(quizQuestion.Id);
                List<AnswerPreviewModel> answerPreviewModels = new List<AnswerPreviewModel>();

                foreach (var questionAnswer in questionAnswers)
                {
                    AnswerPreviewModel answerPreviewModel = new AnswerPreviewModel();
                    answerPreviewModel.Id = questionAnswer.Id;
                    answerPreviewModel.Answer = questionAnswer.Answer;
                    answerPreviewModel.ExtraText = questionAnswer.ExtraText;
                    //answerPreviewModel.IsCorrect = questionAnswer.IsCorrect;
                    answerPreviewModels.Add(answerPreviewModel);
                }

                questionPreviewModel.answers = answerPreviewModels;
                questionPreviews.Add(questionPreviewModel);
            }

            return questionPreviews;
        }

        public NewResponseQuestionModel getQuizDetail(long id)
        {
            Quiz quiz = _EFQuizRepository.GetById(b => b.Id == id && b.IsDeleted != true);
            if (quiz == null)
            {
                return null;
            }
            NewResponseQuestionModel quizDetail = new NewResponseQuestionModel();
            quizDetail.quizid = id;
            quizDetail.passmark = quiz.PassMark;
            List<Question> quizQuestions = EFQuestionRepository.GetQuestionsByQuizId(id);
            List<QuestionModel> questionList = new List<QuestionModel>();
            foreach (var quizQuestion in quizQuestions)
            {
                List<UpdateQuestionFileModel> questionfiles = new List<UpdateQuestionFileModel>();

                QuestionModel questionModel = new QuestionModel();
                questionModel.id = quizQuestion.Id;
                questionModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionModel.questiontext = quizQuestion.QuestionText;
                questionModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionModel.explanation = quizQuestion.Explanation;

                List<QuestionFile> questionFiles = _EFQuizRepository.ListQuestionFile(b => b.QuestionId == quizQuestion.Id && b.IsDeleted != true).ToList();

                List<UpdateQuestionFileModel> images = new List<UpdateQuestionFileModel>();
                foreach (var file in questionFiles)
                {
                    Files singleFile = FilesBusiness.getFilesById(file.FileId);
                    if (singleFile != null)
                    {
                        UpdateQuestionFileModel updateQuestionFileModel = new UpdateQuestionFileModel();
                        updateQuestionFileModel.fileid = singleFile.Id;
                        updateQuestionFileModel.Url = singleFile.Url;
                        images.Add(updateQuestionFileModel);
                    }
                }

                questionModel.images = images;

                List<QuestionAnswer> questionAnswers = QuestionAnswerBusiness.GetAnswersByQuestionId(quizQuestion.Id);
                List<QuestionAnswerModel> answerPreviewModels = new List<QuestionAnswerModel>();

                foreach (var questionAnswer in questionAnswers)
                {
                    QuestionAnswerModel answerPreviewModel = new QuestionAnswerModel();
                    answerPreviewModel.id = questionAnswer.Id;
                    answerPreviewModel.answer = questionAnswer.Answer;
                    answerPreviewModel.extratext = questionAnswer.ExtraText;
                    answerPreviewModel.iscorrect = questionAnswer.IsCorrect;

                    List<AnswerFile> answerFiles = _EFQuizRepository.ListAnswerFile(b => b.AnswerId == questionAnswer.Id && b.IsDeleted != true).ToList();

                    List<UpdateQuestionFileModel> ansfiles = new List<UpdateQuestionFileModel>();
                    foreach (var file in answerFiles)
                    {
                        Files singleFile = FilesBusiness.getFilesById(file.FileId);
                        if (singleFile != null)
                        {
                            UpdateQuestionFileModel updateQuestionFileModel = new UpdateQuestionFileModel();
                            updateQuestionFileModel.fileid = singleFile.Id;
                            updateQuestionFileModel.Url = singleFile.Url;
                            ansfiles.Add(updateQuestionFileModel);
                        }

                    }

                    answerPreviewModel.images = ansfiles;

                    answerPreviewModels.Add(answerPreviewModel);
                }

                questionModel.answers = answerPreviewModels;

                QuestionPreviewModel questionPreviewModel = new QuestionPreviewModel();
                questionPreviewModel.id = quizQuestion.Id;
                questionPreviewModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionPreviewModel.questiontext = quizQuestion.QuestionText;
                questionPreviewModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionPreviewModel.explanation = quizQuestion.Explanation;

                questionList.Add(questionModel);
            }

            quizDetail.questions = questionList;
            return quizDetail;
        }

        public NewResponseQuestionModel1 getQuizDetail1(long id, long studentid)
        {
            Quiz quiz = _EFQuizRepository.GetById(b => b.Id == id && b.IsDeleted != true);
            if (quiz == null)
            {
                return null;
            }
            NewResponseQuestionModel1 quizDetail = new NewResponseQuestionModel1();
            quizDetail.quizid = id;
            quizDetail.passmark = quiz.PassMark;
            QuizSummary quizSummary = _EFQuizRepository.GetQuizSummary(b => b.QuizId == quiz.Id && b.StudentId == studentid);

            if (quizSummary != null)
            {
                QuizSummaryResponse quizSummaryRes = new QuizSummaryResponse();
                quizSummaryRes.id = quizSummary.Id;
                quizSummaryRes.Attempts = quizSummary.Attempts;
                quizSummaryRes.QSummary = quizSummary.QSummary;
                quizDetail.QuizSummaryResponse = quizSummaryRes;
            }

            List<Question> quizQuestions = EFQuestionRepository.GetQuestionsByQuizId1(id);
            List<QuestionModel1> questionList = new List<QuestionModel1>();
            foreach (var quizQuestion in quizQuestions)
            {
                List<UpdateQuestionFileModel> questionfiles = new List<UpdateQuestionFileModel>();

                QuestionModel1 questionModel = new QuestionModel1();
                questionModel.id = quizQuestion.Id;
                questionModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionModel.questiontext = quizQuestion.QuestionText;
                questionModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionModel.explanation = quizQuestion.Explanation;

                List<QuestionFile> questionFiles = _EFQuizRepository.ListQuestionFile(b => b.QuestionId == quizQuestion.Id && b.IsDeleted != true).ToList();

                List<UpdateQuestionFileModel> images = new List<UpdateQuestionFileModel>();
                foreach (var file in questionFiles)
                {
                    Files singleFile = FilesBusiness.getFilesById(file.FileId);
                    UpdateQuestionFileModel updateQuestionFileModel = new UpdateQuestionFileModel();
                    updateQuestionFileModel.fileid = singleFile.Id;
                    updateQuestionFileModel.Url = singleFile.Url;
                    images.Add(updateQuestionFileModel);
                }

                questionModel.images = images;

                List<QuestionAnswer> questionAnswers = QuestionAnswerBusiness.GetAnswersByQuestionId(quizQuestion.Id);
                List<QuestionAnswerModel1> answerPreviewModels = new List<QuestionAnswerModel1>();

                foreach (var questionAnswer in questionAnswers)
                {
                    QuestionAnswerModel1 answerPreviewModel = new QuestionAnswerModel1();
                    answerPreviewModel.id = questionAnswer.Id;
                    answerPreviewModel.answer = questionAnswer.Answer;
                    answerPreviewModel.extratext = questionAnswer.ExtraText;
                    answerPreviewModel.iscorrect = questionAnswer.IsCorrect;

                    List<AnswerFile> answerFiles = _EFQuizRepository.ListAnswerFile(b => b.AnswerId == questionAnswer.Id && b.IsDeleted != true).ToList();

                    List<UpdateQuestionFileModel> ansfiles = new List<UpdateQuestionFileModel>();
                    foreach (var file in answerFiles)
                    {
                        Files singleFile = FilesBusiness.getFilesById(file.FileId);
                        UpdateQuestionFileModel updateQuestionFileModel = new UpdateQuestionFileModel();
                        updateQuestionFileModel.fileid = singleFile.Id;
                        updateQuestionFileModel.Url = singleFile.Url;
                        ansfiles.Add(updateQuestionFileModel);

                    }

                    answerPreviewModel.images = ansfiles;

                    answerPreviewModels.Add(answerPreviewModel);
                }

                questionModel.answers = answerPreviewModels;

                QuestionPreviewModel questionPreviewModel = new QuestionPreviewModel();
                questionPreviewModel.id = quizQuestion.Id;
                questionPreviewModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionPreviewModel.questiontext = quizQuestion.QuestionText;
                questionPreviewModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionPreviewModel.explanation = quizQuestion.Explanation;

                questionList.Add(questionModel);
            }

            quizDetail.questions = questionList;
            return quizDetail;
        }


        public NewResponseQuestionModel1 GetCompleteQuizDetail(long id, long studentid, DateTime? lastModifiedDate)
        {
            Quiz quiz = _EFQuizRepository.GetById(b => b.Id == id && b.IsDeleted != true);
            if (quiz == null)
            {
                return null;
            }
            NewResponseQuestionModel1 quizDetail = new NewResponseQuestionModel1();
            quizDetail.quizid = id;
            quizDetail.passmark = quiz.PassMark;
            QuizSummary quizSummary = _EFQuizRepository.GetQuizSummary(b => b.QuizId == quiz.Id && b.StudentId == studentid);

            if (quizSummary != null)
            {
                QuizSummaryResponse quizSummaryRes = new QuizSummaryResponse();
                quizSummaryRes.id = quizSummary.Id;
                quizSummaryRes.Attempts = quizSummary.Attempts;
                quizSummaryRes.QSummary = quizSummary.QSummary;
                quizDetail.QuizSummaryResponse = quizSummaryRes;
            }

            List<Question> quizQuestions = EFQuestionRepository.GetQuestionsByQuizId(id, lastModifiedDate);
            List<QuestionModel1> questionList = new List<QuestionModel1>();
            foreach (var quizQuestion in quizQuestions)
            {
                List<UpdateQuestionFileModel> questionfiles = new List<UpdateQuestionFileModel>();

                QuestionModel1 questionModel = new QuestionModel1();
                questionModel.id = quizQuestion.Id;
                questionModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionModel.questiontext = quizQuestion.QuestionText;
                questionModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionModel.explanation = quizQuestion.Explanation;

                if (lastModifiedDate != null)
                {
                    if (quizQuestion.IsDeleted == true)
                    {
                        questionModel.Status = QuestionStatus.Deleted;
                    }
                    else
                    {
                        questionModel.Status = QuestionStatus.Modified;
                    }
                }
                

                List<QuestionFile> questionFiles = _EFQuizRepository.ListQuestionFile(b => b.QuestionId == quizQuestion.Id && b.IsDeleted != true).ToList();

                List<UpdateQuestionFileModel> images = new List<UpdateQuestionFileModel>();
                foreach (var file in questionFiles)
                {
                    Files singleFile = FilesBusiness.getFilesById(file.FileId);
                    UpdateQuestionFileModel updateQuestionFileModel = new UpdateQuestionFileModel();
                    updateQuestionFileModel.fileid = singleFile.Id;
                    updateQuestionFileModel.Url = singleFile.Url;
                    images.Add(updateQuestionFileModel);
                }

                questionModel.images = images;

                List<QuestionAnswer> questionAnswers = QuestionAnswerBusiness.GetAnswersByQuestionId(quizQuestion.Id);
                List<QuestionAnswerModel1> answerPreviewModels = new List<QuestionAnswerModel1>();

                foreach (var questionAnswer in questionAnswers)
                {
                    QuestionAnswerModel1 answerPreviewModel = new QuestionAnswerModel1();
                    answerPreviewModel.id = questionAnswer.Id;
                    answerPreviewModel.answer = questionAnswer.Answer;
                    answerPreviewModel.extratext = questionAnswer.ExtraText;
                    answerPreviewModel.iscorrect = questionAnswer.IsCorrect;

                    List<AnswerFile> answerFiles = _EFQuizRepository.ListAnswerFile(b => b.AnswerId == questionAnswer.Id && b.IsDeleted != true).ToList();

                    List<UpdateQuestionFileModel> ansfiles = new List<UpdateQuestionFileModel>();
                    foreach (var file in answerFiles)
                    {
                        Files singleFile = FilesBusiness.getFilesById(file.FileId);
                        UpdateQuestionFileModel updateQuestionFileModel = new UpdateQuestionFileModel();
                        updateQuestionFileModel.fileid = singleFile.Id;
                        updateQuestionFileModel.Url = singleFile.Url;
                        ansfiles.Add(updateQuestionFileModel);

                    }

                    answerPreviewModel.images = ansfiles;

                    answerPreviewModels.Add(answerPreviewModel);
                }

                questionModel.answers = answerPreviewModels;

                QuestionPreviewModel questionPreviewModel = new QuestionPreviewModel();
                questionPreviewModel.id = quizQuestion.Id;
                questionPreviewModel.ismultianswer = quizQuestion.IsMultiAnswer;
                questionPreviewModel.questiontext = quizQuestion.QuestionText;
                questionPreviewModel.questiontypeid = quizQuestion.QuestionTypeId;
                questionPreviewModel.explanation = quizQuestion.Explanation;

                questionList.Add(questionModel);
            }

            quizDetail.questions = questionList;
            return quizDetail;
        }

        public QuizDTO GetQuizDataById(Quiz obj)
        {
            QuizDTO quiz = new QuizDTO();
            if (obj != null)
            {
                quiz.Id = obj.Id;
                quiz.Name = obj.Name;
            }
            return quiz;
        }

        public QuizDTO GetQuizDataById(long Id)
        {
            Quiz quiz = _EFQuizRepository.GetById(b => b.Id == Id && b.IsDeleted != true);
            QuizDTO quizDTO = new QuizDTO();
            if (quiz != null)
            {
                quizDTO.Id = quiz.Id;
                quizDTO.Name = quiz.Name;
            }
            return quizDTO;
        }
    }
}

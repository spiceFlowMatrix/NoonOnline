using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.LessonFile;
using Trainning24.BL.ViewModels.Question;
using Trainning24.BL.ViewModels.QuestionAnswer;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class QuestionBusiness
    {
        private readonly EFQuestionRepository _EFQuestionRepository;
        private static Training24Context _training24Context;
        private readonly QuestionAnswerBusiness _QuestionAnswerBusiness;
        private readonly QuizBusiness QuizBusiness;
        private readonly EFLessonRepository EFLessonRepository;
        private readonly FilesBusiness FilesBusiness;
        private readonly LogObjectBusiness _logObjectBusiness;
        public QuestionBusiness
        (
            EFQuestionRepository EFQuestionRepository,
            Training24Context training24Context,
            QuestionAnswerBusiness QuestionAnswerBusiness,
            EFLessonRepository EFLessonRepository,
            FilesBusiness FilesBusiness,
            QuizBusiness QuizBusiness,
            LogObjectBusiness logObjectBusiness
        )
        {
            _EFQuestionRepository = EFQuestionRepository;
            _training24Context = training24Context;
            _QuestionAnswerBusiness = QuestionAnswerBusiness;
            this.EFLessonRepository = EFLessonRepository;
            this.FilesBusiness = FilesBusiness;
            this.QuizBusiness = QuizBusiness;
            _logObjectBusiness = logObjectBusiness;
        }

        public Question CreateTest(QuestionModel Question, string Id)
        {
            Question newQuestion = new Question
            {
                QuestionTypeId = Question.questiontypeid,
                QuestionText = Question.questiontext,
                Explanation = Question.explanation,
                IsMultiAnswer = Question.ismultianswer,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionRepository.Insert(newQuestion);
            _logObjectBusiness.AddLogsObject(16, newQuestion.Id, int.Parse(Id));
            return newQuestion;
        }

        public Question UpdateTest(QuestionModel Question, string Id)
        {
            Question newQuestion = new Question
            {
                Id = Question.id,
                QuestionTypeId = Question.questiontypeid,
                QuestionText = Question.questiontext,
                Explanation = Question.explanation,
                IsMultiAnswer = Question.ismultianswer,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionRepository.Update(newQuestion);
            _logObjectBusiness.AddLogsObject(17, newQuestion.Id, int.Parse(Id));
            return newQuestion;
        }

        public Question Create(Question Question, string Id)
        {
            Question newQuestion = new Question
            {
                QuestionTypeId = Question.QuestionTypeId,
                QuestionText = Question.QuestionText,
                Explanation = Question.Explanation,
                IsMultiAnswer = Question.IsMultiAnswer,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionRepository.Insert(newQuestion);
            _logObjectBusiness.AddLogsObject(16, newQuestion.Id, int.Parse(Id));
            return newQuestion;
        }

        public int QuestionFileMapping(long questionId , long fileId)
        {

            QuestionFile newQuestionFile = new QuestionFile
            {
                FileId = fileId,
                QuestionId = questionId,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 1,
                IsDeleted = false
            };

            _training24Context.QuestionFile.Add(newQuestionFile);
            
            return _training24Context.SaveChanges();
        }

        public int QuestionQuizMapping(long questionId, long quizId) {
            QuizQuestion newQuizQuestion = new QuizQuestion
            {
                QuizId = quizId,
                QuestionId = questionId,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 1,
                IsDeleted = false
            };
            _training24Context.Add(newQuizQuestion);
            _training24Context.SaveChanges();
            return 0;
        }


        public QuestionModel Update(QuestionModel Question, string Id)
        {
            Question newQuestion = new Question
            {
                Id = Question.id,
                QuestionTypeId = Question.questiontypeid,
                QuestionText = Question.questiontext,
                Explanation = Question.explanation,
                IsMultiAnswer = Question.ismultianswer,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 0,
                IsDeleted = false,
                DeleterUserId = int.Parse(Id),
                DeletionTime = DateTime.Now.ToString(),
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            _EFQuestionRepository.Update(newQuestion);
            _logObjectBusiness.AddLogsObject(17, newQuestion.Id, int.Parse(Id));
            List<QuestionFile> questionFileList = _training24Context.QuestionFile.Where(b => b.QuestionId == newQuestion.Id && b.IsDeleted != true).ToList();

            if (questionFileList.Count > 0)
            {
                foreach (var files in questionFileList)
                {
                    _training24Context.QuestionFile.Remove(files);
                    _training24Context.SaveChanges();
                }
            }

            foreach (var questionFile in Question.images)
            {
                QuestionFile newQuestionFile = new QuestionFile
                {
                    FileId = questionFile.fileid,
                    QuestionId = newQuestion.Id,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = 0,
                    IsDeleted = false,
                    DeleterUserId = int.Parse(Id),
                    DeletionTime = DateTime.Now.ToString(),
                    LastModificationTime = DateTime.Now.ToString(),
                    LastModifierUserId = 0
                };

                _training24Context.QuestionFile.Add(newQuestionFile);
                _training24Context.SaveChanges();
            }

            return Question;
        }

        public Question Delete(Question obj, string Id)
        {
            _EFQuestionRepository.Delete(obj, Id);
            _logObjectBusiness.AddLogsObject(18, obj.Id, int.Parse(Id));
            return obj;
        }

        public Question QuestionExistance(Question Question)
        {
            Question _Question = new Question
            {
                QuestionText = Question.QuestionText
            };

            return _EFQuestionRepository.QuestionExist(_Question);
        }

        public Question QuestionExistanceById(int id)
        {
            Question _Question = new Question
            {
                Id = id
            };

            return _EFQuestionRepository.QuestionExistById(_Question);
        }

        public List<Question> QuestionList(PaginationModel paginationModel)
        {
            return _EFQuestionRepository.GetAllPageWise(paginationModel.pagenumber, paginationModel.perpagerecord, paginationModel.search);
        }

        public List<Question> GetQuestionsByQuizId(long QuizId)
        {
            return _EFQuestionRepository.GetQuestionsByQuizId(QuizId);
        }

        public int TotalQuestionCount()
        {
            return _EFQuestionRepository.GetAll().Count();
        }

        public List<ResponseLessonFileModel> GetLessionFilesByLessionId(long Id)
        {
            List<QuestionFile> lessonFiles = _EFQuestionRepository.GetLessionFile(Id);

            List<ResponseLessonFileModel> LessonFileList = new List<ResponseLessonFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();


            foreach (var lessonFile in lessonFiles)
            {
                ResponseLessonFileModel responseLessonFileModel = new ResponseLessonFileModel();
                Files newFiles = FilesBusiness.getFilesById(lessonFile.FileId);
                ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                var filetyped = FilesBusiness.FileType(newFiles);
                responseFilesModel.Id = newFiles.Id;
                responseFilesModel.name = newFiles.Name;
                responseFilesModel.url = newFiles.Url;
                responseFilesModel.filename = newFiles.FileName;
                responseFilesModel.filetypeid = newFiles.FileTypeId;
                responseFilesModel.description = newFiles.Description;
                responseFilesModel.filetypename = filetyped.Filetype;
                responseFilesModel.filesize = newFiles.FileSize;

                responseLessonFileModel.id = lessonFile.Id;
                responseLessonFileModel.files = responseFilesModel;
                LessonFileList.Add(responseLessonFileModel);
            }

            return LessonFileList;
        }

        public int DeleteQuizDetail(DeleteQuizDetailModel deleteQuizDetailModel,long id)
        {
            switch (deleteQuizDetailModel.recordtodelete)
            {
                case 1:
                    Quiz quiz = _training24Context.Quiz.Where(b => b.Id == deleteQuizDetailModel.quizid).SingleOrDefault();
                    quiz.IsDeleted = true;
                    quiz.DeleterUserId = int.Parse(id.ToString());
                    quiz.DeletionTime = DateTime.Now.ToString();
                    _training24Context.SaveChanges();
                    break;
                case 2:
                    QuizQuestion quizQuestion = _training24Context.QuizQuestion.Where(b => b.QuizId == deleteQuizDetailModel.quizid && b.QuestionId == deleteQuizDetailModel.questionid).SingleOrDefault();
                    quizQuestion.IsDeleted = true;
                    quizQuestion.DeleterUserId = int.Parse(id.ToString());
                    quizQuestion.DeletionTime = DateTime.Now.ToString();
                    _training24Context.SaveChanges();
                    break;
                case 3:
                    QuestionAnswer quizAnswers = _training24Context.QuestionAnswer.Where(b => b.QuestionId == deleteQuizDetailModel.questionid && b.Id == deleteQuizDetailModel.answerid).SingleOrDefault();
                    quizAnswers.IsDeleted = true;
                    quizAnswers.DeleterUserId = int.Parse(id.ToString());
                    quizAnswers.DeletionTime = DateTime.Now.ToString();
                    _training24Context.SaveChanges();
                    break;
                case 4:
                    QuestionFile questionFile = _training24Context.QuestionFile.Where(b => b.QuestionId == deleteQuizDetailModel.questionid && b.FileId == deleteQuizDetailModel.fileid).SingleOrDefault();
                    questionFile.IsDeleted = true;
                    questionFile.DeleterUserId = int.Parse(id.ToString());
                    questionFile.DeletionTime = DateTime.Now.ToString();
                    _training24Context.SaveChanges();
                    break;
                case 5:
                    AnswerFile answerFile = _training24Context.AnswerFile.Where(b => b.AnswerId == deleteQuizDetailModel.answerid && b.FileId == deleteQuizDetailModel.fileid).SingleOrDefault();
                    answerFile.IsDeleted = true;
                    answerFile.DeleterUserId = int.Parse(id.ToString());
                    answerFile.DeletionTime = DateTime.Now.ToString();
                    _training24Context.SaveChanges();
                    break;
            }
            return 1;
        }
    }
}

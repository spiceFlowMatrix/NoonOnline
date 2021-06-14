using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ChapterBusiness
    {
        private readonly EFChapterRepository EFChapterRepository;
        private readonly EFAssignmentRepository EFAssignmentRepository;
        private readonly LogObjectBusiness _logObjectBusiness;
        public ChapterBusiness(
            EFChapterRepository EFChapterRepository,
            EFAssignmentRepository EFAssignmentRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            this.EFChapterRepository = EFChapterRepository;
            this.EFAssignmentRepository = EFAssignmentRepository;
            _logObjectBusiness = logObjectBusiness;
        }

        public Chapter Create(AddChapterModel AddChapterModel, int id)
        {
            int i = EFChapterRepository.ListQuery(b => b.CourseId == AddChapterModel.courseid).Count() == 0 ? 1 : EFChapterRepository.ListQuery(b => b.CourseId == AddChapterModel.courseid).Select(b => b.ItemOrder).Max() + 1;
            Chapter Chapter = new Chapter
            {
                Name = AddChapterModel.name,
                Code = AddChapterModel.code,
                CourseId = AddChapterModel.courseid,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                QuizId = AddChapterModel.quizid,
                ItemOrder = i
            };

            EFChapterRepository.Insert(Chapter);
            _logObjectBusiness.AddLogsObject(7, Chapter.Id, id);
            return Chapter;
        }

        public Chapter Update(UpdateChapterModel UpdateChapterModel, int id)
        {
            Chapter Chapter = new Chapter
            {
                Id = UpdateChapterModel.id,
                Code = UpdateChapterModel.code,
                CourseId = UpdateChapterModel.courseid,
                Name = UpdateChapterModel.name,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id,
                QuizId = UpdateChapterModel.quizid,
                ItemOrder = UpdateChapterModel.itemorder
            };

            int i = EFChapterRepository.Update(Chapter);
            _logObjectBusiness.AddLogsObject(8, Chapter.Id, id);
            Chapter updatedChapter = EFChapterRepository.GetById(UpdateChapterModel.id);

            return updatedChapter;
        }

        public List<Chapter> ChapterList(PaginationModel paginationModel, out int total)
        {
            List<Chapter> chapterList = new List<Chapter>();
            chapterList = EFChapterRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = chapterList.Count();

                chapterList = chapterList.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    chapterList = chapterList.Where(b => b.Name.Any(k => b.Id.ToString().Contains(paginationModel.search)
                                                                      || b.Name.ToLower().Contains(paginationModel.search.ToLower())
                                                                      || b.Code.ToLower().Contains(paginationModel.search)
                                                                      )).ToList();

                return chapterList;
            }

            total = chapterList.Count();
            return chapterList;
        }

        public Chapter getChapterById(int id)
        {
            return EFChapterRepository.GetById(id);
        }


        public List<Chapter> GetChapterByCourseId(int id)
        {
            return EFChapterRepository.ListQuery(b => b.CourseId == id).ToList();
        }

        public Chapter Delete(int Id, int DeleterId)
        {
            Chapter Chapter = new Chapter();
            Chapter.Id = Id;
            Chapter.DeleterUserId = DeleterId;
            int i = EFChapterRepository.Delete(Chapter);
            _logObjectBusiness.AddLogsObject(9, Chapter.Id, DeleterId);
            Chapter deletedChapter = EFChapterRepository.GetById(Id);
            return deletedChapter;
        }


        public ResponseChapterModel getChapterByIdWithAssignments(int id)
        {
            Chapter newChapter = EFChapterRepository.GetById(id);
            ResponseChapterModel responseChapterModel = new ResponseChapterModel();
            responseChapterModel.Code = newChapter.Code;
            responseChapterModel.Courseid = newChapter.CourseId;
            responseChapterModel.Id = int.Parse(newChapter.Id.ToString());
            responseChapterModel.Name = newChapter.Name;
            responseChapterModel.quizid = newChapter.QuizId;
            responseChapterModel.assignmentDetails = getAssignmentDetailsByChapterId(newChapter.Id);
            return responseChapterModel;
        }

        public List<AssignmentDetailModel> getAssignmentDetailsByChapterId(long id)
        {
            List<Assignment> assignments = EFAssignmentRepository.ListQuery(b => b.ChapterId == id).ToList();
            List<AssignmentDetailModel> assignmentDetails = new List<AssignmentDetailModel>();
            foreach (var assignment in assignments)
            {
                AssignmentDetailModel assignmentDetail = new AssignmentDetailModel();
                assignmentDetail.id = assignment.Id;
                assignmentDetail.code = assignment.Code;
                assignmentDetail.description = assignment.Description;
                assignmentDetail.name = assignment.Name;
                assignmentDetails.Add(assignmentDetail);
            }
            return assignmentDetails;
        }

        public ChapterDTO GetChapterById(int Id)
        {
            Chapter chapter = getChapterById(Id);
            ChapterDTO chapterDTO = new ChapterDTO();
            if (chapter != null)
            {
                chapterDTO.Id = chapter.Id;
                chapterDTO.Name = chapter.Name;
                chapterDTO.courseid = chapter.CourseId;
            }
            return chapterDTO;
        }

        public string GetChapterNameById(int Id)
        {
            var name = EFChapterRepository.ListQuery(c => c.Id == Id).Select(c => c.Name).FirstOrDefault();
            return name;
        }
    }
}

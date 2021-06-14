using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.AssignmentSubmission;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class AssignmentSubmissionBusinees
    {
        private readonly EFAssignmentSubmissionRepository _eFAssignmentSubmission;
        private readonly FilesBusiness _filesBusiness;
        private readonly LessonBusiness _lessonBusiness;
        private readonly UsersBusiness _usersBusiness;
        private readonly EFChapterRepository _eFChapterRepository;
        private readonly EFAssignmentRepository _eFAssignmentRepository;
        public AssignmentSubmissionBusinees(EFAssignmentSubmissionRepository eFAssignmentSubmission,
            FilesBusiness filesBusiness,
            LessonBusiness lessonBusiness,
            UsersBusiness usersBusiness,
            EFChapterRepository eFChapterRepository,
            EFAssignmentRepository eFAssignmentRepository)
        {
            _eFAssignmentSubmission = eFAssignmentSubmission;
            _filesBusiness = filesBusiness;
            _lessonBusiness = lessonBusiness;
            _usersBusiness = usersBusiness;
            _eFChapterRepository = eFChapterRepository;
            _eFAssignmentRepository = eFAssignmentRepository;
        }
        public AssignmentSubmission Create(AssignmentSubmission dto)
        {
            AssignmentSubmission submission = new AssignmentSubmission
            {
                AssignmentId = dto.AssignmentId,
                UserId = dto.UserId,
                IsSubmission = dto.IsSubmission,
                IsApproved = dto.IsApproved,
                Remark = dto.Remark,
                Score = dto.Score,
                Comment = dto.Comment,
                TeacherId = dto.TeacherId,
                IsDeleted = false,
                CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
                CreatorUserId = (int)dto.UserId
            };
            _eFAssignmentSubmission.Insert(submission);
            return submission;
        }

        public List<SubmissionFileDTO> CreateSubmissionFile(List<AssignmentSubmissionFile> submissionFiles)
        {
            submissionFiles = _eFAssignmentSubmission.InsertSubmissionFile(submissionFiles);
            List<SubmissionFileDTO> AssignmentFileList = new List<SubmissionFileDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var submissionFile in submissionFiles)
            {
                SubmissionFileDTO responseAssignmentFileModel = new SubmissionFileDTO();
                Files newFiles = _filesBusiness.getFilesById(submissionFile.FileId);
                ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                var filetyped = _filesBusiness.FileType(newFiles);
                responseFilesModel.Id = newFiles.Id;
                responseFilesModel.name = newFiles.Name;
                responseFilesModel.filename = newFiles.FileName;
                responseFilesModel.filetypeid = newFiles.FileTypeId;
                responseFilesModel.description = newFiles.Description;
                responseFilesModel.filetypename = filetyped.Filetype;
                responseFilesModel.filesize = newFiles.FileSize;
                responseAssignmentFileModel.id = submissionFile.Id;
                responseAssignmentFileModel.files = responseFilesModel;
                AssignmentFileList.Add(responseAssignmentFileModel);
            }
            return AssignmentFileList;
        }

        public List<AssignmentSubmission> GetAssignmentDetails(PaginationModel paginationModel, int assignmentid, int studentid)
        {
            List<AssignmentSubmission> getRecords = new List<AssignmentSubmission>();
            getRecords = _eFAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid && p.UserId == studentid).ToList();
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                getRecords = getRecords.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return getRecords;
            }
            return getRecords;
        }

        public List<SubmissionUserDTO> GetStudentDetails(int assignmentid)
        {
            List<SubmissionUserDTO> submissionUserDTO = new List<SubmissionUserDTO>();
            List<long> getRecords = new List<long>();
            getRecords = _eFAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid).Select(s => s.UserId).Distinct().ToList();
            if (getRecords.Count > 0)
            {
                foreach (var id in getRecords)
                {
                    var user = _usersBusiness.GetUserbyId(id);
                    SubmissionUserDTO submissionUser = new SubmissionUserDTO();
                    submissionUser.id = user.Id;
                    submissionUser.name = user.FullName ?? user.Username;
                    submissionUser.submissions = _eFAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid && p.UserId == id && p.IsSubmission).Count();
                    var getAllSubmissionByStudent = _eFAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid && p.UserId == id).ToList();
                    if (getAllSubmissionByStudent.Count > 0)
                    {
                        submissionUser.status = 3;
                        if (getAllSubmissionByStudent.Any(p => p.IsApproved))
                        {
                            submissionUser.status = 0;
                        }
                        else if (getAllSubmissionByStudent.Any(p => !string.IsNullOrEmpty(p.Remark) && p.Score != 0 && !p.IsApproved))
                        {
                            submissionUser.status = 1;
                        }
                        else if (getAllSubmissionByStudent.Any(p => p.Comment == null && p.Remark == null && p.Score == 0 && !p.IsApproved && p.IsSubmission))
                        {
                            submissionUser.status = 2;
                        }
                    }
                    submissionUserDTO.Add(submissionUser);
                }
            }
            return submissionUserDTO;
        }

        public List<SubmissionFileDTO> GetSubmissionFilesById(long Id, string Certificate)
        {
            List<AssignmentSubmissionFile> submissionFiles = _eFAssignmentSubmission.GetAssignmentFile(Id);
            List<SubmissionFileDTO> submissionFilesDTO = new List<SubmissionFileDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            if (submissionFiles.Count > 0)
            {
                foreach (var file in submissionFiles)
                {
                    Files newFiles = _filesBusiness.getFilesById(file.FileId);
                    if (newFiles != null)
                    {
                        SubmissionFileDTO submissionFile = new SubmissionFileDTO();
                        ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                        var filetyped = _filesBusiness.FileType(newFiles);
                        responseFilesModel.Id = newFiles.Id;
                        responseFilesModel.name = newFiles.Name;
                        if (!string.IsNullOrEmpty(newFiles.Url))
                            responseFilesModel.url = _lessonBusiness.geturl(newFiles.Url, Certificate);
                        //responseFilesModel.url = newFiles.Url;
                        responseFilesModel.filename = newFiles.FileName;
                        responseFilesModel.filetypeid = newFiles.FileTypeId;
                        responseFilesModel.description = newFiles.Description;
                        responseFilesModel.filetypename = filetyped.Filetype;
                        responseFilesModel.filesize = newFiles.FileSize;
                        submissionFile.id = file.Id;
                        submissionFile.files = responseFilesModel;
                        submissionFilesDTO.Add(submissionFile);
                    }
                }
            }
            return submissionFilesDTO;
        }

        public List<AssignmentSubmission> GetAssignmentProgressByCourseId(List<int> studentid, int courseid)
        {
            if (courseid != 0)
            {
                List<AssignmentSubmission> assignmentProgresses = new List<AssignmentSubmission>();
                List<long> chapterAssignment = new List<long>();
                var chapterIds = _eFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    chapterAssignment = _eFAssignmentRepository.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.IsDeleted != true).Select(s => s.Id).ToList();
                    if (chapterAssignment.Count > 0)
                    {
                        foreach (var assid in chapterAssignment)
                        {
                            var assignmentprogress = _eFAssignmentSubmission.ListQuery(b => studentid.Contains((int)b.UserId) && b.AssignmentId == assid && !string.IsNullOrEmpty(b.Remark) && b.IsDeleted != true).ToList();
                            if (assignmentprogress.Count > 0)
                            {
                                assignmentProgresses.AddRange(assignmentprogress);
                            }
                        }
                    }
                }
                return assignmentProgresses;
            }
            else
            {
                return _eFAssignmentSubmission.ListQuery(b => studentid.Contains((int)b.UserId) && !string.IsNullOrEmpty(b.Remark) && b.IsDeleted != true).ToList();
            }
        }

        public string CalculateGAP(double score)
        {
            double i = score;

            switch (i)
            {
                case double n when (n < 65):
                    return "F";
                case double n when (n < 67):
                    return "D";
                case double n when (n < 70):
                    return "D+";
                case double n when (n < 73):
                    return "C-";
                case double n when (n < 77):
                    return "C";
                case double n when (n < 80):
                    return "C+";
                case double n when (n < 80):
                    return "C+";
                case double n when (n < 83):
                    return "B-";
                case double n when (n < 87):
                    return "B";
                case double n when (n < 90):
                    return "B+";
                case double n when (n < 93):
                    return "A-";
                case double n when (n < 97):
                    return "A";
                default:
                    return "A+";
            }
        }
    }
}

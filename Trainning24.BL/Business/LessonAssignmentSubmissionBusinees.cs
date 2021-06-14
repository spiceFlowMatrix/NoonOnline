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
    public class LessonAssignmentSubmissionBusinees
    {
        private readonly EFLessonAssignmentSubmissionRepository _eFLessonAssignmentSubmission;
        private readonly FilesBusiness _filesBusiness;
        private readonly LessonBusiness _lessonBusiness;
        private readonly UsersBusiness _usersBusiness;
        public LessonAssignmentSubmissionBusinees(EFLessonAssignmentSubmissionRepository eFLessonAssignmentSubmission,
            FilesBusiness filesBusiness,
            LessonBusiness lessonBusiness,
            UsersBusiness usersBusiness)
        {
            _eFLessonAssignmentSubmission = eFLessonAssignmentSubmission;
            _filesBusiness = filesBusiness;
            _lessonBusiness = lessonBusiness;
            _usersBusiness = usersBusiness;
        }
        public LessonAssignmentSubmission Create(LessonAssignmentSubmission dto)
        {
            LessonAssignmentSubmission submission = new LessonAssignmentSubmission
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
            _eFLessonAssignmentSubmission.Insert(submission);
            return submission;
        }

        public List<SubmissionFileDTO> CreateSubmissionFile(List<LessonAssignmentSubmissionFile> submissionFiles)
        {
            submissionFiles = _eFLessonAssignmentSubmission.InsertSubmissionFile(submissionFiles);
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

        public List<LessonAssignmentSubmission> GetAssignmentDetails(PaginationModel paginationModel, int assignmentid, int studentid)
        {
            List<LessonAssignmentSubmission> getRecords = new List<LessonAssignmentSubmission>();
            getRecords = _eFLessonAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid && p.UserId == studentid).ToList();
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
            getRecords = _eFLessonAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid).Select(s => s.UserId).Distinct().ToList();
            if (getRecords.Count > 0)
            {
                foreach (var id in getRecords)
                {
                    var user = _usersBusiness.GetUserbyId(id);
                    SubmissionUserDTO submissionUser = new SubmissionUserDTO();
                    submissionUser.id = user.Id;
                    submissionUser.name = user.FullName ?? user.Username;
                    submissionUser.submissions = _eFLessonAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid && p.UserId == id && p.IsSubmission).Count();
                    var getAllSubmissionByStudent = _eFLessonAssignmentSubmission.ListQuery(p => p.AssignmentId == assignmentid && p.UserId == id).ToList();
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
            List<LessonAssignmentSubmissionFile> submissionFiles = _eFLessonAssignmentSubmission.GetAssignmentFile(Id);
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
    }
}

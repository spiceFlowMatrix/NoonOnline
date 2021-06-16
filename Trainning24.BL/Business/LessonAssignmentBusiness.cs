using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.LessonAssignment;
using Trainning24.BL.ViewModels.LessonAssignmentFile;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class LessonAssignmentBusiness
    {
        private readonly EFLessonAssignmentRepository EFLessonAssignmentRepository;
        private readonly FilesBusiness FilesBusiness;
        private readonly LessonBusiness LessonBusiness;
        public LessonAssignmentBusiness(
            EFLessonAssignmentRepository EFLessonAssignmentRepository,
            FilesBusiness FilesBusiness,
            LessonBusiness LessonBusiness
        )
        {
            this.EFLessonAssignmentRepository = EFLessonAssignmentRepository;
            this.FilesBusiness = FilesBusiness;
            this.LessonBusiness = LessonBusiness;
        }
        public LessonAssignment Create(AddLessionAssignmentDTO dto, int id)
        {
            LessonAssignment Assignment = null;
            if (!string.IsNullOrEmpty(dto.name))
            {
                Assignment = new LessonAssignment
                {
                    Name = dto.name,
                    Description = dto.description,
                    Code = dto.code,
                    LessonId = dto.lessonid,
                    IsDeleted = false,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = id
                };
                EFLessonAssignmentRepository.Insert(Assignment);
                return Assignment;
            }
            return Assignment;
        }

        public LessonAssignment Update(AddLessionAssignmentDTO dto, int id)
        {
            LessonAssignment Assignment = new LessonAssignment
            {
                Id = dto.id,
                Name = dto.name,
                Description = dto.description,
                Code = dto.code,
                LessonId = dto.lessonid,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id
            };
            EFLessonAssignmentRepository.Update(Assignment);
            return Assignment;
        }

        public List<LessonAssignment> AssignmentList()
        {
            return EFLessonAssignmentRepository.GetAll();
        }

        public List<ResponseLessonAssignmentFileDTO> CreateAssignmentFile(List<LessonAssignmentFile> AssignmentFiles)
        {
            AssignmentFiles = EFLessonAssignmentRepository.InsertLessionFile(AssignmentFiles);
            List<ResponseLessonAssignmentFileDTO> AssignmentFileList = new List<ResponseLessonAssignmentFileDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseLessonAssignmentFileDTO responseAssignmentFileModel = new ResponseLessonAssignmentFileDTO();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                if (newFiles != null)
                {
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseAssignmentFileModel.id = AssignmentFile.Id;
                    responseAssignmentFileModel.files = responseFilesModel;
                    AssignmentFileList.Add(responseAssignmentFileModel);
                }
            }
            return AssignmentFileList;
        }

        public List<ResponseLessonAssignmentFileDTO> GetAssignmentFilesByAssignmentId(long Id)
        {
            List<LessonAssignmentFile> AssignmentFiles = EFLessonAssignmentRepository.GetAssignmentFile(Id);
            List<ResponseLessonAssignmentFileDTO> AssignmentFileList = new List<ResponseLessonAssignmentFileDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseLessonAssignmentFileDTO responseAssignmentFileModel = new ResponseLessonAssignmentFileDTO();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                if (newFiles != null)
                {
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseAssignmentFileModel.id = AssignmentFile.Id;
                    responseAssignmentFileModel.files = responseFilesModel;
                    AssignmentFileList.Add(responseAssignmentFileModel);
                }
            }
            return AssignmentFileList;
        }

        public List<ResponseLessonAssignmentFileDTO> UpdateAssignmentFile(List<LessonAssignmentFile> AssignmentFiles)
        {
            AssignmentFiles = EFLessonAssignmentRepository.UpdateAssignmentFile(AssignmentFiles);
            List<ResponseLessonAssignmentFileDTO> AssignmentFileList = new List<ResponseLessonAssignmentFileDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseLessonAssignmentFileDTO responseAssignmentFileModel = new ResponseLessonAssignmentFileDTO();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                if (newFiles != null)
                {
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseAssignmentFileModel.id = AssignmentFile.Id;
                    responseAssignmentFileModel.files = responseFilesModel;
                    AssignmentFileList.Add(responseAssignmentFileModel);
                }
            }
            return AssignmentFileList;
        }

        public List<ResponseLessonAssignmentFileDTO> DeleteAssignmentFile(long id)
        {
            List<LessonAssignmentFile> assignmentFileList = new List<LessonAssignmentFile>();
            assignmentFileList = EFLessonAssignmentRepository.DeleteAssignmentFile(id);
            List<ResponseLessonAssignmentFileDTO> AssignmentFileList = new List<ResponseLessonAssignmentFileDTO>();
            return AssignmentFileList;
        }

        public LessonAssignment GetAssignmentByLesson(long lessonid)
        {
            return EFLessonAssignmentRepository.GetById(b => b.LessonId == lessonid && b.IsDeleted != true);
        }

        public ResponseLessionAssignmentDTO GetAssignmentByLesson(long lessonid, string Certificate)
        {
            var assignment = EFLessonAssignmentRepository.GetById(b => b.LessonId == lessonid && b.IsDeleted != true);
            ResponseLessionAssignmentDTO responseLessionAssignmentDTO = new ResponseLessionAssignmentDTO();
            if (assignment != null)
            {
                responseLessionAssignmentDTO = new ResponseLessionAssignmentDTO
                {
                    id = Convert.ToInt32(assignment.Id),
                    name = assignment.Name,
                    code = assignment.Code,
                    description = assignment.Description,
                    assignmentfiles = GetAssignmentFilesByAssignmentId(assignment.Id, Certificate)
                };
                return responseLessionAssignmentDTO;
            }
            else
            {
                return null;
            }
        }

        public List<ResponseLessonAssignmentFileDTO> GetAssignmentFilesByAssignmentId(long Id, string Certificate)
        {
            List<LessonAssignmentFile> AssignmentFiles = EFLessonAssignmentRepository.GetAssignmentFile(Id);
            List<ResponseLessonAssignmentFileDTO> AssignmentFileList = new List<ResponseLessonAssignmentFileDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseLessonAssignmentFileDTO responseAssignmentFileModel = new ResponseLessonAssignmentFileDTO();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                if (newFiles != null)
                {
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.url = LessonBusiness.geturl(newFiles.Url, Certificate);
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseAssignmentFileModel.id = AssignmentFile.Id;
                    responseAssignmentFileModel.files = responseFilesModel;
                    AssignmentFileList.Add(responseAssignmentFileModel);
                }
            }
            return AssignmentFileList;
        }

        public LessonAssignmentFile DeleteAssignmentFileSingle(long id)
        {
            return EFLessonAssignmentRepository.DeleteAssignmentFileSingle(id);
        }
    }
}

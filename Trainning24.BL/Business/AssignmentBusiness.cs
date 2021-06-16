using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Assignment;
using Trainning24.BL.ViewModels.AssignmentFile;
using Trainning24.BL.ViewModels.AssignmentStudent;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class AssignmentBusiness
    {
        private readonly EFAssignmentRepository EFAssignmentRepository;
        private readonly EFAssignmentStudent EFAssignmentStudent;
        private readonly EFStudentCourseRepository EFStudentCourseRepository;
        private readonly EFCourseRepository EFCourseRepository;
        private readonly EFChapterRepository EFChapterRepository;
        private readonly FilesBusiness FilesBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly UsersBusiness usersBusiness;

        public AssignmentBusiness(
            EFAssignmentRepository EFAssignmentRepository,
            EFAssignmentStudent EFAssignmentStudent,
            EFStudentCourseRepository EFStudentCourseRepository,
            EFCourseRepository EFCourseRepository,
            EFChapterRepository EFChapterRepository,
            FilesBusiness FilesBusiness,
            LessonBusiness LessonBusiness,
            UsersBusiness usersBusiness
        )
        {
            this.EFAssignmentRepository = EFAssignmentRepository;
            this.EFAssignmentStudent = EFAssignmentStudent;
            this.EFStudentCourseRepository = EFStudentCourseRepository;
            this.EFCourseRepository = EFCourseRepository;
            this.EFChapterRepository = EFChapterRepository;
            this.FilesBusiness = FilesBusiness;
            this.LessonBusiness = LessonBusiness;
            this.usersBusiness = usersBusiness;
        }

        public Assignment Create(AddAssignmentModel AddAssignmentModel, int id)
        {
            Assignment Assignment = new Assignment
            {
                Name = AddAssignmentModel.name,
                Description = AddAssignmentModel.description,
                Code = AddAssignmentModel.code,
                ChapterId = AddAssignmentModel.chapterid,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFAssignmentRepository.Insert(Assignment);

            return Assignment;
        }

        public Assignment Update(AddAssignmentModel dto, int id)
        {
            Assignment Assignment = new Assignment
            {
                Id = dto.id,
                Name = dto.name,
                Description = dto.description,
                Code = dto.code,
                ChapterId = dto.chapterid,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id
            };
            EFAssignmentRepository.Update(Assignment);
            return Assignment;
        }

        public List<Assignment> AssignmentList()
        {
            return EFAssignmentRepository.GetAll();
        }

        public Assignment GetAssignmentById(int id)
        {
            return EFAssignmentRepository.GetById(id);
        }

        public Assignment Delete(int Id, int DeleterId)
        {
            Assignment Assignment = new Assignment
            {
                Id = Id,
                DeleterUserId = DeleterId
            };
            int i = EFAssignmentRepository.Delete(Assignment);
            return Assignment;
        }

        public List<Assignment> AssignmentList(PaginationModel paginationModel, out int total)
        {
            List<Assignment> AssignmentList = new List<Assignment>();
            AssignmentList = EFAssignmentRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = AssignmentList.Count();

                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    AssignmentList = AssignmentList.Where(b => b.Description != null && b.Code.ToLower().Contains(paginationModel.search.ToLower()) ||
                                b.Name != null && b.Name.ToLower().Contains(paginationModel.search.ToLower()) ||
                                b.Code != null && b.Code.ToLower().Contains(paginationModel.search.ToLower()) ||
                                b.Id.ToString().ToLower().Contains(paginationModel.search.ToLower())).ToList();

                    total = AssignmentList.Count();

                    AssignmentList = AssignmentList.OrderByDescending(b => b.CreationTime).
                                    Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                                    Take(paginationModel.perpagerecord).
                                    ToList();


                    return AssignmentList;
                }

                AssignmentList = AssignmentList.OrderByDescending(b => b.CreationTime).
                                Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                                Take(paginationModel.perpagerecord).
                                ToList();


                return AssignmentList;
            }

            total = AssignmentList.Count();
            return EFAssignmentRepository.GetAll();
        }

        public List<ResponseAssignmentFileModel> CreateAssignmentFile(List<AssignmentFile> AssignmentFiles, string Certificate)
        {
            AssignmentFiles = EFAssignmentRepository.InsertAssignmentFile(AssignmentFiles);
            List<ResponseAssignmentFileModel> AssignmentFileList = new List<ResponseAssignmentFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseAssignmentFileModel responseAssignmentFileModel = new ResponseAssignmentFileModel();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                var filetyped = FilesBusiness.FileType(newFiles);
                responseFilesModel.Id = newFiles.Id;
                responseFilesModel.name = newFiles.Name;
                if (!string.IsNullOrEmpty(newFiles.Url))
                    responseFilesModel.url = LessonBusiness.geturl(newFiles.Url, Certificate);
                //responseFilesModel.url = newFiles.Url;
                responseFilesModel.filename = newFiles.FileName;
                responseFilesModel.filetypeid = newFiles.FileTypeId;
                responseFilesModel.description = newFiles.Description;
                responseFilesModel.filetypename = filetyped.Filetype;
                responseFilesModel.filesize = newFiles.FileSize;
                responseAssignmentFileModel.id = AssignmentFile.Id;
                responseAssignmentFileModel.files = responseFilesModel;
                AssignmentFileList.Add(responseAssignmentFileModel);
            }
            return AssignmentFileList;
        }

        public List<ResponseAssignmentFileModel> GetAssignmentFilesByAssignmentId(long Id, string Certificate)
        {
            List<AssignmentFile> AssignmentFiles = EFAssignmentRepository.GetAssignmentFile(Id);
            List<ResponseAssignmentFileModel> AssignmentFileList = new List<ResponseAssignmentFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseAssignmentFileModel responseAssignmentFileModel = new ResponseAssignmentFileModel();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                var filetyped = FilesBusiness.FileType(newFiles);
                responseFilesModel.Id = newFiles.Id;
                responseFilesModel.name = newFiles.Name;
                if (!string.IsNullOrEmpty(newFiles.Url))
                    responseFilesModel.url = LessonBusiness.geturl(newFiles.Url, Certificate);
                //responseFilesModel.url = newFiles.Url;
                responseFilesModel.filename = newFiles.FileName;
                responseFilesModel.filetypeid = newFiles.FileTypeId;
                responseFilesModel.description = newFiles.Description;
                responseFilesModel.filetypename = filetyped.Filetype;
                responseFilesModel.filesize = newFiles.FileSize;

                responseAssignmentFileModel.id = AssignmentFile.Id;
                responseAssignmentFileModel.files = responseFilesModel;
                AssignmentFileList.Add(responseAssignmentFileModel);
            }
            return AssignmentFileList;
        }

        public List<ResponseAssignmentFileModel> UpdateAssignmentFile(List<AssignmentFile> AssignmentFiles, string Certificate)
        {
            AssignmentFiles = EFAssignmentRepository.UpdateAssignmentFile(AssignmentFiles);
            List<ResponseAssignmentFileModel> AssignmentFileList = new List<ResponseAssignmentFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            foreach (var AssignmentFile in AssignmentFiles)
            {
                ResponseAssignmentFileModel responseAssignmentFileModel = new ResponseAssignmentFileModel();
                Files newFiles = FilesBusiness.getFilesById(AssignmentFile.FileId);
                ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                var filetyped = FilesBusiness.FileType(newFiles);
                responseFilesModel.Id = newFiles.Id;
                responseFilesModel.name = newFiles.Name;
                if (!string.IsNullOrEmpty(newFiles.Url))
                    responseFilesModel.url = LessonBusiness.geturl(newFiles.Url, Certificate);
                //responseFilesModel.url = newFiles.Url;
                responseFilesModel.filename = newFiles.FileName;
                responseFilesModel.filetypeid = newFiles.FileTypeId;
                responseFilesModel.description = newFiles.Description;
                responseFilesModel.filetypename = filetyped.Filetype;
                responseFilesModel.filesize = newFiles.FileSize;
                responseAssignmentFileModel.id = AssignmentFile.Id;
                responseAssignmentFileModel.files = responseFilesModel;
                AssignmentFileList.Add(responseAssignmentFileModel);
            }

            return AssignmentFileList;
        }

        public List<ResponseAssignmentFileModel> DeleteAssignmentFile(long id)
        {

            List<AssignmentFile> assignmentFileList = new List<AssignmentFile>();

            assignmentFileList = EFAssignmentRepository.DeleteAssignmentFile(id);

            List<ResponseAssignmentFileModel> AssignmentFileList = new List<ResponseAssignmentFileModel>();


            return AssignmentFileList;
        }

        public AssignmentStudent CreateAssignmentStudent(AddAssignmentStudentModel AddAssignmentStudentModel, int id)
        {
            AssignmentStudent AssignmentStudent = new AssignmentStudent
            {
                StudentId = AddAssignmentStudentModel.studentid,
                AssignmentId = AddAssignmentStudentModel.assignmentid,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            List<AssignmentStudent> AssignmentStudentList = EFAssignmentStudent.
                                                ListQuery(b => b.StudentId == AddAssignmentStudentModel.studentid
                                                && b.AssignmentId == AddAssignmentStudentModel.assignmentid && b.IsDeleted == false).ToList();

            if (AssignmentStudentList.Count == 0)
            {
                EFAssignmentStudent.Insert(AssignmentStudent);
            }
            else
            {
                AssignmentStudent = null;
            }


            return AssignmentStudent;
        }

        public AssignmentStudent DeleteAssignmentStudent(int Id, int DeleterId)
        {
            AssignmentStudent AssignmentStudent = new AssignmentStudent
            {
                Id = Id,
                DeleterUserId = DeleterId
            };
            int i = EFAssignmentStudent.Delete(AssignmentStudent);
            AssignmentStudent = EFAssignmentStudent.GetById(b => b.Id == AssignmentStudent.Id);
            return AssignmentStudent;
        }

        public List<AssignmentStudent> AssignmentStudentList(PaginationModel paginationModel, out int total)
        {
            List<AssignmentStudent> bundleList = new List<AssignmentStudent>();
            bundleList = EFAssignmentStudent.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = bundleList.Count();

                bundleList = bundleList.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                return bundleList;
            }

            total = bundleList.Count();
            return bundleList;
        }

        public List<Assignment> GetAssignmentStudentById(int id)
        {
            List<Assignment> assignments = new List<Assignment>();
            List<AssignmentStudent> assignmentStudents = EFAssignmentStudent.ListQuery(b => b.StudentId == id).ToList();

            foreach (var assignment in assignmentStudents)
            {
                Assignment assignmentd = EFAssignmentRepository.GetById(int.Parse(assignment.AssignmentId.ToString()));
                assignments.Add(assignmentd);
            }

            return assignments;
        }

        public List<DetailUser> GetStudentsByAssignmentId(int id, string Certificate)
        {
            List<AssignmentStudent> assignmentStudents = EFAssignmentStudent.ListQuery(b => b.AssignmentId == id && b.IsDeleted != true).ToList();
            List<DetailUser> userDTOList = new List<DetailUser>();




            foreach (var assignment in assignmentStudents)
            {
                User user = usersBusiness.GetUserbyId(assignment.StudentId);

                DetailUser userDto = new DetailUser();
                List<Role> roles = usersBusiness.Role(user);

                if (roles != null)
                {
                    List<long> roleids = new List<long>();
                    List<string> rolenames = new List<string>();
                    foreach (var role in roles)
                    {
                        roleids.Add(role.Id);
                        rolenames.Add(role.Name);
                    }
                    userDto.Roles = roleids;
                    userDto.RoleName = rolenames;
                }
                userDto.assignmentStudentId = assignment.Id;
                userDto.Id = user.Id;
                userDto.Username = user.Username;
                userDto.FullName = user.FullName;
                userDto.Email = user.Email;
                userDto.Bio = user.Bio;
                if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                    userDto.profilepicurl = LessonBusiness.geturl(user.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                userDTOList.Add(userDto);
            }
            return userDTOList;
        }

        public bool CheckCourseAssginToUser(int userId, int courseId)
        {
            bool flag = false;
            var data = EFStudentCourseRepository.GetById(b => b.UserId == userId && b.CourseId == courseId && b.IsDeleted != true);
            if (data != null)
            {
                flag = true;
            }
            return flag;
        }

        public List<CourseAssignmentModel> GetAssignmentsByCourse(int Id)
        {
            List<CourseAssignmentModel> _lstAssignments = new List<CourseAssignmentModel>();
            var course = EFCourseRepository.GetById(Id);
            if (course != null)
            {
                var chapter = EFChapterRepository.ListQuery(b => b.CourseId == course.Id && b.IsDeleted != true).ToList();
                foreach (var ch in chapter)
                {

                    var assignment = EFAssignmentRepository.ListQuery(b => b.ChapterId == ch.Id && b.IsDeleted != true).ToList();
                    foreach (var asg in assignment)
                    {
                        CourseAssignmentModel _objAssignmentmodel = new CourseAssignmentModel();
                        _objAssignmentmodel.id = asg.Id;
                        _objAssignmentmodel.chapterid = asg.ChapterId;
                        _objAssignmentmodel.name = asg.Name;
                        _objAssignmentmodel.description = asg.Description;
                        _objAssignmentmodel.code = asg.Code;
                        _lstAssignments.Add(_objAssignmentmodel);
                    }
                }
            }
            return _lstAssignments.ToList();
        }

        public AssignmentFile DeleteAssignmentFileSingle(long id)
        {
            return EFAssignmentRepository.DeleteAssignmentFileSingle(id);
        }

        public Assignment UpdateItemOrder(Assignment dto)
        {
            EFAssignmentRepository.UpdateItemOrder(dto);
            return dto;
        }

        public int GetAssignmentCount(List<int> studentid, int courseid)
        {
            int assignmentCount = 0;
            if (courseid != 0)
            {
                var chapterIds = EFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    assignmentCount = EFAssignmentRepository.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.IsDeleted != true).Count();
                }
                return assignmentCount;
            }
            else
            {
                var userCourse = EFStudentCourseRepository.ListQuery(b => studentid.Contains((int)b.UserId) && b.IsDeleted != true).Select(b => b.CourseId).ToList();
                if (userCourse.Count > 0)
                {
                    var chapterIds = EFChapterRepository.ListQuery(b => userCourse.Contains(b.CourseId) && b.IsDeleted != true).Select(s => s.Id).ToList();
                    if (chapterIds.Count > 0)
                    {
                        assignmentCount = EFAssignmentRepository.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.IsDeleted != true).Count();
                    }
                }
                return assignmentCount;
            }
        }
    }

}

using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.LessonFile;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;


namespace Trainning24.BL.Business
{
    public class LessonBusiness
    {
        private readonly EFLessonRepository EFLessonRepository;
        private readonly EFUsersRepository EFUsersRepository;
        private readonly EFChapterRepository EFChapterRepository;
        private readonly EFCourseRepository EFCourseRepository;
        private readonly FilesBusiness FilesBusiness;
        private readonly ProgressBusiness ProgressBusiness;
        private readonly LogObjectBusiness _logObjectBusiness;
        private readonly EFStudentCourseRepository _eFStudentCourseRepository;
        public LessonBusiness(
            EFLessonRepository EFLessonRepository,
            EFUsersRepository EFUsersRepository,
            EFCourseRepository EFCourseRepository,
            FilesBusiness FilesBusiness,
            EFChapterRepository EFChapterRepository,
            ProgressBusiness ProgressBusiness,
            LogObjectBusiness logObjectBusiness,
            EFStudentCourseRepository eFStudentCourseRepository
        )
        {
            this.EFLessonRepository = EFLessonRepository;
            this.EFCourseRepository = EFCourseRepository;
            this.FilesBusiness = FilesBusiness;
            this.EFChapterRepository = EFChapterRepository;
            this.ProgressBusiness = ProgressBusiness;
            this.EFUsersRepository = EFUsersRepository;
            _logObjectBusiness = logObjectBusiness;
            _eFStudentCourseRepository = eFStudentCourseRepository;
        }



        public string geturltest(string str, string Certificate)
        {
            TimeSpan timeSpan = TimeSpan.FromHours(1);
            double exp = timeSpan.TotalMilliseconds;
            string fileName = "";
            fileName = str.Trim('"');
            string bucName = fileName.Split("/").ElementAt(7);
            string filename = fileName.Split("/").LastOrDefault().Split("?").FirstOrDefault();

            UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(Certificate);
            string url = urlSigner.Sign(
                                           bucName,
                                           filename,
                                           timeSpan,
                                           HttpMethod.Get
                                      );

            return url;
        }

        //getting userid by auth0 id
        public string getUserId(string authid)
        {
            User user = EFUsersRepository.GetById(b => b.AuthId == authid && b.IsDeleted != true);
            if (user != null)
            {
                return user.Id.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        public static string getRoleType(string roleid)
        {
            switch (roleid)
            {
                case "1":
                    return "admin";
                case "2":
                    return "analyst";
                case "3":
                    return "teacher";
                case "4":
                    return "student";
                case "5":
                    return "customer";
                case "6":
                    return "aafmanager";
                case "7":
                    return "coordinator";
                case "8":
                    return "edit_team_leader";
                case "9":
                    return "shooting_team_leader";
                case "10":
                    return "graphics_team_leader";
                case "11":
                    return "quality_assurance";
                case "12":
                    return "feedback_edge_team";
                case "13":
                    return "sales_admin";
                case "14":
                    return "filming_staff";
                case "15":
                    return "editing_staff";
                case "16":
                    return "graphics_staff";
                default:
                    break;
            };
            return "";
        }

        public string geturl(string str, string Certificate)
        {
            string url = "";
            string bucName = "";
            string fileName = "";
            fileName = str.Trim('"');
            string filename = fileName.Split("/").LastOrDefault().Split("?").FirstOrDefault();

            // Here if we add new bucket to google then we need to add another condition for that bucket
            if (str.Contains("edg-primary-course-image-storage"))
            {
                bucName = "edg-primary-course-image-storage";
            }
            else if (str.Contains("edg-primary-profile-image-storage"))
            {
                bucName = "edg-primary-profile-image-storage";
            }
            else if (str.Contains("edg-sales-primary-storage"))
            {
                bucName = "edg-sales-primary-storage";
            }
            else if (str.Contains("t24-primary-audio-storage"))
            {
                bucName = "t24-primary-audio-storage";
            }
            else if (str.Contains("t24-primary-image-storage"))
            {
                bucName = "t24-primary-image-storage";
            }
            else if (str.Contains("t24-primary-pdf-storage"))
            {
                bucName = "t24-primary-pdf-storage";
            }
            else if (str.Contains("t24-primary-video-storage"))
            {
                bucName = "t24-primary-video-storage";
            }
            else
            {
                return str;
            }

            if (bucName == "t24-primary-image-storage")
            {
                return str;
            }
            else
            {
                TimeSpan timeSpan = TimeSpan.FromHours(1);
                double exp = timeSpan.TotalMilliseconds;

                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(Certificate);
                url = urlSigner.Sign(
                                               bucName,
                                               filename,
                                               timeSpan,
                                               HttpMethod.Get
                                          );

                return url;
            }
        }

        public string geturl(string str, string Certificate, string bucketname)
        {
            TimeSpan timeSpan = TimeSpan.FromHours(1);
            double exp = timeSpan.TotalMilliseconds;
            string fileName = "";
            fileName = str.Trim('"');
            string filename = fileName.Split("/").LastOrDefault().Split("?").FirstOrDefault();

            UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(Certificate);
            string url = urlSigner.Sign(
                                           bucketname,
                                           filename,
                                           timeSpan,
                                           HttpMethod.Get
                                      );

            return url;
        }

        public Lesson Create(AddLessonModel AddLessonModel, int id)
        {
            Lesson Lesson = new Lesson
            {
                Name = AddLessonModel.name,
                Code = AddLessonModel.code,
                Description = AddLessonModel.description,
                ChapterId = AddLessonModel.chapterid,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };
            List<Lesson> lesson = EFLessonRepository.ListQuery(b => b.ChapterId == AddLessonModel.chapterid).ToList();
            if (lesson.Count == 0)
            {
                Lesson.ItemOrder = 1;
            }
            else
            {
                Lesson.ItemOrder = EFLessonRepository.ListQuery(b => b.ChapterId == AddLessonModel.chapterid).Select(b => b.ItemOrder).Max() + 1;
            }
            EFLessonRepository.Insert(Lesson);
            _logObjectBusiness.AddLogsObject(10, Lesson.Id, id);
            return Lesson;
        }

        public Lesson Update(AddLessonModel UpdateLessonModel, int id)
        {
            Lesson lessons = getLessonById(int.Parse(UpdateLessonModel.id.ToString()));
            if (lessons != null)
            {
                lessons.Id = UpdateLessonModel.id;
                lessons.Name = UpdateLessonModel.name;
                lessons.Code = UpdateLessonModel.code;
                lessons.Description = UpdateLessonModel.description;
                lessons.ChapterId = UpdateLessonModel.chapterid;
                //lessons.ItemOrder = lessons.ItemOrder;
                lessons.LastModificationTime = DateTime.Now.ToString();
                lessons.LastModifierUserId = id;

                EFLessonRepository.Update(lessons);
                _logObjectBusiness.AddLogsObject(11, lessons.Id, id);
            }
            return lessons;
        }

        public Lesson UpdateItemOrder(Lesson dto)
        {
            EFLessonRepository.Update(dto);
            _logObjectBusiness.AddLogsObject(11, dto.Id, dto.LastModifierUserId ?? 0);
            return dto;
        }

        public List<Lesson> LessonList()
        {
            return EFLessonRepository.GetAll();
        }

        public Lesson getLessonById(int id)
        {
            return EFLessonRepository.GetById(id);
        }

        public List<Lesson> GetLessonByGivenChapterId(int id)
        {
            return EFLessonRepository.ListQuery(b => b.ChapterId == id).ToList();
        }

        public List<Lesson> GetLessonByCourseId(int id)
        {
            List<Chapter> chapters = EFChapterRepository.ListQuery(b => b.CourseId == id).ToList();

            List<Lesson> lessonsList = new List<Lesson>();
            foreach (var chapter in chapters)
            {
                List<Lesson> lessons = EFLessonRepository.ListQuery(b => b.ChapterId == chapter.Id).ToList();
                lessonsList.AddRange(lessons);
            }

            return lessonsList;
        }

        public Lesson Delete(int Id, int DeleterId)
        {
            Lesson Lesson = new Lesson();
            Lesson.Id = Id;
            Lesson.DeleterUserId = DeleterId;
            int i = EFLessonRepository.Delete(Lesson);
            _logObjectBusiness.AddLogsObject(12, Lesson.Id, DeleterId);
            return Lesson;
        }

        public List<Lesson> LessonList(PaginationModel paginationModel, out int total)
        {
            List<Lesson> LessonList = new List<Lesson>();
            LessonList = EFLessonRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = LessonList.Count();

                LessonList = LessonList.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    LessonList = LessonList.Where(b => b.Name.Any(k => b.Id.ToString().Contains(paginationModel.search)
                                                                    || b.Name.ToLower().Contains(paginationModel.search.ToLower())
                                                                    || b.Code.ToLower().Contains(paginationModel.search.ToLower())
                                                                    || b.Description.ToLower().Contains(paginationModel.search.ToLower())
                                                                    )).ToList();
                return LessonList;
            }

            total = LessonList.Count();
            return EFLessonRepository.GetAll();
        }

        public List<ResponseLessonFileModel> CreateLessonFile(List<LessonFile> lessonFiles)
        {
            lessonFiles = EFLessonRepository.InsertLessionFile(lessonFiles);

            List<ResponseLessonFileModel> LessonFileList = new List<ResponseLessonFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();

            foreach (var lessonFile in lessonFiles)
            {
                ResponseLessonFileModel responseLessonFileModel = new ResponseLessonFileModel();
                Files newFiles = FilesBusiness.getFilesById(lessonFile.FileId);
                if (newFiles != null)
                {
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
                }
                LessonFileList.Add(responseLessonFileModel);
            }

            return LessonFileList;
        }

        public List<ResponseLessonFileModel> GetLessionFilesByLessionId(long Id)
        {
            List<LessonFile> lessonFiles = EFLessonRepository.GetLessionFile(Id);

            List<ResponseLessonFileModel> LessonFileList = new List<ResponseLessonFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();


            foreach (var lessonFile in lessonFiles)
            {
                ResponseLessonFileModel responseLessonFileModel = new ResponseLessonFileModel();
                ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                Files newFiles = FilesBusiness.getFilesById(lessonFile.FileId);
                if (newFiles != null)
                {
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;

                    responseLessonFileModel.id = lessonFile.Id;
                    responseLessonFileModel.files = responseFilesModel;
                    LessonFileList.Add(responseLessonFileModel);
                }
            }

            return LessonFileList;
        }


        public ResponseFilesModel1 GetLessionFilesByLessionId1(long Id)
        {
            List<LessonFile> lessonFiles = EFLessonRepository.GetLessionFile(Id);
            var lessonfile = lessonFiles.OrderByDescending(b => b.Id).First();
            ResponseFilesModel1 responseLessonFileModel = new ResponseFilesModel1();
            Files newFiles = FilesBusiness.getFilesById(lessonfile.FileId);
            var filetyped = FilesBusiness.FileType(newFiles);
            responseLessonFileModel.Id = newFiles.Id;
            responseLessonFileModel.name = newFiles.Name;
            responseLessonFileModel.filetypeid = newFiles.FileTypeId;
            responseLessonFileModel.filetypename = filetyped.Filetype;
            return responseLessonFileModel;
        }

        public List<ResponseLessonFileModel> GetLessionFilesByLessionId(long Id, string Certificate)
        {
            List<LessonFile> lessonFiles = EFLessonRepository.GetLessionFile(Id);

            List<ResponseLessonFileModel> LessonFileList = new List<ResponseLessonFileModel>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();


            foreach (var lessonFile in lessonFiles)
            {

                if (lessonFile.Id == 103)
                {
                    Console.WriteLine("vvv");
                }
                try
                {
                    ResponseLessonFileModel responseLessonFileModel = new ResponseLessonFileModel();
                    Files newFiles = FilesBusiness.getFilesById(lessonFile.FileId);
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    if (!string.IsNullOrEmpty(newFiles.Url))
                        responseFilesModel.url = geturl(newFiles.Url, Certificate);
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;

                    responseLessonFileModel.id = lessonFile.Id;
                    responseLessonFileModel.files = responseFilesModel;
                    LessonFileList.Add(responseLessonFileModel);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }



            }

            return LessonFileList;
        }

        public List<ResponseLessonFileModel> GetLessionFilesByLessionId(long Id, long studentid, string Certificate)
        {
            List<LessonFile> lessonFiles = EFLessonRepository.GetLessionFile(Id);
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
                responseFilesModel.url = geturl(newFiles.Url, Certificate);
                responseFilesModel.filename = newFiles.FileName;
                responseFilesModel.filetypeid = newFiles.FileTypeId;
                responseFilesModel.description = newFiles.Description;
                responseFilesModel.filetypename = filetyped.Filetype;
                responseFilesModel.filesize = newFiles.FileSize;
                responseFilesModel.duration = newFiles.Duration;
                responseFilesModel.totalpages = newFiles.TotalPages;
                responseLessonFileModel.progress = ProgressBusiness.GetStudentLessonProgressByLession(Id, studentid);
                responseLessonFileModel.id = lessonFile.Id;
                responseLessonFileModel.files = responseFilesModel;
                LessonFileList.Add(responseLessonFileModel);
            }
            return LessonFileList;
        }

        public List<ResponseLessonFileModel> UpdateLessonFile(List<LessonFile> lessonFiles)
        {
            lessonFiles = EFLessonRepository.UpdateLessionFile(lessonFiles);

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

        public LessonDTO GetLessionById(Lesson obj)
        {
            LessonDTO lesson = new LessonDTO();
            if (obj != null)
            {
                lesson.id = obj.Id;
                lesson.name = obj.Name;
                lesson.code = obj.Code;
                lesson.description = obj.Description;
            }
            return lesson;
        }

        public LessonDTO GetLessionById(int Id)
        {
            Lesson lesson = EFLessonRepository.GetById(Id);
            LessonDTO lessonDTO = new LessonDTO();
            if (lesson != null)
            {
                lessonDTO.id = lesson.Id;
                lessonDTO.name = lesson.Name;
                lessonDTO.code = lesson.Code;
                lessonDTO.description = lesson.Description;
            }
            return lessonDTO;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public async Task<bool> DeleteLesson(List<LessonDeleteExcelDTO> dto)
        {
            if (dto.Count > 0)
            {
                List<long> lessonsIds = dto.Select(s => s.LessonId).ToList();
                await EFLessonRepository.DeleteLessonById(lessonsIds);
            }
            return true;
        }

        public async Task<int> SaveUpdateLessonFiles(List<LessonFileExcelDTO> dto)
        {
            if (dto.Count > 0)
            {
                List<LessonFile> saveLessonFiles = new List<LessonFile>();
                List<LessonFile> updateLessonFiles = new List<LessonFile>();
                foreach (var dt in dto)
                {
                    var exist = await EFLessonRepository.GetLessonFile(b => b.LessionId == dt.LessonId && b.FileId == dt.FileId && b.IsDeleted != true);
                    if (exist == null)
                    {
                        LessonFile lessonFile = new LessonFile();
                        lessonFile.LessionId = dt.LessonId;
                        lessonFile.FileId = dt.FileId;
                        lessonFile.CreationTime = DateTime.Now.ToString();
                        lessonFile.CreatorUserId = 1;
                        saveLessonFiles.Add(lessonFile);
                    }
                    else
                    {
                        exist.LessionId = dt.LessonId;
                        exist.FileId = dt.FileId;
                        exist.LastModificationTime = DateTime.Now.ToString();
                        exist.LastModifierUserId = 1;
                        updateLessonFiles.Add(exist);
                    }
                }
                if (saveLessonFiles.Count > 0)
                    await EFLessonRepository.SaveLessonFileBulk(saveLessonFiles);
                if (updateLessonFiles.Count > 0)
                    await EFLessonRepository.UpdateLessonFileBulk(updateLessonFiles);
            }
            return 1;
        }

        public int GetLessonCount(List<int> studentid, int courseid)
        {
            int LessonCount = 0;
            if (courseid != 0)
            {
                var chapterIds = EFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    LessonCount = EFLessonRepository.ListQuery(b => chapterIds.Contains(b.ChapterId ?? 0) && b.IsDeleted != true).Count();
                }
                return LessonCount;
            }
            else
            {
                var userCourse = _eFStudentCourseRepository.ListQuery(b => studentid.Contains((int)b.UserId) && b.IsDeleted != true).Select(b => b.CourseId).ToList();
                if (userCourse.Count > 0)
                {
                    var chapterIds = EFChapterRepository.ListQuery(b => userCourse.Contains(b.CourseId) && b.IsDeleted != true).Select(s => s.Id).ToList();
                    if (chapterIds.Count > 0)
                    {
                        LessonCount = EFLessonRepository.ListQuery(b => chapterIds.Contains(b.ChapterId ?? 0) && b.IsDeleted != true).Count();
                    }
                }
                return LessonCount;
            }
        }

        public List<Lesson> GetLessonByCourse(int id)
        {
            List<Chapter> chapters = EFChapterRepository.ListQuery(b => b.CourseId == id).ToList();
            List<Lesson> lessonsList = new List<Lesson>();
            foreach (var chapter in chapters)
            {
                List<Lesson> lessons = EFLessonRepository.ListQuery(b => b.ChapterId == chapter.Id).ToList();
                lessonsList.AddRange(lessons);
            }
            return lessonsList;
        }
    }
}

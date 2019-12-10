using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Contact;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Feedback;
using Trainning24.BL.ViewModels.FeedBackCategory;
using Trainning24.BL.ViewModels.FeedBackTask;
using Trainning24.BL.ViewModels.FeedbackTime;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.QuestionFile;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class FeedbackBusiness
    {
        private readonly EFContact EFContact;
        private readonly EFFeedbackTime EFFeedbackTime;
        private readonly EFFeedbackFile EFFeedbackFile;
        private readonly EFFeedback EFFeedback;
        private readonly LessonBusiness LessonBusiness;
        private readonly FilesBusiness FilesBusiness;
        private readonly UsersBusiness UsersBusiness;
        private readonly FeedBackTaskBusiness FeedBackTaskBusiness;
        private readonly EFFeedBackCategory EFFeedBackCategory;
        private readonly EFGradeRepository EFGradeRepository;
        private readonly EFChapterRepository EFChapterRepository;
        private readonly EFFeedBackTaskStatusOption EFFeedBackTaskStatusOption;
        private readonly EFFeedBackTaskStatus EFFeedBackTaskStatus;
        private readonly EFFeedBackTask EFFeedBackTask;
        private readonly EFCourseRepository EFCourseRepository;
        private readonly EFLessonRepository EFLessonRepository;
        private readonly EFFeedbackStaffRepository EFFeedbackStaffRepository;
        private readonly IMapper _mapper;

        public FeedbackBusiness(
            EFContact EFContact,
            EFFeedbackTime EFFeedbackTime,
            FeedBackTaskBusiness FeedBackTaskBusiness,
            LessonBusiness LessonBusiness,
            UsersBusiness UsersBusiness,
            FilesBusiness FilesBusiness,
            EFFeedBackTaskStatus EFFeedBackTaskStatus,
            EFFeedBackTaskStatusOption EFFeedBackTaskStatusOption,
            EFFeedbackFile EFFeedbackFile,
            EFFeedback EFFeedback,
            EFFeedBackTask EFFeedBackTask,
            EFFeedBackCategory EFFeedBackCategory,
            IMapper mapper,
            EFGradeRepository EFGradeRepository,
            EFChapterRepository EFChapterRepository,
            EFCourseRepository EFCourseRepository,
            EFLessonRepository EFLessonRepository,
            EFFeedbackStaffRepository EFFeedbackStaffRepository
            )
        {
            this.FilesBusiness = FilesBusiness;
            this.EFFeedBackTaskStatusOption = EFFeedBackTaskStatusOption;
            this.UsersBusiness = UsersBusiness;
            this.EFContact = EFContact;
            this.EFFeedbackTime = EFFeedbackTime;
            this.EFFeedbackFile = EFFeedbackFile;
            this.EFFeedback = EFFeedback;
            this.EFFeedBackTaskStatus = EFFeedBackTaskStatus;
            this.EFFeedBackTask = EFFeedBackTask;
            this.EFFeedBackCategory = EFFeedBackCategory;
            this.EFGradeRepository = EFGradeRepository;
            this.FeedBackTaskBusiness = FeedBackTaskBusiness;
            this.EFChapterRepository = EFChapterRepository;
            this.LessonBusiness = LessonBusiness;
            this.EFCourseRepository = EFCourseRepository;
            this.EFLessonRepository = EFLessonRepository;
            this.EFFeedbackStaffRepository = EFFeedbackStaffRepository;
            _mapper = mapper;
        }

        public Contact AddContact(Contact contact)
        {
            contact.CreationTime = DateTime.Now.ToString();
            contact.IsDeleted = false;
            EFContact.Insert(contact);
            return contact;
        }

        public FeebackTime AddFeedbackTime(FeebackTime feebackTime)
        {
            feebackTime.CreationTime = DateTime.Now.ToString();
            feebackTime.IsDeleted = false;
            feebackTime.CreatorUserId = 1;
            EFFeedbackTime.Insert(feebackTime);
            return feebackTime;
        }

        public FeedbackFile AddFeedbackFile(FeedbackFile feedbackFile)
        {
            EFFeedbackFile.Insert(feedbackFile);
            return feedbackFile;
        }


        public Feedback FeedbackStatusChangedToCompleted(FeedbackStatusChangedModel obj, int id)
        {
            Feedback feedback = EFFeedback.GetById(b => b.Id == obj.feedbackid);
            feedback.Status = obj.status;
            feedback.LastModificationTime = DateTime.Now.ToString();
            feedback.LastModifierUserId = id;
            EFFeedback.Update(feedback);
            return feedback;
        }

        public Feedback AddFeedback(Feedback feedback, int id)
        {
            feedback.CreatorUserId = id;
            feedback.CreationTime = DateTime.UtcNow.ToString();
            feedback.IsDeleted = false;
            EFFeedback.Insert(feedback);
            return feedback;
        }

        public List<FeedBackCategory> GetFeedBackCategory()
        {
            return EFFeedBackCategory.GetAll();
        }

        public ResponseFeedback GetFeedback(long id, string Certificate)
        {
            ResponseFeedback responseFeedback = new ResponseFeedback();
            Feedback feedback = EFFeedback.GetById(b => b.Id == id);

            if (feedback != null)
            {
                responseFeedback.Id = feedback.Id;
                responseFeedback.Creationdate = feedback.CreationTime;
                responseFeedback.Grade = _mapper.Map<Grade, ResponseGradeModel>(EFGradeRepository.GetById(b => b.Id == feedback.GradeId));
                responseFeedback.Category = _mapper.Map<FeedBackCategory, ResponseFeedBackCategory>(EFFeedBackCategory.GetById(b => b.Id == feedback.GradeId));
                responseFeedback.Chapter = _mapper.Map<Chapter, ResponseChapter>(EFChapterRepository.GetById(b => b.Id == feedback.ChapterId));
                responseFeedback.Contact = _mapper.Map<Contact, ResponseContact>(EFContact.GetById(b => b.Id == feedback.Contactid));

                List<FeedBackTask> editingtask = FeedBackTaskBusiness.getFeedBackTaskByFeedbackId(feedback.Id, 1);
                List<FeedBackTask> graphicstask = FeedBackTaskBusiness.getFeedBackTaskByFeedbackId(feedback.Id, 2);
                List<FeedBackTask> filmingtask = FeedBackTaskBusiness.getFeedBackTaskByFeedbackId(feedback.Id, 3);

                if (editingtask.Count != 0)
                {
                    List<ResponseFeedBackTaskModel> lst = new List<ResponseFeedBackTaskModel>();
                    foreach (var item in editingtask)
                    {
                        FeedBackTaskStatus feedBackTaskStatus = EFFeedBackTaskStatus.ListQuery(b => b.FeedbackId == item.Id).LastOrDefault();

                        FeedBackTaskStatusOption feedBackTaskStatusO = new FeedBackTaskStatusOption();
                        if (feedBackTaskStatus != null)
                        {
                            feedBackTaskStatusO = EFFeedBackTaskStatusOption.GetById(b => b.Id == feedBackTaskStatus.Status);
                        }

                        ResponseFeedBackTaskModel responseFeedBackTaskModel = new ResponseFeedBackTaskModel
                        {
                            creationtime = item.CreationTime,
                            description = item.Description,
                            feedbackid = item.FeedbackId,
                            filelink = item.FileLink,
                            id = item.Id,
                            type = item.Type
                        };

                        if (feedBackTaskStatusO != null)
                        {
                            responseFeedBackTaskModel.Status = feedBackTaskStatusO.Name;
                        }


                        lst.Add(responseFeedBackTaskModel);
                    }
                    responseFeedback.editingtask = lst;
                }

                if (graphicstask.Count != 0)
                {
                    List<ResponseFeedBackTaskModel> lst = new List<ResponseFeedBackTaskModel>();
                    foreach (var item in graphicstask)
                    {
                        FeedBackTaskStatus feedBackTaskStatus = EFFeedBackTaskStatus.ListQuery(b => b.FeedbackId == item.Id).LastOrDefault();

                        FeedBackTaskStatusOption feedBackTaskStatusO = new FeedBackTaskStatusOption();
                        if (feedBackTaskStatus != null)
                        {
                            feedBackTaskStatusO = EFFeedBackTaskStatusOption.GetById(b => b.Id == feedBackTaskStatus.Status);
                        }

                        ResponseFeedBackTaskModel responseFeedBackTaskModel = new ResponseFeedBackTaskModel
                        {
                            creationtime = item.CreationTime,
                            description = item.Description,
                            feedbackid = item.FeedbackId,
                            filelink = item.FileLink,
                            id = item.Id,
                            type = item.Type
                        };

                        if (feedBackTaskStatusO != null)
                        {
                            responseFeedBackTaskModel.Status = feedBackTaskStatusO.Name;
                        }

                        lst.Add(responseFeedBackTaskModel);
                    }
                    responseFeedback.graphicstask = lst;
                }

                if (filmingtask.Count != 0)
                {
                    List<ResponseFeedBackTaskModel> lst = new List<ResponseFeedBackTaskModel>();
                    foreach (var item in filmingtask)
                    {
                        FeedBackTaskStatus feedBackTaskStatus = EFFeedBackTaskStatus.ListQuery(b => b.FeedbackId == item.Id).LastOrDefault();

                        FeedBackTaskStatusOption feedBackTaskStatusO = new FeedBackTaskStatusOption();
                        if (feedBackTaskStatus != null)
                        {
                            feedBackTaskStatusO = EFFeedBackTaskStatusOption.GetById(b => b.Id == feedBackTaskStatus.Status);
                        }

                        ResponseFeedBackTaskModel responseFeedBackTaskModel = new ResponseFeedBackTaskModel
                        {
                            creationtime = item.CreationTime,
                            description = item.Description,
                            feedbackid = item.FeedbackId,
                            filelink = item.FileLink,
                            id = item.Id,
                            type = item.Type
                        };

                        if (feedBackTaskStatusO != null)
                        {
                            responseFeedBackTaskModel.Status = feedBackTaskStatusO.Name;
                        }


                        lst.Add(responseFeedBackTaskModel);
                    }
                    responseFeedback.filmingtask = lst;
                }

                //responseFeedback.editingtask = _mapper.Map<List<FeedBackTask>, List<ResponseFeedBackTaskModel>>(FeedBackTaskBusiness.getFeedBackTaskByFeedbackId(feedback.Id,1));
                //responseFeedback.graphicstask = _mapper.Map<List<FeedBackTask>, List<ResponseFeedBackTaskModel>>(FeedBackTaskBusiness.getFeedBackTaskByFeedbackId(feedback.Id,2));
                //responseFeedback.filmingtask = _mapper.Map<List<FeedBackTask>, List<ResponseFeedBackTaskModel>>(FeedBackTaskBusiness.getFeedBackTaskByFeedbackId(feedback.Id,3));

                responseFeedback.Status = feedback.Status;

                if (feedback.QaUser != 0)
                {
                    User user = UsersBusiness.GetUserbyId(feedback.QaUser);
                    FeedBackUser qaUser = new FeedBackUser();
                    List<Role> roles = UsersBusiness.Role(user);

                    if (roles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in roles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        qaUser.Roles = roleids;
                        qaUser.RoleName = rolenames;
                    }

                    qaUser.Id = user.Id;
                    qaUser.Username = user.Username;
                    qaUser.FullName = user.FullName;
                    qaUser.Email = user.Email;
                    qaUser.Bio = user.Bio;
                    if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                        qaUser.profilepicurl = LessonBusiness.geturl(user.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                    responseFeedback.QaUser = qaUser;
                }
                if (feedback.Coordinator != 0)
                {

                    User coordinator = UsersBusiness.GetUserbyId(feedback.Coordinator);
                    FeedBackUser CoordinatorUser = new FeedBackUser();
                    List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);

                    if (Coordinatorroles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in Coordinatorroles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        CoordinatorUser.Roles = roleids;
                        CoordinatorUser.RoleName = rolenames;
                    }

                    CoordinatorUser.Id = coordinator.Id;
                    CoordinatorUser.Username = coordinator.Username;
                    CoordinatorUser.FullName = coordinator.FullName;
                    CoordinatorUser.Email = coordinator.Email;
                    CoordinatorUser.Bio = coordinator.Bio;
                    if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                        CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");

                    responseFeedback.Coordinator = CoordinatorUser;
                }

                FeedBackStaff editingManager = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 1 && b.IsManager == true && b.IsDeleted != true).SingleOrDefault();
                if (editingManager != null)
                {
                    User coordinator = UsersBusiness.GetUserbyId(editingManager.UserId);
                    FeedBackUser CoordinatorUser = new FeedBackUser();
                    List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);

                    if (Coordinatorroles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in Coordinatorroles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        CoordinatorUser.Roles = roleids;
                        CoordinatorUser.RoleName = rolenames;
                    }

                    CoordinatorUser.Id = coordinator.Id;
                    CoordinatorUser.Username = coordinator.Username;
                    CoordinatorUser.FullName = coordinator.FullName;
                    CoordinatorUser.Email = coordinator.Email;
                    CoordinatorUser.Bio = coordinator.Bio;
                    if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                        CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                    responseFeedback.editingManager = CoordinatorUser;
                }

                List<FeedBackStaff> editingStaffList = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 1 && b.IsManager != true && b.IsDeleted != true).ToList();
                if (editingStaffList.Count != 0)
                {
                    List<FeedBackUser> editingStaffs = new List<FeedBackUser>();
                    foreach (var editingStaff in editingStaffList)
                    {
                        User coordinator = UsersBusiness.GetUserbyId(editingStaff.UserId);
                        FeedBackUser CoordinatorUser = new FeedBackUser();
                        List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);
                        if (Coordinatorroles != null)
                        {
                            List<long> roleids = new List<long>();
                            List<string> rolenames = new List<string>();
                            foreach (var role in Coordinatorroles)
                            {
                                roleids.Add(role.Id);
                                rolenames.Add(role.Name);
                            }
                            CoordinatorUser.Roles = roleids;
                            CoordinatorUser.RoleName = rolenames;
                        }
                        CoordinatorUser.Id = coordinator.Id;
                        CoordinatorUser.Username = coordinator.Username;
                        CoordinatorUser.FullName = coordinator.FullName;
                        CoordinatorUser.Email = coordinator.Email;
                        CoordinatorUser.Bio = coordinator.Bio;
                        if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                            CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                        editingStaffs.Add(CoordinatorUser);
                    }
                    responseFeedback.editingStaffs = editingStaffs;
                }

                FeedBackStaff graphicsManager = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 2 && b.IsManager == true && b.IsDeleted != true).SingleOrDefault();
                if (graphicsManager != null)
                {
                    User coordinator = UsersBusiness.GetUserbyId(graphicsManager.UserId);
                    FeedBackUser CoordinatorUser = new FeedBackUser();
                    List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);

                    if (Coordinatorroles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in Coordinatorroles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        CoordinatorUser.Roles = roleids;
                        CoordinatorUser.RoleName = rolenames;
                    }

                    CoordinatorUser.Id = coordinator.Id;
                    CoordinatorUser.Username = coordinator.Username;
                    CoordinatorUser.FullName = coordinator.FullName;
                    CoordinatorUser.Email = coordinator.Email;
                    CoordinatorUser.Bio = coordinator.Bio;
                    if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                        CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                    responseFeedback.graphicsManager = CoordinatorUser;
                }

                List<FeedBackStaff> graphicsStaffList = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 2 && b.IsManager != true && b.IsDeleted != true).ToList();
                if (graphicsStaffList.Count != 0)
                {
                    List<FeedBackUser> graphicsStaffs = new List<FeedBackUser>();
                    foreach (var graphicsStaff in graphicsStaffList)
                    {
                        User coordinator = UsersBusiness.GetUserbyId(graphicsStaff.UserId);
                        FeedBackUser CoordinatorUser = new FeedBackUser();
                        List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);
                        if (Coordinatorroles != null)
                        {
                            List<long> roleids = new List<long>();
                            List<string> rolenames = new List<string>();
                            foreach (var role in Coordinatorroles)
                            {
                                roleids.Add(role.Id);
                                rolenames.Add(role.Name);
                            }
                            CoordinatorUser.Roles = roleids;
                            CoordinatorUser.RoleName = rolenames;
                        }
                        CoordinatorUser.Id = coordinator.Id;
                        CoordinatorUser.Username = coordinator.Username;
                        CoordinatorUser.FullName = coordinator.FullName;
                        CoordinatorUser.Email = coordinator.Email;
                        CoordinatorUser.Bio = coordinator.Bio;
                        if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                            CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                        graphicsStaffs.Add(CoordinatorUser);
                    }
                    responseFeedback.graphicsStaffs = graphicsStaffs;
                }

                FeedBackStaff filmingManager = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 3 && b.IsManager == true && b.IsDeleted != true).SingleOrDefault();
                if (filmingManager != null)
                {
                    User coordinator = UsersBusiness.GetUserbyId(filmingManager.UserId);
                    FeedBackUser CoordinatorUser = new FeedBackUser();
                    List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);

                    if (Coordinatorroles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in Coordinatorroles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        CoordinatorUser.Roles = roleids;
                        CoordinatorUser.RoleName = rolenames;
                    }

                    CoordinatorUser.Id = coordinator.Id;
                    CoordinatorUser.Username = coordinator.Username;
                    CoordinatorUser.FullName = coordinator.FullName;
                    CoordinatorUser.Email = coordinator.Email;
                    CoordinatorUser.Bio = coordinator.Bio;
                    if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                        CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                    responseFeedback.filmingManager = CoordinatorUser;
                }

                List<FeedBackStaff> filmingStaffList = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 3 && b.IsManager != true && b.IsDeleted != true).ToList();
                if (filmingStaffList.Count != 0)
                {
                    List<FeedBackUser> filmingStaffs = new List<FeedBackUser>();
                    foreach (var filmingStaff in filmingStaffList)
                    {
                        User coordinator = UsersBusiness.GetUserbyId(filmingStaff.UserId);
                        FeedBackUser CoordinatorUser = new FeedBackUser();
                        List<Role> Coordinatorroles = UsersBusiness.Role(coordinator);
                        if (Coordinatorroles != null)
                        {
                            List<long> roleids = new List<long>();
                            List<string> rolenames = new List<string>();
                            foreach (var role in Coordinatorroles)
                            {
                                roleids.Add(role.Id);
                                rolenames.Add(role.Name);
                            }
                            CoordinatorUser.Roles = roleids;
                            CoordinatorUser.RoleName = rolenames;
                        }
                        CoordinatorUser.Id = coordinator.Id;
                        CoordinatorUser.Username = coordinator.Username;
                        CoordinatorUser.FullName = coordinator.FullName;
                        CoordinatorUser.Email = coordinator.Email;
                        CoordinatorUser.Bio = coordinator.Bio;
                        if (!string.IsNullOrEmpty(coordinator.ProfilePicUrl))
                            CoordinatorUser.profilepicurl = LessonBusiness.geturl(coordinator.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                        filmingStaffs.Add(CoordinatorUser);
                    }
                    responseFeedback.filmingStaffs = filmingStaffs;
                }


                List<ResponseFdTime> feedbacktimedetails = GetFeedBackTimeDetails(id, Certificate);

                responseFeedback.feedbacktime = feedbacktimedetails;

                ResponseCourseModel responseCourseModel = new ResponseCourseModel();

                if (feedback.CourseId != 0)
                {
                    Course course = EFCourseRepository.GetById(b => b.Id == feedback.CourseId);
                    responseCourseModel.Code = course.Code;
                    responseCourseModel.Description = course.Description;
                    responseCourseModel.Id = int.Parse(course.Id.ToString());
                    responseCourseModel.Name = course.Name;
                    responseCourseModel.Description = feedback.Description;

                }

                responseFeedback.Course = responseCourseModel;
                responseFeedback.Lesson = _mapper.Map<Lesson, ResponseLessonModelByChapter>(EFLessonRepository.GetById(b => b.Id == feedback.LessonId));
                return responseFeedback;
            }
            else
            {
                return null;
            }
        }

        public List<ResponseFdTime> GetFeedBackTimeDetails(long id, string Certificate)
        {
            List<FeebackTime> feebackTimes = EFFeedbackTime.ListQuery(b => b.FeedbackId == id).ToList();
            List<ResponseFdTime> responseFeedbackTimeList = new List<ResponseFdTime>();
            foreach (var ft in feebackTimes)
            {
                ResponseFdTime rft = new ResponseFdTime();
                rft.Id = ft.Id;
                rft.Description = ft.Description;
                rft.Time = ft.Time;
                List<FeedbackFile> feedbackFiles = EFFeedbackFile.ListQuery(b => b.FeedtimeId == ft.Id).ToList();
                List<UpdateQuestionFileModel1> feedbackTimeFiles = new List<UpdateQuestionFileModel1>();
                foreach (var feedbackfile in feedbackFiles)
                {
                    Files newFiles = FilesBusiness.getFilesById(feedbackfile.FileId);
                    UpdateQuestionFileModel1 singleanswerFile = new UpdateQuestionFileModel1();
                    singleanswerFile.fileid = newFiles.Id;
                    if (!string.IsNullOrEmpty(newFiles.Url))
                        singleanswerFile.Url = LessonBusiness.geturl(newFiles.Url, Certificate);

                    singleanswerFile.name = newFiles.FileName;
                    singleanswerFile.duration = newFiles.Duration;


                    feedbackTimeFiles.Add(singleanswerFile);
                }
                rft.feedbackTimeFiles = feedbackTimeFiles;
                responseFeedbackTimeList.Add(rft);
            }
            return responseFeedbackTimeList;
        }

        public AllResponseFeedback GetFeedbackTmp(long id)
        {
            try
            {
                AllResponseFeedback responseFeedback = new AllResponseFeedback();
                Feedback feedback = EFFeedback.GetById(b => b.Id == id);

                if (feedback != null)
                {
                    responseFeedback.Id = feedback.Id;
                    responseFeedback.Creationdate = feedback.CreationTime;
                    responseFeedback.Grade = _mapper.Map<Grade, AllResponseGradeModel>(EFGradeRepository.GetById(b => b.Id == feedback.GradeId));
                    responseFeedback.Category = _mapper.Map<FeedBackCategory, ResponseFeedBackCategory>(EFFeedBackCategory.GetById(b => b.Id == feedback.CategoryId));
                    responseFeedback.Chapter = _mapper.Map<Chapter, ResponseChapter>(EFChapterRepository.GetById(b => b.Id == feedback.ChapterId));
                    responseFeedback.Contact = _mapper.Map<Contact, AllResponseContact>(EFContact.GetById(b => b.Id == feedback.Contactid));
                    responseFeedback.Status = feedback.Status;

                    if (feedback.QaUser != 0)
                    {
                        User user = UsersBusiness.GetUserbyId(feedback.QaUser);
                        AllFeedBackUser qaUser = new AllFeedBackUser();
                        if (user != null)
                        {
                            List<Role> roles = UsersBusiness.Role(user);
                            qaUser.Id = user.Id;
                            qaUser.Username = user.Username;
                            responseFeedback.QaUser = qaUser;
                        }
                    }

                    if (feedback.Coordinator != 0)
                    {

                        User coordinator = UsersBusiness.GetUserbyId(feedback.Coordinator);
                        AllFeedBackUser CoordinatorUser = new AllFeedBackUser();
                        if (coordinator != null)
                        {
                            CoordinatorUser.Id = coordinator.Id;
                            CoordinatorUser.Username = coordinator.Username;
                            responseFeedback.Coordinator = CoordinatorUser;
                        }
                    }



                    FeedBackStaff editingStaff = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 1 && b.IsManager == true && b.IsDeleted != true).SingleOrDefault();

                    if (editingStaff != null)
                    {
                        User editingManager1st = UsersBusiness.GetUserbyId(editingStaff.UserId);
                        AllFeedBackUser editingManager = new AllFeedBackUser();
                        editingManager.Id = editingManager1st.Id;
                        editingManager.Username = editingManager1st.Username;
                        responseFeedback.editingManager = editingManager;
                    }

                    FeedBackStaff graphicsStaff = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 2 && b.IsManager == true && b.IsDeleted != true).SingleOrDefault();

                    if (graphicsStaff != null)
                    {
                        User graphicsManager1st = UsersBusiness.GetUserbyId(graphicsStaff.UserId);
                        AllFeedBackUser graphicsManager = new AllFeedBackUser();

                        if (graphicsManager1st != null)
                        {
                            graphicsManager.Id = graphicsManager1st.Id;
                            graphicsManager.Username = graphicsManager1st.Username;
                            responseFeedback.graphicsManager = graphicsManager;
                        }

                    }

                    FeedBackStaff filmingStaff = EFFeedbackStaffRepository.ListQuery(b => b.FeedBackId == feedback.Id && b.Type == 3 && b.IsManager == true && b.IsDeleted != true).SingleOrDefault();

                    if (filmingStaff != null)
                    {
                        User filmingManager1st = UsersBusiness.GetUserbyId(filmingStaff.UserId);
                        AllFeedBackUser filmingManager = new AllFeedBackUser();
                        if (filmingManager1st != null)
                        {
                            filmingManager.Id = filmingManager1st.Id;
                            filmingManager.Username = filmingManager1st.Username;
                            responseFeedback.filmingManager = filmingManager;
                        }
                    }

                    AllResponseCourseModel responseCourseModel = new AllResponseCourseModel();

                    if (feedback.CourseId != 0)
                    {
                        Course course = EFCourseRepository.GetById(b => b.Id == feedback.CourseId);

                        responseCourseModel.Id = int.Parse(course.Id.ToString());
                        responseCourseModel.Name = course.Name;

                    }

                    responseFeedback.Course = responseCourseModel;
                    responseFeedback.Lesson = _mapper.Map<Lesson, ResponseLessonModelByChapter>(EFLessonRepository.GetById(b => b.Id == feedback.LessonId));
                    return responseFeedback;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
        }

        public object GetAllFeedBackTemp(string Certificate)
        {
            List<FeedBackList> responseFeedbackList = new List<FeedBackList>();
            var feedbacklist = EFFeedback.GetAll().Select(s => new
            {
                s.Id,
                s.CreationTime,
                s.Status,
                s.Description
            }).ToList();
            foreach (var feedback in feedbacklist)
            {
                FeedBackList feedBackList = new FeedBackList();
                feedBackList.Id = feedback.Id;
                feedBackList.Creationdate = feedback.CreationTime;
                feedBackList.Status = feedback.Status;
                feedBackList.Description = feedback.Description;
                responseFeedbackList.Add(feedBackList);
            }

            return responseFeedbackList;
        }

        public List<AllResponseFeedback> GetAllFeedBack()
        {

            List<AllResponseFeedback> responseFeedbackList = new List<AllResponseFeedback>();
            List<Feedback> feedbacklist = EFFeedback.GetAll().OrderByDescending(b => b.Id).ToList();

            foreach (var feedback in feedbacklist)
            {
                responseFeedbackList.Add(GetFeedbackTmp(feedback.Id));
            }
            return responseFeedbackList;
        }

        public List<AllResponseFeedback> GetAllFeedBackForOtherUser(long id)
        {
            List<FeedBackStaff> feedBackStaffList = EFFeedbackStaffRepository.ListQuery(b => b.UserId == id && b.IsDeleted != true).ToList();
            List<AllResponseFeedback> responseFeedbackList = new List<AllResponseFeedback>();

            foreach (var feedback in feedBackStaffList)
            {
                responseFeedbackList.Add(GetFeedbackTmp(feedback.FeedBackId));
            }
            return responseFeedbackList;
        }


        public AllResponseFeedbackStatusWise GetAllFeedBacktest(FeedBackFilterModel fdbackmodel, List<string> rolename, long id)
        {
            AllResponseFeedbackStatusWise allResponseFeedbackStatusWise = new AllResponseFeedbackStatusWise();
            List<AllResponseFeedback> feedbacklist = new List<AllResponseFeedback>();

            if (rolename.Contains(LessonBusiness.getRoleType("1")) ||
               rolename.Contains(LessonBusiness.getRoleType("6")) ||
               rolename.Contains(LessonBusiness.getRoleType("7")) ||
               rolename.Contains(LessonBusiness.getRoleType("11"))
            )
            {
                feedbacklist = GetAllFeedBack();
            }
            if (rolename.Contains(LessonBusiness.getRoleType("12")))
            {
                feedbacklist = GetAllFeedBack();
                feedbacklist = feedbacklist.Where(b => b.Status == 1 || b.Status == 2).ToList();
            }
            if (rolename.Contains(LessonBusiness.getRoleType("14")) ||
               rolename.Contains(LessonBusiness.getRoleType("15")) ||
               rolename.Contains(LessonBusiness.getRoleType("16")) ||
               rolename.Contains(LessonBusiness.getRoleType("8")) ||
               rolename.Contains(LessonBusiness.getRoleType("9")) ||
               rolename.Contains(LessonBusiness.getRoleType("10"))
               )
            {
                feedbacklist = GetAllFeedBackForOtherUser(id);
            }


            List<AllResponseFeedback> newfeedbackList = new List<AllResponseFeedback>();
            List<AllResponseFeedback> completedfeedbackList = new List<AllResponseFeedback>();
            List<AllResponseFeedback> uploadedfeedbackList = new List<AllResponseFeedback>();
            List<AllResponseFeedback> filteredList = new List<AllResponseFeedback>();

            if (fdbackmodel.isfilterneeded == true)
            {
                //1 Equals 2 Similar
                switch (fdbackmodel.fltroptiongrade)
                {
                    case 1:
                        if (!string.IsNullOrEmpty(fdbackmodel.grade))
                        {
                            filteredList.AddRange((feedbacklist.Where(b => b.Grade.name == fdbackmodel.grade).ToList()).Distinct());
                        }
                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(fdbackmodel.grade))
                        {
                            filteredList.AddRange((feedbacklist.Where(b => b.Grade.name.Any(k => b.Grade.name.Contains(fdbackmodel.grade))).ToList()).Distinct());
                        }
                        break;
                }

                if (!string.IsNullOrEmpty(fdbackmodel.courseids))
                {
                    string[] courseids = fdbackmodel.courseids.Split(",").ToArray();
                    int cnt = courseids.Count();

                    if (cnt != 0)
                    {
                        for (int i = 0; i < cnt; i++)
                        {
                            if (filteredList.Count != 0)
                            {
                                filteredList = filteredList.Where(b => b.Course.Id == long.Parse(courseids[i])).ToList();
                            }
                            else
                            {
                                filteredList = feedbacklist.Where(b => b.Course.Id == long.Parse(courseids[i])).ToList();
                            }
                        }
                    }
                }

                switch (fdbackmodel.fltroptionenddate)
                {
                    case 1:
                        if (!string.IsNullOrEmpty(fdbackmodel.enddate))
                        {
                            DateTime dateTime = DateTime.Parse(fdbackmodel.enddate).ToUniversalTime();
                            filteredList.AddRange((feedbacklist.Where(b => b.Creationdate == dateTime.ToString()).ToList()).Distinct());
                        }
                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(fdbackmodel.enddate))
                        {
                            filteredList.AddRange((feedbacklist.Where(b => b.Creationdate.Any(k => b.Creationdate.Contains(fdbackmodel.enddate))).ToList()).Distinct());
                        }
                        break;
                }
            }
            else
            {
                filteredList = feedbacklist;
            }

            foreach (var feedback in filteredList)
            {
                switch (feedback.Status)
                {
                    case 0:
                        newfeedbackList.Add(feedback);
                        break;
                    case 1:
                        completedfeedbackList.Add(feedback);
                        break;
                    case 2:
                        uploadedfeedbackList.Add(feedback);
                        break;
                }
            }

            allResponseFeedbackStatusWise.newfeedback = newfeedbackList;
            allResponseFeedbackStatusWise.completedfeedback = completedfeedbackList;
            allResponseFeedbackStatusWise.uploadedfeedback = uploadedfeedbackList;

            return allResponseFeedbackStatusWise;
        }

        public Feedback CheckRecordExists(long fId, long userId, long type)
        {
            Feedback feedback = new Feedback();
            if (type == 4)
            {
                feedback = EFFeedback.GetById(b => b.Id == fId && b.QaUser == userId);
            }
            else
            {
                feedback = EFFeedback.GetById(b => b.Id == fId && b.Coordinator == userId);
            }
            return feedback;
        }

        public ResponseFeedbackStaff UpdateFeedBackManager(long fId, long userId, long type, long createdby, string Certificate)
        {
            Feedback feedback = new Feedback();
            ResponseFeedbackStaff responseFeedbackStaff = new ResponseFeedbackStaff();
            try
            {
                if (type == 4)
                {
                    feedback = EFFeedback.GetById(b => b.Id == fId);
                    feedback.QaUser = userId;
                    EFFeedback.Update(feedback);
                }
                else
                {
                    feedback = EFFeedback.GetById(b => b.Id == fId);
                    feedback.Coordinator = userId;
                    EFFeedback.Update(feedback);
                }
                FeedBackUser user = new FeedBackUser();
                User getUser = UsersBusiness.GetUserbyId(userId);
                List<Role> roles = UsersBusiness.Role(getUser);
                if (roles != null)
                {
                    List<long> roleids = new List<long>();
                    List<string> rolenames = new List<string>();
                    foreach (var role in roles)
                    {
                        roleids.Add(role.Id);
                        rolenames.Add(role.Name);
                    }
                    user.Roles = roleids;
                    user.RoleName = rolenames;
                }
                user.Id = getUser.Id;
                user.Username = getUser.Username;
                user.FullName = getUser.FullName;
                user.Email = user.Email;
                user.Bio = user.Bio;
                if (!string.IsNullOrEmpty(getUser.ProfilePicUrl))
                    user.profilepicurl = LessonBusiness.geturl(getUser.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
                responseFeedbackStaff.Id = userId;
                responseFeedbackStaff.CreationDate = feedback.CreationTime;
                responseFeedbackStaff.Type = type;
                responseFeedbackStaff.User = user;
                responseFeedbackStaff.IsManager = true;
                return responseFeedbackStaff;
            }
            catch
            {
                return null;
            }

        }
    }
}

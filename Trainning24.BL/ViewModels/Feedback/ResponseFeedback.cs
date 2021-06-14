using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Contact;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.FeedBackCategory;
using Trainning24.BL.ViewModels.FeedBackTask;
using Trainning24.BL.ViewModels.FeedbackTime;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Trainning24.BL.ViewModels.Feedback
{
    public class ResponseFeedback
    {
        public long Id { get; set; }
        public string Creationdate { get; set; }
        public ResponseContact Contact { get; set; }
        public ResponseFeedBackCategory Category { get; set; }
        public ResponseGradeModel Grade { get; set; }
        public ResponseCourseModel Course { get; set; }
        public ResponseLessonModelByChapter Lesson { get; set; }
        public ResponseChapter Chapter { get; set; }
        public int Status { get; set; }
        public FeedBackUser QaUser { get; set; }
        public FeedBackUser Coordinator { get; set; }
        public FeedBackUser filmingManager { get; set; }
        public FeedBackUser graphicsManager { get; set; }
        public FeedBackUser editingManager { get; set; }
        public List<FeedBackUser> filmingStaffs { get; set; }
        public List<FeedBackUser> graphicsStaffs { get; set; }
        public List<FeedBackUser> editingStaffs { get; set; }
        public List<ResponseFeedBackTaskModel> filmingtask { get; set; }
        public List<ResponseFeedBackTaskModel> graphicstask { get; set; }
        public List<ResponseFeedBackTaskModel> editingtask { get; set; }
        public List<ResponseFdTime> feedbacktime { get; set; }
        public string Description { get; set; }
    }


    public class AllResponseFeedback
    {
        public long Id { get; set; }
        public string Creationdate { get; set; }
        public AllResponseContact Contact { get; set; }
        public ResponseFeedBackCategory Category { get; set; }
        public AllResponseGradeModel Grade { get; set; }
        public AllResponseCourseModel Course { get; set; }
        //public LessonDetailModel Lesson { get; set; }
        public ResponseLessonModelByChapter Lesson { get; set; }
        public ResponseChapter Chapter { get; set; }
        public int Status { get; set; }
        public AllFeedBackUser QaUser { get; set; }
        public AllFeedBackUser Coordinator { get; set; }
        public AllFeedBackUser filmingManager { get; set; }
        public AllFeedBackUser graphicsManager { get; set; }
        public AllFeedBackUser editingManager { get; set; }
        public string Description { get; set; }
    }

    public class AllResponseFeedbackStatusWise
    {
        public List<AllResponseFeedback> newfeedback { get; set; }
        public List<AllResponseFeedback> completedfeedback { get; set; }
        public List<AllResponseFeedback> uploadedfeedback { get; set; }
    }

    public class ResponseFeedbackStaff
    {
        public long Id { get; set; }
        public string CreationDate { get; set; }
        public FeedBackUser User { get; set; }
        public long Type { get; set; }
        public bool IsManager { get; set; }
    }


    public class FeedBackList
    {
        public long Id { get; set; }
        public string Creationdate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
    }

    public class FeedbackStatusChangedModel
    {
        public long feedbackid { get; set; }
        public int status { get; set; }
    }
}

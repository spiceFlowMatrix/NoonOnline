using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.TaskFeedBack
{
    public class TaskFeedBackDetailsDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public TaskFeedBackCategoryDTO category { get; set; }
        public string startdate { get; set; }
        public string complateddate { get; set; }
        public string time { get; set; }
        public int status { get; set; }
        public BasicUserDTO user { get; set; }
        public string submitteddate { get; set; }
        public string archiveddate { get; set; }
        public string device { get; set; }
        public string version { get; set; }
        public string appversion { get; set; }
        public string operatingsystem { get; set; }
        public long assign { get; set; }
        public List<FeedBackFilesDTO> files { get; set; }
        public GradeDetailsDTO grade { get; set; }
        public CourseDetailsDTO course { get; set; }
        public ChapterDetailsDTO chapter { get; set; }
        public LessonDetailsDTO lesson { get; set; }
    }

    public class GradeDetailsDTO
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class CourseDetailsDTO
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class ChapterDetailsDTO
    {
        public long id { get; set; }
        public string name { get; set; }
    }

    public class LessonDetailsDTO
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}

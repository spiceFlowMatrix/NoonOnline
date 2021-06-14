using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.TaskFeedBack
{
    public class TaskFeedBackListDTO
    {
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public TaskFeedBackCategoryDTO category { get; set; }
        public string startdate { get; set; }
        public string complateddate { get; set; }
        public int status { get; set; }
        public BasicUserDTO user { get; set; }
        public string submitteddate { get; set; }
        public string archiveddate { get; set; }
    }

    public class TaskFeedBackCategoryDTO
    {
        public long id { get; set; }
        public string name { get; set; }
    }
}

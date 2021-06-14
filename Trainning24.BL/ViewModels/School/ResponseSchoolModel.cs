using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.School
{
    public class ResponseSchoolModel
    {
        public long id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string creationtime { get; set; }
        public AllFeedBackUser creatoruserid { get; set; }
        public string lastmodificationtime { get; set; }
        public AllFeedBackUser lastmodifieruserid { get; set; }
    }
}

using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class UserCourse : EntityBase, IUserCourse
    {        
        public long UserId { get; set; }
        public long CourseId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool? IsExpire { get; set; }
    }
}

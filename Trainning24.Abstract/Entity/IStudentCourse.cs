using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IUserCourse : IEntityBase
    {
        long UserId { get; set; }
        long CourseId { get; set; }
        string StartDate { get; set; }
        string EndDate { get; set; }
        bool? IsExpire { get; set; }
    }
}

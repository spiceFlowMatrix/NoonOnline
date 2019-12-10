using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.AssignmentStudent
{
    public class UpdateAssignmentStudentModel
    {
        public long id { get; set; }
        public long assignmentid { get; set; }
        public long studentid { get; set; }
        public long teacherid { get; set; }
    }
}

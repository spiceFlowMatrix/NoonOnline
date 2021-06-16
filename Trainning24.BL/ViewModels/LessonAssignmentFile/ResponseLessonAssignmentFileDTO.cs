using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.StudentLessonProgress;

namespace Trainning24.BL.ViewModels.LessonAssignmentFile
{
    public class ResponseLessonAssignmentFileDTO
    {
        public long id { get; set; }
        public ResponseFilesModel files { get; set; }
        public ResponseStudentLessonProgressModel progress { get; set; }
    }
}

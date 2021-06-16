using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.StudentLessonProgress;

namespace Trainning24.BL.ViewModels.LessonFile
{
    public class ResponseLessonFileModel
    {
        public long  id { get; set; }                     
        public ResponseFilesModel files { get; set; }
        public ResponseStudentLessonProgressModel progress { get; set; }
    }
}

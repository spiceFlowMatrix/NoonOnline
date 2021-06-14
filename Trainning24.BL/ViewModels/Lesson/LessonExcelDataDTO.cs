using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Lesson
{
    public class LessonExcelDataDTO
    {
        public string Lesson_Id { get; set; }
        public string FileId { get; set; }
        public string LessonIdsThatNeedsToBeDeleted { get; set; }
    }

    public class LessonFileExcelDTO
    {
        public long LessonId { get; set; }
        public long FileId { get; set; }
    }

    public class LessonDeleteExcelDTO
    {
        public long LessonId { get; set; }
    }
}

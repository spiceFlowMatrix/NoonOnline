using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Files;

namespace Trainning24.BL.ViewModels.AssignmentSubmission
{
    public class SubmissionFileDTO
    {
        public long id { get; set; }
        public ResponseFilesModel files { get; set; }
    }
}

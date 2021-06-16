using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Files;

namespace Trainning24.BL.ViewModels.AssignmentFile
{
    public class ResponseAssignmentFileModel
    {
        public long id { get; set; }
        public ResponseFilesModel files { get; set; }
    }
}

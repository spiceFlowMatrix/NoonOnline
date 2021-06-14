using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Assignment
{
    public class UpdateAssignmentModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public long chapterid { get; set; }
        public List<int> fileid { get; set; }
    }
}

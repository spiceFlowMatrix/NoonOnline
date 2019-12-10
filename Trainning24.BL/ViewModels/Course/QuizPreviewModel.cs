using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Course
{
    public class QuizPreviewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int numquestions { get; set; }
        public  int itemorder { get; set; }
        public int type { get; set; }
    }
}

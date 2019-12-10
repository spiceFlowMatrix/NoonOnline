using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.School;

namespace Trainning24.BL.ViewModels.Grade
{
    public class ResponseGradeModel
    {
        public long id { get; set; }
        public string name { set; get; }
        public string description { set; get; }        
    }


    public class ResponseGradeModel1
    {
        public long id { get; set; }
        public string name { set; get; }
        public string description { set; get; }
        public ResponseSchoolModel school { get; set; }
    }


    public class AllResponseGradeModel
    {
        public long id { get; set; }
        public string name { set; get; }        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Course
{
    public class AddCourseModel
    {
        public AddCourseModel() {
            bundleId = 0;
            passmark = 0;
        }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public string image { get; set; }        
        public decimal passmark { get; set; }
        public long gradeid { get; set; }
        public long bundleId { get; set; }
        public bool istrial { get; set; }
    }
}

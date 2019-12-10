using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Bundle
{
    public class AddBundleModel
    {        

        [Required(AllowEmptyStrings = false, ErrorMessage = "BundleNameIsRequired")]
        public string Name { get; set; }
        //public long CourseId { get; set; }
    }
}

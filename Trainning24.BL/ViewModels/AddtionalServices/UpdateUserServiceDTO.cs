using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.AddtionalServices
{
    public class UpdateUserServiceDTO
    {
        public long Id { get; set; }
        public bool is_discussion_authorized { get; set; }
        public bool is_library_authorized { get; set; }
        public bool is_assignment_authorized { get; set; }
    }
}

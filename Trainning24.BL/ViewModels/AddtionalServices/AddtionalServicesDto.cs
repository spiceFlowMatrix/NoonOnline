using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.AddtionalServices
{
    public class AddtionalServicesDto
    {
        public long id { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public string createdDate { get; set; }
        public long createdBy { get; set; }
    }
}

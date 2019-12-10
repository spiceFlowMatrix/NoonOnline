using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Contact
{
    public class ResponseContact
    {
        public long id { get; set; }
        public string FirstName { get; set; }
        public string Email { get; set; }
        public string CountryCode { get; set; }
        public string Phone { get; set; }
    }


    public class AllResponseContact
    {
        public long id { get; set; }
        public string FirstName { get; set; }
    }
}

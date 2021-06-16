using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Domain.Entity
{
    public class Contact : EntityBase
    {
        public string FirstName { get; set; }
        public string Email { get; set; }        
        public string CountryCode { get; set; }
        public string Phone { get; set; }
    }
}

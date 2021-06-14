using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Teacher //: EntityBase, ITeacher
    {

        public long Id { get; set; }
        public string Bio { get; set; }
        public string PhotoUrl { get; set; }
        public string Designation { get; set; }
        public string FullName { get; set; }

        public Teacher()
        {
        }
    }
}

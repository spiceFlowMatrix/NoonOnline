using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.UserRole;

namespace Trainning24.BL.ViewModels.Users
{
    public class CreateTrialUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
        public string StudentName { get; set; }
        public string FatherName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Village { get; set; }
        public string SchoolName { get; set; }
        public long GradeId { get; set; }
        public string TazkiraNo { get; set; }
        public string Phone { get; set; }
        public string SoicalMediaLinked { get; set; }
        public string SocialMediaAccount { get; set; }
        public string Reference { get; set; }
        public long UserId { get; set; }

        // default fields needs to be update on create user
        public bool is_skippable { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
        public int intervals { get; set; }
        public string authid { get; set; }
        public bool is_discussion_authorized { get; set; }
        public bool is_library_authorized { get; set; }
        public bool is_assignment_authorized { get; set; }

        // default studen role on create user
        public List<AddUserRoleModel> Roles { get; set; }
    }
}

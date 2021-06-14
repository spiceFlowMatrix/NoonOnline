using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Trainning24.BL.ViewModels.UserRole;

namespace Trainning24.BL.ViewModels.SalesAgent
{
    public class CreateSalesAgentDTO
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "PasswordIsRequired")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ConfirmPasswordIsRequired")]
        public string ConfirmPassword { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RoleIIsRequired")]
        public long RoleId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RoleIIsRequired")]
        public List<AddUserRoleModel> Roles { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "EmailIsRequired")]
        public string Email { get; set; }

        public bool is_skippable { get; set; }

        public int timeout { get; set; }

        public int reminder { get; set; }

        public bool istimeouton { get; set; }

        public int intervals { get; set; }

        public string phonenumber { get; set; }

        public string authid { get; set; }

        public bool is_discussion_authorized { get; set; }

        public bool is_library_authorized { get; set; }

        public bool is_assignment_authorized { get; set; }

        //Fields added for sales agents
        public long salesagentId { get; set; }
        public long agentCategoryId { get; set; }
        public int agreedMonthlyDeposit { get; set; }
        public string currencyCode { get; set; }
        public string focalPoint { get; set; }
        public string partnerBackgroud { get; set; }
        public string physicalAddress { get; set; }
        public string position { get; set; }
        public bool isactive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training24Admin.Model
{
    public class DetailUserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public List<string> RoleName { get; set; }
        public string Email { get; set; }
        public List<long> Roles { get; set; }
        public string Bio { get; set; }
        public string profilepicurl { get; set; }
        public bool is_skippable { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public int intervals { get; set; }
        public bool istimeouton { get; set; }
        public string phonenumber { get; set; }
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
        public bool isallowmap { get; set; }
    }

    public class GetUserList
    {
        public long Id { get; set; }
        public string Username { get; set; }
    }

    public class UserDetailRoleWise
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public List<DetailUserRole> Details { get; set; }
    }

    public class DetailUserRole
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool is_skippable { get; set; }
        public List<string> RoleName { get; set; }
        public List<long> Roles { get; set; }
        public int timeout { get; set; }
        public int reminder { get; set; }
        public bool istimeouton { get; set; }
        public string phonenumber { get; set; }
    }
}

using System;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using Trainning24.Abstract.Entity;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using Trainning24.BL.ViewModels.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Trainning24.BL.ViewModels.UserRole;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Trainning24.BL.ViewModels.AddtionalServices;
using Trainning24.BL.ViewModels.Erp;

namespace Trainning24.BL.Business
{
    public class UsersBusiness
    {
        private readonly EFUsersRepository _EFUsersRepository;
        private static EFStudentCourseRepository _EFStudentCourseRepository;
        private readonly EFCourseRepository _EFCourseRepository;
        private readonly LessonBusiness _lessonBusiness;
        public EmailSettings _emailSettings { get; }
        public ErpSettings _erpSettings { get; }
        public readonly EFStudParentRepository _eFStudParentRepository;
        private readonly LogObjectBusiness _logObjectBusiness;
        private readonly EFUserRoleRepository _eFUserRoleRepository;
        private readonly EFRoleRepository _eFRoleRepository;


        public UsersBusiness
        (
            EFUsersRepository eFUsersRepository,
            EFStudentCourseRepository EFStudentCourseRepository,
            EFCourseRepository EFCourseRepository,
            IOptions<EmailSettings> emailSettings,
            IOptions<ErpSettings> erpSettings,
            LessonBusiness lessonBusiness,
            LogObjectBusiness logObjectBusiness,
            EFStudParentRepository eFStudParentRepository,
            EFUserRoleRepository eFUserRoleRepository,
            EFRoleRepository eFRoleRepository
        )
        {
            _EFUsersRepository = eFUsersRepository;
            _EFStudentCourseRepository = EFStudentCourseRepository;
            _EFCourseRepository = EFCourseRepository;
            _emailSettings = emailSettings.Value;
            _erpSettings = erpSettings.Value;
            _lessonBusiness = lessonBusiness;
            _logObjectBusiness = logObjectBusiness;
            _eFStudParentRepository = eFStudParentRepository;
            _eFUserRoleRepository = eFUserRoleRepository;
            _eFRoleRepository = eFRoleRepository;
        }


        //public List<string> getrolelist(List<AddUserRoleModel> RoleList)
        public string getrolelist(List<AddUserRoleModel> RoleList)
        {
            string role = "";
            foreach (var rl in RoleList)
            {
                switch (rl.roleid)
                {
                    case 1:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "admin" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "admin" + '"';
                        }
                        break;
                    case 2:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "analyst" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "analyst" + '"';
                        }
                        break;
                    case 3:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "teacher" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "teacher" + '"';
                        }
                        break;
                    case 4:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "student" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "student" + '"';
                        }
                        break;
                    case 5:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "customer" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "customer" + '"';
                        }
                        break;
                    case 6:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "aafmanager" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "aafmanager" + '"';
                        }
                        break;
                    case 7:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "coordinator" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "coordinator" + '"';
                        }
                        break;
                    case 8:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "edit_team_leader" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "edit_team_leader" + '"';
                        }
                        break;
                    case 9:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "shooting_team_leader" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "shooting_team_leader" + '"';
                        }
                        break;
                    case 10:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "graphics_team_leader" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "graphics_team_leader" + '"';
                        }
                        break;
                    case 11:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "quality_assurance" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "quality_assurance" + '"';
                        }
                        break;
                    case 12:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "feedback_edge_team" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "feedback_edge_team" + '"';
                        }
                        break;
                    case 13:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "sales_admin" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "sales_admin" + '"';
                        }
                        break;
                    case 14:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "filming_staff" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "filming_staff" + '"';
                        }
                        break;
                    case 15:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "editing_staff" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "editing_staff" + '"';
                        }
                        break;
                    case 16:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "graphics_staff" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "graphics_staff" + '"';
                        }
                        break;
                    case 17:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "sales_agent" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "sales_agent" + '"';
                        }
                        break;
                    case 18:
                        if (string.IsNullOrEmpty(role))
                        {
                            role = '"' + "parent" + '"';
                        }
                        else
                        {
                            role = role + "," + '"' + "parent" + '"';
                        }
                        break;
                };
            }
            return role;
        }

        public User blocked(long id)
        {
            User user = _EFUsersRepository.GetById(b => b.Id == id);
            user.IsBlocked = true;
            _EFUsersRepository.BlockUser(user);
            return user;
        }

        public List<UserRole> getUserRole(long id)
        {
            return _eFUserRoleRepository.ListQuery(b => b.RoleId == id).ToList();
        }

        public User GetUserbyId(long id)
        {
            return _EFUsersRepository.ListQuery(b => b.Id == id).FirstOrDefault();
        }

        public User GetByAuthId(string authid)
        {
            return _EFUsersRepository.GetById(b => b.AuthId == authid && b.IsDeleted != true);
        }

        public async Task<User> Create(CreateUserViewModel createUserViewModel, string Id)
        {
            var hashedPassword = Crypto.Hash(createUserViewModel.Password, "MD5");
            User newUser = new User
            {
                Username = string.IsNullOrEmpty(createUserViewModel.Username) ? "" : createUserViewModel.Username,
                FullName = string.IsNullOrEmpty(createUserViewModel.FullName) ? "" : createUserViewModel.FullName,
                Password = hashedPassword,
                Email = createUserViewModel.Email,
                is_skippable = createUserViewModel.is_skippable,
                timeout = createUserViewModel.timeout,
                reminder = createUserViewModel.reminder,
                intervals = createUserViewModel.intervals,
                istimeouton = createUserViewModel.istimeouton,
                phonenumber = string.IsNullOrEmpty(createUserViewModel.phonenumber) ? "" : createUserViewModel.phonenumber,
                AuthId = createUserViewModel.authid,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 1,//int.Parse(Id),
                IsDeleted = false,
                is_discussion_authorized = createUserViewModel.is_discussion_authorized,
                is_assignment_authorized = createUserViewModel.is_assignment_authorized,
                is_library_authorized = createUserViewModel.is_library_authorized,
                isallowmap = createUserViewModel.isallowmap
            };
            _EFUsersRepository.Insert(newUser);
            _logObjectBusiness.AddLogsObject(4, newUser.Id, newUser.CreatorUserId ?? 1);
            foreach (var userrole in createUserViewModel.Roles)
            {
                UserRole userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = userrole.roleid,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = 1,//int.Parse(Id),
                    IsDeleted = false
                };
                _eFUserRoleRepository.Insert(userRole);
            }
            return newUser;
        }


        public User UpdateAuthId(UpdateUserViewModel createUserViewModel)
        {
            User user = _EFUsersRepository.GetById(b => b.Id == createUserViewModel.Id);
            user.AuthId = createUserViewModel.authid;
            _EFUsersRepository.Update(user);
            return user;
        }


        public User UpdateIsfirstlogin(UpdateUserViewModel createUserViewModel)
        {
            User user = _EFUsersRepository.GetById(b => b.Id == createUserViewModel.Id);
            user.isfirstlogin = createUserViewModel.isfirstlogin;
            _EFUsersRepository.Update(user);
            return user;
        }

        public User Update(UpdateUserViewModel createUserViewModel, string Id)
        {
            var hashedPassword = "";
            if (!string.IsNullOrEmpty(createUserViewModel.Password))
            {
                hashedPassword = Crypto.Hash(createUserViewModel.Password, "MD5");
            }

            User newUser = new User
            {
                Id = createUserViewModel.Id,
                Username = createUserViewModel.Username,
                FullName = createUserViewModel.FullName,
                timeout = createUserViewModel.timeout,
                reminder = createUserViewModel.reminder,
                intervals = createUserViewModel.intervals,
                istimeouton = createUserViewModel.istimeouton,
                Password = hashedPassword,
                is_skippable = createUserViewModel.is_skippable,
                Email = createUserViewModel.Email,
                phonenumber = createUserViewModel.phonenumber,
                IsDeleted = false,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = int.Parse(Id),
                is_discussion_authorized = createUserViewModel.is_discussion_authorized,
                is_assignment_authorized = createUserViewModel.is_assignment_authorized,
                is_library_authorized = createUserViewModel.is_library_authorized,
                isallowmap = createUserViewModel.isallowmap
            };

            _EFUsersRepository.Update(newUser);
            _logObjectBusiness.AddLogsObject(5, newUser.Id, newUser.CreatorUserId ?? 1);
            List<UserRole> existUserRoles = _eFUserRoleRepository.ListQuery(b => b.UserId == newUser.Id).ToList();
            if (existUserRoles != null)
            {
                foreach (var existingUserRole in existUserRoles)
                {
                    _eFUserRoleRepository.DeleteUserRole(existingUserRole);
                }
            }
            foreach (var userrole in createUserViewModel.Roles)
            {
                UserRole userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = userrole.roleid,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = int.Parse(Id),
                    IsDeleted = false
                };
                _eFUserRoleRepository.Insert(userRole);
            }

            return newUser;
        }

        public User UpdateSalesAgentUser(CreateUserViewModel createUserViewModel, string Id)
        {
            var hashedPassword = "";
            if (!string.IsNullOrEmpty(createUserViewModel.Password))
            {
                hashedPassword = Crypto.Hash(createUserViewModel.Password, "MD5");
            }

            User newUser = new User
            {
                Id = createUserViewModel.Id,
                Username = createUserViewModel.Username,
                FullName = createUserViewModel.FullName,
                timeout = createUserViewModel.timeout,
                reminder = createUserViewModel.reminder,
                intervals = createUserViewModel.intervals,
                istimeouton = createUserViewModel.istimeouton,
                Password = hashedPassword,
                is_skippable = createUserViewModel.is_skippable,
                Email = createUserViewModel.Email,
                phonenumber = createUserViewModel.phonenumber,
                IsDeleted = false,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = int.Parse(Id),
                is_discussion_authorized = createUserViewModel.is_discussion_authorized,
                is_assignment_authorized = createUserViewModel.is_assignment_authorized,
                is_library_authorized = createUserViewModel.is_library_authorized
            };

            _EFUsersRepository.Update(newUser);
            _logObjectBusiness.AddLogsObject(5, newUser.Id, newUser.CreatorUserId ?? 1);
            List<UserRole> existUserRoles = _eFUserRoleRepository.ListQuery(b => b.UserId == newUser.Id && b.RoleId == 17).ToList();
            if (existUserRoles != null)
            {
                foreach (var existingUserRole in existUserRoles)
                {
                    _eFUserRoleRepository.DeleteUserRole(existingUserRole);
                }
            }
            foreach (var userrole in createUserViewModel.Roles)
            {
                UserRole userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = userrole.roleid,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = int.Parse(Id),
                    IsDeleted = false
                };
                _eFUserRoleRepository.Insert(userRole);
            }

            return newUser;
        }

        public User Delete(User createUserViewModel, string Id)
        {
            _EFUsersRepository.Delete(createUserViewModel, Id);
            _logObjectBusiness.AddLogsObject(6, createUserViewModel.Id, createUserViewModel.DeleterUserId ?? 1);
            return createUserViewModel;
        }

        public User Authentication(UserAuthenticationModel userAuthenticationModel, string auth)
        {

            string hashedPassword = "";
            if (!string.IsNullOrEmpty(userAuthenticationModel.Password))
            {
                hashedPassword = Crypto.Hash(userAuthenticationModel.Password, "MD5");
            }

            User _user = new User
            {
                Email = string.IsNullOrEmpty(userAuthenticationModel.Email) ? "" : userAuthenticationModel.Email,
                Password = hashedPassword,
                AuthId = auth
            };

            return _EFUsersRepository.Auth(_user);
        }


        public User AuthenticationByAuth(string auth)
        {
            User _user = new User
            {
                AuthId = auth
            };

            return _EFUsersRepository.Auth(_user);
        }


        public User UserExistance(CreateUserViewModel createUserViewModel)
        {
            User _user = new User
            {
                Email = createUserViewModel.Email
            };

            return _EFUsersRepository.UserExist(_user);
        }

        public User UserExistanceUpdate(UpdateUserViewModel updateUserViewModel)
        {
            User _user = new User
            {
                Email = updateUserViewModel.Email
            };

            return _EFUsersRepository.UpdateUserExist(_user); ;
        }

        public User UserExistanceById(CreateUserViewModel createUserViewModel)
        {

            User _user = new User
            {
                Id = createUserViewModel.Id
            };

            return _EFUsersRepository.UserExistById(_user); ;
        }

        public List<Role> Role(User user)
        {
            List<UserRole> roles = _eFUserRoleRepository.ListQuery(b => b.UserId == user.Id).ToList();
            List<Role> roleList = new List<Role>();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    Role role1st = _eFRoleRepository.GetById(b => b.Id == role.RoleId);
                    roleList.Add(role1st);
                }
            }

            return roleList;
        }

        public async Task<List<Role>> RoleAsync(User user)
        {
            List<UserRole> roles = await _eFUserRoleRepository.ListQueryAsync(b => b.UserId == user.Id);
            List<Role> roleList = new List<Role>();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    Role role1st = await _eFRoleRepository.GetByIdAsync(b => b.Id == role.RoleId);
                    roleList.Add(role1st);
                }
            }

            return roleList;
        }

        public Role GetRole(long id)
        {
            return _eFRoleRepository.GetById(b => b.Id == id);
        }

        public List<Role> RoleList()
        {
            return _eFRoleRepository.GetAll();
        }

        public int TotalUserCount()
        {
            return _EFUsersRepository.GetAll().Count();
        }

        public bool IsvalidUsername(string username, int Id, long userid)
        {
            if (string.IsNullOrEmpty(username))
            {
                return true;
            }
            User user = _EFUsersRepository.UserExistByUsername(username);
            if (user == null)
            {
                return true;
            }
            else
            {
                if (user.Id == Id || user.Id == userid)
                    return true;
                else
                    return false;
            }
        }

        public List<AssignedPersonModel> GetAssignedPersonDetails(long id, List<String> rolename, PaginationModel paginationModel, out int total)
        {
            List<AssignedPersonModel> allAssigned = new List<AssignedPersonModel>();
            var usercourses = _EFStudentCourseRepository.ListQuery(b => b.UserId == id).ToList();
            foreach (var usercourse in usercourses)
            {
                List<UserDetails> users = new List<UserDetails>();
                Course userCourseDetail = _EFCourseRepository.GetById(b => b.Id == usercourse.CourseId);

                var usersList = _EFUsersRepository.GetAll();
                var userCourseList = _EFStudentCourseRepository.GetAll();
                var userrolelist = _eFUserRoleRepository.GetAll();
                List<User> allUsers = new List<User>();
                if (rolename.Contains(LessonBusiness.getRoleType("3")))
                {
                    allUsers = (from user in usersList
                                join userCourse in userCourseList on user.Id equals userCourse.UserId
                                where userCourse.CourseId == usercourse.CourseId && user.Id != id
                                select user
                                             ).ToList();
                }


                if (rolename.Contains(LessonBusiness.getRoleType("4")))
                {
                    allUsers = (
                                from user in usersList
                                join userCourse in userCourseList on user.Id equals userCourse.UserId
                                join thatsrole in userrolelist on user.Id equals thatsrole.UserId
                                where userCourse.CourseId == usercourse.CourseId && thatsrole.RoleId == 3
                                select user
                               ).ToList();
                }

                foreach (var student in allUsers)
                {
                    users.Add(new UserDetails
                    {
                        bio = student.Bio,
                        email = student.Email,
                        is_skippable = student.is_skippable,
                        fullname = student.FullName,
                        id = student.Id,
                        profilepicurl = student.ProfilePicUrl,
                        username = student.Username,
                        phonenumber = student.phonenumber
                    });
                }

                allAssigned.Add(new AssignedPersonModel
                {
                    id = userCourseDetail.Id,
                    code = userCourseDetail.Code,
                    description = userCourseDetail.Description,
                    name = userCourseDetail.Name,
                    image = userCourseDetail.Image,
                    UserDetails = users
                });
            }
            total = allAssigned.Count();

            if (!string.IsNullOrEmpty(paginationModel.search))
                allAssigned = allAssigned.Where(b => b.id.ToString().Any(k => b.id.ToString().Contains(paginationModel.search)
                                                            || b.name.ToLower().Contains(paginationModel.search.ToLower())
                                                            || b.code.ToLower().Contains(paginationModel.search.ToLower())
                                                            || b.description.ToLower().Contains(paginationModel.search.ToLower())
                                                            )).ToList();



            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {

                allAssigned = allAssigned.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                Take(paginationModel.perpagerecord).
                ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    allAssigned = allAssigned.Where(b => b.id.ToString().Any(k => b.id.ToString().Contains(paginationModel.search)
                                                                || b.name.ToLower().Contains(paginationModel.search.ToLower())
                                                                || b.code.ToLower().Contains(paginationModel.search.ToLower())
                                                                || b.description.ToLower().Contains(paginationModel.search.ToLower())
                                                                )).ToList();
            }

            return allAssigned;
        }

        public UserStatistics GetStatistics(string rolename, int userid)
        {
            UserStatistics userStatistics = new UserStatistics();
            try
            {
                userStatistics.unreadnotifications = 5;
                userStatistics.avragequizscore = 13;
                userStatistics.avrageassignmentscore = 15;
                userStatistics.complatecourse = 10;
                var totalavailablescore = _EFStudentCourseRepository.ListQuery(b => b.UserId == userid && b.IsDeleted != true).Count();
                userStatistics.totalavailablescore = totalavailablescore;
                var terminatedcourse = _EFStudentCourseRepository.ListQuery(b => b.UserId == userid && b.IsDeleted != true).ToList();
                if (terminatedcourse != null)
                {
                    var tcCount = 0;
                    foreach (var tc in terminatedcourse)
                    {
                        if (!string.IsNullOrEmpty(tc.EndDate))
                        {
                            DateTime parsedDate = DateTime.ParseExact(tc.EndDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            if (parsedDate <= DateTime.Now)
                            {
                                tcCount++;
                            }
                        }
                    }
                    userStatistics.terminatedcourse = tcCount;
                }
                var totalcourse = _EFCourseRepository.GetAll().Count();
                userStatistics.totalcourse = totalcourse;
                return userStatistics;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public User UserExistsByEmail(string email)
        {
            return _EFUsersRepository.ListQuery(b => b.Email == email && b.IsDeleted != true).FirstOrDefault();
        }

        public async Task<User> UserExistsByEmailAsync(string email)
        {
            try
            {
                return await _EFUsersRepository.GetByIdAsync(b => b.Email == email && b.IsDeleted != true);
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }

        public int updateForgotCode(User user)
        {
            return _EFUsersRepository.Update(user);
        }

        public User CheckForgotKey(string key)
        {
            return _EFUsersRepository.ListQuery(b => b.forgotkey == key && b.isforgot == true).FirstOrDefault();
        }
        public Task SendEmailAsync(string email, string subject, string message)
        {

            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }
        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Noon Online Education System")
                };
                mail.To.Add(new MailAddress(toEmail));

                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<long> GetTeachersByCourseId(long courseid)
        {
            var usersList = _EFUsersRepository.GetAll();
            var userCourseList = _EFStudentCourseRepository.GetAll();
            var userrolelist = _eFUserRoleRepository.GetAll();

            var obj = (from uc in userCourseList
                       join ul in usersList on uc.UserId equals ul.Id
                       join ur in userrolelist on ul.Id equals ur.UserId
                       where uc.CourseId == courseid && uc.DeletionTime == null && ur.RoleId == 3
                       select ul.Id).Distinct().ToList();
            return obj;
        }

        public List<long> GetStudentsByCourseId(long courseid)
        {
            var usersList = _EFUsersRepository.GetAll();
            var userCourseList = _EFStudentCourseRepository.GetAll();
            var userrolelist = _eFUserRoleRepository.GetAll();

            var obj = (from uc in userCourseList
                       join ul in usersList on uc.UserId equals ul.Id
                       join ur in userrolelist on ul.Id equals ur.UserId
                       where uc.CourseId == courseid && uc.DeletionTime == null && ur.RoleId == 4 && ul.is_discussion_authorized == true
                       select ul.Id).Distinct().ToList();

            return obj;
        }

        public List<long> GetUserByCourseId(long courseid)
        {
            List<long> userlist = _EFStudentCourseRepository.ListQuery(b => b.CourseId == courseid && b.DeletionTime == null).Select(u => u.UserId).ToList();
            return userlist;
        }

        public UserDetails GetNotificationUserById(long id, string certificate)
        {
            UserDetails userDetails = new UserDetails();
            User user = _EFUsersRepository.ListQuery(b => b.Id == id).FirstOrDefault();
            if (user != null)
            {
                userDetails.id = user.Id;
                userDetails.username = user.Username;
                userDetails.fullname = user.FullName;
                Role role = GetRole(user.Id);
                if (role != null)
                {
                    userDetails.roleid = role.Id;
                    userDetails.rolename = role.Name;
                }
                userDetails.email = user.Email;
                userDetails.bio = user.Bio;
                if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                    userDetails.profilepicurl = _lessonBusiness.geturl(user.ProfilePicUrl, certificate);
                userDetails.is_skippable = user.is_skippable;
                userDetails.phonenumber = user.phonenumber;
            }
            return userDetails;
        }

        public object GetUserList(PaginationModel2 paginationModel, out int total)
        {
            var userList = _EFUsersRepository.ListQuery(u => u.IsDeleted != true).ToList();
            var userrole = _eFUserRoleRepository.GetAll();

            var userListwithp = (from ul in userList
                                 join ur in userrole on ul.Id equals ur.UserId
                                 where ur.RoleId != 1 && ul.IsDeleted != true
                                 select new { ul.Id, ul.FullName, ul.Email, ul.Username, ul.phonenumber }).Distinct().OrderBy(o => o.Id).ToList();
            try
            {
                if (paginationModel.roleid != null)
                {
                    foreach (var role in paginationModel.roleid)
                    {
                        userListwithp = (from ul in userList
                                         join ur in userrole on ul.Id equals ur.UserId
                                         where ur.RoleId != 1 && ul.IsDeleted != true && ur.RoleId == Convert.ToInt32(role)
                                         select new { ul.Id, ul.FullName, ul.Email, ul.Username, ul.phonenumber }).Distinct().OrderBy(o => o.Id).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(paginationModel.search) && userListwithp != null)
                    userListwithp = userListwithp.OrderByDescending(b => b.Id).Where(
                                       b => b.Id.ToString().Contains(paginationModel.search)
                                    || !string.IsNullOrEmpty(b.Username) && b.Username.ToLower().Contains(paginationModel.search.ToLower())
                                    || !string.IsNullOrEmpty(b.Email) && b.Email.ToLower().Contains(paginationModel.search.ToLower())
                                    ).ToList();

                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                {
                    total = userListwithp == null ? 0 : userListwithp.Count();
                    userListwithp = userListwithp.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                }
                total = userListwithp == null ? 0 : userListwithp.Count();
                return userListwithp;
            }
            catch (Exception ex)
            {
                total = userListwithp == null ? 0 : userListwithp.Count();
                return userListwithp;
            }

        }

        public List<User> UserList(PaginationModel2 paginationModel, out int total)
        {
            List<User> userList = new List<User>();
            var users = _EFUsersRepository.ListQuery(u => u.IsDeleted != true).ToList();
            var userrole = _eFUserRoleRepository.GetAll();
            try
            {
                if (paginationModel.roleid != null)
                {
                    foreach (var role in paginationModel.roleid)
                    {
                        var user = (from ul in users
                                    join ur in userrole on ul.Id equals ur.UserId
                                    where ul.IsDeleted != true && ur.RoleId == Convert.ToInt64(role)
                                    select ul).Distinct().OrderBy(b => b.Id).ToList();
                        userList.AddRange(user);
                    }
                }
                else
                {
                    userList = (from ul in users
                                join ur in userrole on ul.Id equals ur.UserId
                                where ur.RoleId != 1 && ul.IsDeleted != true
                                select ul).Distinct().OrderBy(o => o.Id).ToList();
                }

                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    userList = userList.Where(
                    b => b.Id.ToString().Contains(paginationModel.search)
                    || !string.IsNullOrEmpty(b.Username) && b.Username.ToLower().Contains(paginationModel.search.ToLower())
                    || !string.IsNullOrEmpty(b.Email) && b.Email.ToLower().Contains(paginationModel.search.ToLower())
                    ).OrderByDescending(b => b.Id).ToList();
                }

                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                {
                    userList = userList.OrderByDescending(b => b.CreationTime).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                }
                total = users.Count();
                return userList;
            }
            catch (Exception ex)
            {
                total = userList.Count();
                return userList;
            }
        }

        public User UpdateAdditionalService(UpdateUserServiceDTO obj)
        {
            User newUser = new User
            {
                Id = obj.Id,
                is_discussion_authorized = obj.is_discussion_authorized,
                is_assignment_authorized = obj.is_assignment_authorized,
                is_library_authorized = obj.is_library_authorized
            };
            _EFUsersRepository.UpdateAdditionalService(newUser);
            return newUser;
        }

        public int RemoveUserRole(long userId, long roleId)
        {
            try
            {
                List<UserRole> existUserRoles = _eFUserRoleRepository.ListQuery(b => b.UserId == userId && b.RoleId == roleId).ToList();
                if (existUserRoles != null)
                {
                    foreach (var existingUserRole in existUserRoles)
                    {
                        _eFUserRoleRepository.DeleteUserRole(existingUserRole);
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<User> CreateTrialUser(CreateTrialUser dto)
        {
            var hashedPassword = Crypto.Hash(dto.Password, "MD5");
            User newUser = new User
            {
                FullName = dto.StudentName,
                Password = hashedPassword,
                Email = dto.Email,
                is_skippable = dto.is_skippable,
                timeout = dto.timeout,
                reminder = dto.reminder,
                intervals = dto.intervals,
                istimeouton = dto.istimeouton,
                phonenumber = dto.Phone,
                AuthId = dto.authid,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = 1,
                IsDeleted = false,
                is_discussion_authorized = dto.is_discussion_authorized,
                is_assignment_authorized = dto.is_assignment_authorized,
                is_library_authorized = dto.is_library_authorized,
                istrial = true
            };
            await _EFUsersRepository.InsertAsync(newUser);
            _logObjectBusiness.AddLogsObject(4, newUser.Id, newUser.CreatorUserId ?? 1);
            foreach (var userrole in dto.Roles)
            {
                UserRole userRole = new UserRole
                {
                    UserId = newUser.Id,
                    RoleId = userrole.roleid,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = 1,//int.Parse(Id),
                    IsDeleted = false
                };
                await _eFUserRoleRepository.InsertAsync(userRole);
            }

            return newUser;
        }

        public object GetErpDetails()
        {
            return _erpSettings;
        }

        public BasicUserDTO GetUserBasicDetails(long TeacherId, long StudentId, string Certificate)
        {
            User _getuser = new User();
            if (TeacherId != 0)
            {
                _getuser = _EFUsersRepository.GetById(Convert.ToInt32(TeacherId));
            }
            else
            {
                _getuser = _EFUsersRepository.GetById(Convert.ToInt32(StudentId));
            }
            BasicUserDTO basicUserDTO = new BasicUserDTO();
            if (_getuser != null)
            {
                basicUserDTO.id = _getuser.Id;
                basicUserDTO.username = _getuser.Username;
                basicUserDTO.fullname = _getuser.FullName;
                if (!string.IsNullOrEmpty(_getuser.ProfilePicUrl))
                    basicUserDTO.profilepicurl = _lessonBusiness.geturl(_getuser.ProfilePicUrl, Certificate);
            }
            return basicUserDTO;
        }

        public List<StudParent> GetStudParentById(long studentId)
        {
            return _eFStudParentRepository.ListQuery(b => b.StudentId == studentId && b.DeletionTime == null).ToList();
        }

        public int RemoveParent(long studentId, long parentId)
        {
            try
            {
                var _user = _eFStudParentRepository.GetById(b => b.StudentId == studentId && b.ParentId == parentId && b.DeletionTime == null);
                if (_user != null)
                {
                    _user.DeletionTime = DateTime.UtcNow.ToString();
                    _user.IsDeleted = true;
                    _eFStudParentRepository.Update(_user);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public BasicUserDTO GetUsersDetails(long UserId, string Certificate)
        {
            User _getuser = _EFUsersRepository.GetById(Convert.ToInt32(UserId));
            BasicUserDTO basicUserDTO = new BasicUserDTO();
            if (_getuser != null)
            {
                basicUserDTO.id = _getuser.Id;
                basicUserDTO.username = _getuser.Username;
                basicUserDTO.fullname = _getuser.FullName;
                if (!string.IsNullOrEmpty(_getuser.ProfilePicUrl))
                    basicUserDTO.profilepicurl = _lessonBusiness.geturl(_getuser.ProfilePicUrl, Certificate);
            }
            return basicUserDTO;
        }
    }
}

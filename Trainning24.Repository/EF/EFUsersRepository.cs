using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Abstract.Infrastructure.IRepository;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFUsersRepository : IGenericRepository<User>
    {
        private readonly EFGenericRepository<User> _context;

        public EFUsersRepository
        (
            EFGenericRepository<User> context
        )
        {
            _context = context;
        }

        public int Insert(User obj)
        {
            return _context.Insert(obj);
        }

        public async Task<int> InsertAsync(User obj)
        {
            return await _context.InsertAsync(obj);
        }

        public int Update(User obj)
        {
            User user = _context.GetById(b => b.Id == obj.Id);

            user.Email = obj.Email;
            user.FullName = obj.FullName;
            user.LastModificationTime = DateTime.Now.ToString();
            user.LastModifierUserId = 1;
            user.Username = obj.Username;
            user.is_skippable = obj.is_skippable;
            user.timeout = obj.timeout;
            user.reminder = obj.reminder;
            user.istimeouton = obj.istimeouton;
            user.intervals = obj.intervals;
            user.is_discussion_authorized = obj.is_discussion_authorized;

            if (!string.IsNullOrEmpty(obj.Password))
                user.Password = obj.Password;


            return _context.Update(user);
        }

        public async Task<int> UpdateAsync(User obj)
        {
            User user = _context.GetById(b => b.Id == obj.Id);

            user.Email = obj.Email;
            user.FullName = obj.FullName;
            user.LastModificationTime = DateTime.Now.ToString();
            user.LastModifierUserId = 1;
            user.Username = obj.Username;
            user.is_skippable = obj.is_skippable;
            user.timeout = obj.timeout;
            user.reminder = obj.reminder;
            user.istimeouton = obj.istimeouton;
            user.intervals = obj.intervals;
            user.is_discussion_authorized = obj.is_discussion_authorized;

            if (!string.IsNullOrEmpty(obj.Password))
                user.Password = obj.Password;


            return _context.Update(user);
        }

        public int UpdateAdditionalService(User obj)
        {
            User user = _context.GetById(b => b.Id == obj.Id);
            user.is_discussion_authorized = obj.is_discussion_authorized;
            user.is_assignment_authorized = obj.is_assignment_authorized;
            user.is_library_authorized = obj.is_library_authorized;
            return _context.Update(user);
        }

        public int BlockUser(User obj)
        {
            return _context.Update(obj);
        }

        public int Delete(User obj, string Id)
        {
            return _context.Delete(obj, Id);
        }

        public User Auth(User user)
        {
            User result = null;
            try
            {
                //result = _context.ListQuery(b => b.Email == user.Email && b.Password == user.Password && b.IsDeleted != true && b.AuthId == user.AuthId).FirstOrDefault();
                result = _context.ListQuery(b => b.IsDeleted != true && b.AuthId == user.AuthId).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public User UserExist(User user)
        {
            User result = null;

            try
            {
                result = _context.ListQuery(b => b.Email == user.Email && b.IsDeleted != true).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public User UpdateUserExist(User user)
        {
            User result = null;

            try
            {

                result = _context.ListQuery(b => b.Email == user.Email && b.IsDeleted != true).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public User UpdateUserExist(string email)
        {
            User result = null;

            try
            {
                result = _context.ListQuery(b => b.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public User UserExistById(User user)
        {
            User result = null;

            try
            {
                result = _context.GetById(t => t.Id == user.Id);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public bool CheckUser(User user)
        {
            bool IsExist = false;

            try
            {
                IsExist = _context.ListQuery(t => t.Username == user.Username).Any();
            }
            catch (Exception ex)
            {
            }

            return IsExist;
        }

        public User UserExistByUsername(string username)
        {
            User result = null;
            try
            {
                result = _context.GetById(t => t.Username == username && t.IsDeleted != true);
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public List<User> UserList()
        {
            List<User> userData = new List<User>();
            try
            {
                userData = _context.ListQuery(b => b.IsDeleted != true).ToList();
            }
            catch (Exception ex)
            {

            }
            return userData;
        }

        public bool IsPasswordChanged(User user)
        {
            User newuser = _context.GetById(b => b.Email == user.Email);
            newuser.Password = user.Password;
            _context.Update(newuser);
            return true;
        }

        public bool IsProfileUpdate(User user)
        {
            User newuser = _context.GetById(b => b.Email == user.Email);
            newuser.FullName = user.FullName;
            newuser.ProfilePicUrl = user.ProfilePicUrl;
            newuser.Email = user.Email;
            newuser.phonenumber = user.phonenumber;

            newuser.LastModificationTime = DateTime.Now.ToString();

            _context.Update(newuser);
            return true;
        }

        //Unused method.
        #region
        public List<User> GetAll()
        {
            //List<User> userList = (from testuser in _updateContext.Users
            //                       join thatsrole in _updateContext.UserRole on testuser.Id equals thatsrole.UserId
            //                       where testuser.IsDeleted != true && thatsrole.RoleId != 1
            //                       select testuser).ToList();
            List<User> userList = _context.ListQuery(b => b.IsDeleted != true).ToList();
            return userList;
        }

        public List<User> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public User GetById(Expression<Func<User, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public async Task<User> GetByIdAsync(Expression<Func<User, bool>> ex)
        {
            try
            {
                return await _context.GetByIdAsyncTest(ex);
            }
            catch (Exception)
            {
                throw;
            }  
        }

        public IQueryable<User> ListQuery(Expression<Func<User, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public async Task<List<User>> ListQueryAsync(Expression<Func<User, bool>> where)
        {
            return await _context.FindByConditionAsync(where);
        }
        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Delete(User obj)
        {
            throw new NotImplementedException();
        }

        public User GetById(int Id)
        {
            return _context.GetById(Id);
        }
        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class PasswordBusiness
    {
        private readonly EFUsersRepository EFUsersRepository;

        public PasswordBusiness(EFUsersRepository EFUsersRepository)
        {
            this.EFUsersRepository = EFUsersRepository;
        }

        public User UserExistanceUpdate(string email)
        {
            User _user = new User
            {
                Email = email
            };
            return EFUsersRepository.UpdateUserExist(_user); ;
        }

        public bool isValidateOldPassword(string email, string oldPassword)
        {
            var hashedPassword = "";
            hashedPassword = Crypto.Hash(oldPassword, "MD5");
            User user = UserExistanceUpdate(email);

            if (user.Password == hashedPassword) {
                return true;
            }
            else
            {
                return false;
            }            
        }

        public bool isPasswordChanged(string email, string newPassword) {

            User user = UserExistanceUpdate(email);

            var hashedPassword = "";
            hashedPassword = Crypto.Hash(newPassword, "MD5");

            user.Password = hashedPassword;

            EFUsersRepository.IsPasswordChanged(user);


            return true;            
        }

        public bool ForgotPassword(string email, string newPassword)
        {
            User user = UserExistanceUpdate(email);
            var hashedPassword = "";
            hashedPassword = Crypto.Hash(newPassword, "MD5");
            user.Password = hashedPassword;
            user.isforgot = false;
            user.forgotkey = null;
            EFUsersRepository.IsPasswordChanged(user);
            return true;
        }
    }
}

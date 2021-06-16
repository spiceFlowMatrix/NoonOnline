using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Students;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ProfileBusiness
    {
        private readonly EFUsersRepository EFUsersRepository;
        private readonly UsersBusiness usersBusiness;

        public ProfileBusiness(
            EFUsersRepository EFUsersRepository,
            UsersBusiness usersBusiness
        )
        {
            this.EFUsersRepository = EFUsersRepository;
            this.usersBusiness = usersBusiness;
        }

        public bool isProfileUpdate(string email, UpdateProfileModel UpdateProfileModel,int id,long userid)
        {
            User user = UserExistanceUpdate(email);
            user.FullName = UpdateProfileModel.fullname;

            if (usersBusiness.IsvalidUsername(UpdateProfileModel.username, id, userid))
                user.Username = UpdateProfileModel.username;
            else
                return false;
            
            user.Bio = UpdateProfileModel.bio;
            user.phonenumber = UpdateProfileModel.phonenumber;
            EFUsersRepository.IsProfileUpdate(user);
            return true;
        }

        public bool isProfilePhototUpload(string email, string photoUrl)
        {
            User user = UserExistanceUpdate(email);
            user.ProfilePicUrl = photoUrl;            
            EFUsersRepository.IsProfileUpdate(user);
            return true;
        }


        public User UserExistanceUpdate(string email)
        {
            User _user = new User
            {
                Email = email
            };
            return EFUsersRepository.UpdateUserExist(_user); ;
        }

    }
}

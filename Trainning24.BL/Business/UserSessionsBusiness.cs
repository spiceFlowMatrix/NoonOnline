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

namespace Trainning24.BL.Business
{
    public class UserSessionsBusiness
    {
        private readonly EFUserSessionsRepository _EFUserSessionsRepository;


        public UserSessionsBusiness
        (
            EFUserSessionsRepository EFUserSessionsRepository
        )
        {
            _EFUserSessionsRepository = EFUserSessionsRepository;
        }

        public UserSessions Create(UserSessions UserSessions, string Id)
        {
            UserSessions newObj = new UserSessions
            {
                UserId = UserSessions.UserId,
                AccessToken = UserSessions.AccessToken,
                DeviceToken = UserSessions.DeviceToken,
                DeviceType = UserSessions.DeviceType,
                Version = UserSessions.Version,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = int.Parse(Id),
                IsDeleted = false,
                DeleterUserId = 0,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = 0
            };

            newObj.Id = _EFUserSessionsRepository.Insert(newObj);

            return newObj;
        }

        public void Update(UserSessions UserSessions, string Id)
        {
            _EFUserSessionsRepository.Update(UserSessions);
        }

        public UserSessions GetUserSessionByAccessToken(string token)
        {
            return _EFUserSessionsRepository.GetUserSessionByAccessToken(token);
        }


        public void DeleteSession(string token, string uId)
        {
            _EFUserSessionsRepository.DeleteSession(token, uId);
        }

        public List<UserSessions> GetUsersSession(List<long> obj)
        {
            var data = _EFUserSessionsRepository.GetUserSessions(x => string.IsNullOrEmpty(x.DeletionTime) && !string.IsNullOrEmpty(x.DeviceToken)).Where(s => obj.Any(u => u.Equals(s.UserId)));
            return data.ToList();
        }
    }
}

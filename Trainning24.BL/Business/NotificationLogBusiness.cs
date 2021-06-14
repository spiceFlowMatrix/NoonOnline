using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class NotificationLogBusiness
    {
        private readonly EFNotificationLogRepository _EFNotificationLogRepository;
        private readonly LogObjectBusiness _logObjectBusiness;
        public NotificationLogBusiness(
            EFNotificationLogRepository EFNotificationLogRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            _EFNotificationLogRepository = EFNotificationLogRepository;
            _logObjectBusiness = logObjectBusiness;
        }

        public int AddLogsObject(long typeid, long entitykey, long actionuserid, long notifieduserid, bool IsRead)
        {
            LogObject logObject = _logObjectBusiness.AddLogsNotification(typeid, entitykey, actionuserid);
            NotificationLog notificationLog = new NotificationLog
            {
                NotifiedUserId = notifieduserid,
                LogObjectId = logObject.Id,
                IsRead = IsRead,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = Convert.ToInt32(actionuserid)
            };
            return _EFNotificationLogRepository.Insert(notificationLog);
        }
    }
}

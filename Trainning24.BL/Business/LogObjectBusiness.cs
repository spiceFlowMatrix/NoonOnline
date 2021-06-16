using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class LogObjectBusiness
    {
        private readonly EFLogObjectRepository _EFLogObjectRepository;

        public LogObjectBusiness(
            EFLogObjectRepository EFLogObjectRepository
        )
        {
            _EFLogObjectRepository = EFLogObjectRepository;
        }

        public int AddLogsObject(long typeid, long entitykey, long actionuserid)
        {
            LogObject logObject = new LogObject
            {
                TypeId = typeid,
                EntityKey = entitykey,
                ActionUserId = actionuserid,
                TimeStamp = DateTime.Now.ToString(),
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = Convert.ToInt32(actionuserid)
            };
            return _EFLogObjectRepository.Insert(logObject);
        }

        public LogObject AddLogsNotification(long typeid, long entitykey, long actionuserid)
        {
            LogObject logObject = new LogObject
            {
                TypeId = typeid,
                EntityKey = entitykey,
                ActionUserId = actionuserid,
                TimeStamp = DateTime.Now.ToString(),
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = Convert.ToInt32(actionuserid)
            };
            _EFLogObjectRepository.Insert(logObject);
            return logObject;
        }
    }
}

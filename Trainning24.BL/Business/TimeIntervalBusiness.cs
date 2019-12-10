using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class TimeIntervalBusiness
    {
        private readonly EFTimeIntervalRepository _EFTimeIntervalRepository;
        public TimeIntervalBusiness(
            EFTimeIntervalRepository EFTimeIntervalRepository
        )
        {
            _EFTimeIntervalRepository = EFTimeIntervalRepository;
        }

        public TimeInterval GetById(long Id)
        {
            return _EFTimeIntervalRepository.GetById(b => b.Id == Id && b.DeletionTime == null);
        }

        public TimeInterval Get()
        {
            return _EFTimeIntervalRepository.Get();
        }

        public TimeInterval UpdateInterval(TimeInterval obj)
        {
            var interval = GetById(obj.Id);
            if (interval != null)
            {
                interval.Interval = obj.Interval;
                interval.LastModificationTime = obj.LastModificationTime;
                interval.LastModifierUserId = obj.LastModifierUserId;
                _EFTimeIntervalRepository.Update(interval);
            }
            return interval;
        }
    }
}

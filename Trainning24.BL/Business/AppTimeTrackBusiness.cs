using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.AppTimeTrack;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class AppTimeTrackBusiness
    {
        private readonly EFAppTimeTrackSync _eFAppTimeTrackSync;

        public AppTimeTrackBusiness(
            EFAppTimeTrackSync eFAppTimeTrackSync
        )
        {
            _eFAppTimeTrackSync = eFAppTimeTrackSync;
        }

        public AppTimeTrack AddRecord(AppTimeTrack obj)
        {
            _eFAppTimeTrackSync.Insert(obj);
            return obj;
        }
        public async Task<int> AddRecordBulk(List<AppTimeTrack> obj)
        {
            return await _eFAppTimeTrackSync.SaveAsyncBulk(obj);
        }
        public List<AppTimeTrack> GetTimeTrackingByUserId(List<int> userId)
        {
            return _eFAppTimeTrackSync.ListQuery(b => userId.Contains((int)b.UserId) && b.ActivityTime != null && b.Outtime != null && b.DeletionTime == null && b.IsDeleted != true).ToList();
        }

        public List<AppTimeTrack> GetLocationTrackingByUserId(long userId)
        {
            return _eFAppTimeTrackSync.ListQuery(b => b.UserId == userId && b.ActivityTime != null && b.Outtime != null && b.DeletionTime == null && b.IsDeleted != true).ToList();
        }

        public async Task<int> SaveUpdateRecords(List<AppTimeTrackCSVDTO> dto)
        {
            if (dto.Count > 0)
            {
                List<AppTimeTrack> appTimeTracks = new List<AppTimeTrack>();
                foreach (var dt in dto)
                {
                    var exist = await _eFAppTimeTrackSync.GetRecors(b => b.Id == dt.AppTimeId && b.IsDeleted != true);
                    if (exist != null)
                    {
                        exist.ActivityTime = null;
                        exist.Outtime = null;
                        appTimeTracks.Add(exist);
                    }
                }
                if (appTimeTracks.Count > 0)
                    await _eFAppTimeTrackSync.UpdateRecordBulk(appTimeTracks);
            }
            return 1;
        }
    }
}

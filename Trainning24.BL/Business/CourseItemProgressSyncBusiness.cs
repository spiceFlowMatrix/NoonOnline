using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.CourseItemProgressSync;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class CourseItemProgressSyncBusiness
    {
        private readonly EFCourseItemProgressSync EFCourseItemProgressSync;

        public CourseItemProgressSyncBusiness
        (
            EFCourseItemProgressSync EFCourseItemProgressSync
        )
        {
            this.EFCourseItemProgressSync = EFCourseItemProgressSync;
        }

        public List<AddCourseItemProgressSync> Create(List<AddCourseItemProgressSync> addCourseItemProgressSyncs)
        {
            foreach (var courseItemProgress in addCourseItemProgressSyncs)
            {
                EFCourseItemProgressSync.Insert(new CourseItemProgressSync()
                {
                    Lessonid = courseItemProgress.lessonid,
                    Lessonprogress = courseItemProgress.lessonprogress,
                    
                    Quizid = courseItemProgress.quizid,
                    CreationTime = DateTime.Now.ToString()
                });
            }
            return addCourseItemProgressSyncs;
        }
    }
}

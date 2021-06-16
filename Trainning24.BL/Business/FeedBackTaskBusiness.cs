using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.FeedBackTask;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class FeedBackTaskBusiness
    {
        private readonly EFFeedBackTask EFFeedBackTask;
        private readonly EFFeedBackTaskStatus EFFeedBackTaskStatus;
        private readonly EFFeedBackTaskStatusOption EFFeedBackTaskStatusOption;
        private readonly UsersBusiness UsersBusiness;
        private readonly LessonBusiness LessonBusiness;

        public FeedBackTaskBusiness(
            EFFeedBackTask EFFeedBackTask,
            LessonBusiness LessonBusiness,
            EFFeedBackTaskStatus EFFeedBackTaskStatus,
            EFFeedBackTaskStatusOption EFFeedBackTaskStatusOption,
            UsersBusiness UsersBusiness
        )
        {
            this.UsersBusiness = UsersBusiness;
            this.EFFeedBackTaskStatus = EFFeedBackTaskStatus;
            this.EFFeedBackTaskStatusOption = EFFeedBackTaskStatusOption;
            this.EFFeedBackTask = EFFeedBackTask;
            this.LessonBusiness = LessonBusiness;
        }

        public FeedBackTask Create(AddFeedBackTaskModel AddFeedBackTaskModel, int id)
        {
            FeedBackTask FeedBackTask = new FeedBackTask
            {                
                FeedbackId = AddFeedBackTaskModel.feedbackid,
                //UserId = AddFeedBackTaskModel.userid,
                Description = AddFeedBackTaskModel.description,
                FileLink = AddFeedBackTaskModel.filelink,
                Type = AddFeedBackTaskModel.type,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFFeedBackTask.Insert(FeedBackTask);

            EFFeedBackTaskStatus.Insert(new FeedBackTaskStatus
            {
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                IsDeleted = false,
                FeedbackId = FeedBackTask.Id,
                Status = 1
            });

            return FeedBackTask;
        }

        public FeedBackTask Update(UpdateFeedBackTaskModel UpdateFeedBackTaskModel, int id)
        {
            FeedBackTask FeedBackTask = EFFeedBackTask.GetById(b => b.Id == UpdateFeedBackTaskModel.id);
            FeedBackTask.FeedbackId = UpdateFeedBackTaskModel.feedbackid;            
            FeedBackTask.Description = UpdateFeedBackTaskModel.description;
            FeedBackTask.FileLink = UpdateFeedBackTaskModel.filelink;
            FeedBackTask.Type = UpdateFeedBackTaskModel.type;
            FeedBackTask.LastModificationTime = DateTime.Now.ToString();
            FeedBackTask.LastModifierUserId = id;

            if (EFFeedBackTask.Update(FeedBackTask) == 1)
            {
                return EFFeedBackTask.GetById(b => b.Id == FeedBackTask.Id);
            }
            else
            {
                return null;
            }
        }

        public int  UpdateTaskStatus(int status, long feedbacktaskid, int userid)
        {
            FeedBackTaskStatus feedBackTaskStatus = EFFeedBackTaskStatus.ListQuery(b => b.FeedbackId == feedbacktaskid).LastOrDefault();

            if(feedBackTaskStatus != null)
            {
                if (feedBackTaskStatus.Status == status)
                    return 0;
            }

            return      EFFeedBackTaskStatus.Insert(new FeedBackTaskStatus
                        {
                            CreationTime = DateTime.Now.ToString(),
                            CreatorUserId = userid,
                            IsDeleted = false,
                            FeedbackId = feedbacktaskid,
                            Status = status
                        });
        }

        public List<FeedBackTask> FeedBackTaskList(PaginationModel paginationModel, out int total)
        {
            List<FeedBackTask> FeedBackTaskList = new List<FeedBackTask>();
            FeedBackTaskList = EFFeedBackTask.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = FeedBackTaskList.Count();

                FeedBackTaskList = FeedBackTaskList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();


                return FeedBackTaskList;
            }

            total = FeedBackTaskList.Count();
            return EFFeedBackTask.GetAll();
        }

        public ResponseFeedBackTaskModel getFeedBackTaskById(long id,string Certificate)
        {
            FeedBackTask fbt = EFFeedBackTask.GetById(b => b.Id == id && b.IsDeleted != true);
            ResponseFeedBackTaskModel ftm = new ResponseFeedBackTaskModel();
            ftm.id = fbt.Id;
            ftm.description = fbt.Description;
            ftm.feedbackid = fbt.FeedbackId;
            ftm.filelink = fbt.FileLink;
            ftm.type = fbt.Type;
            ftm.creationtime = fbt.CreationTime;

            FeedBackTaskStatus feedBackTaskStatus = EFFeedBackTaskStatus.ListQuery(k => k.FeedbackId == id).LastOrDefault();
            if(feedBackTaskStatus != null)
            {                                
                    ftm.Status = EFFeedBackTaskStatusOption.GetById(b => b.Id == feedBackTaskStatus.Status).Name;                                    
            }

            return ftm;
        }

        public List<FeedBackTask> getFeedBackTaskByFeedbackId(long id,long type)
        {
            List<FeedBackTask> fbt = EFFeedBackTask.ListQuery(b=>b.FeedbackId == id && b.Type == type).ToList();
            return fbt;
        }

        public FeedBackTask Delete(int Id, int DeleterId)
        {
            FeedBackTask FeedBackTask = new FeedBackTask();
            FeedBackTask.Id = Id;
            FeedBackTask.DeleterUserId = DeleterId;
            int i = EFFeedBackTask.Delete(FeedBackTask);
            FeedBackTask deletedFeedBackTask = EFFeedBackTask.GetById(b => b.Id == Id);
            return deletedFeedBackTask;
        }
    }
}

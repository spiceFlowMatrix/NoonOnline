using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.FeedBackActivity;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class FeedBackActivityBusiness
    {
        private readonly EFFeedBackActivity EFFeedBackActivity;
        private readonly UsersBusiness UsersBusiness;
        private readonly LessonBusiness LessonBusiness;

        public FeedBackActivityBusiness(
            EFFeedBackActivity EFFeedBackActivity,
            LessonBusiness LessonBusiness,
            UsersBusiness UsersBusiness
        )
        {
            this.EFFeedBackActivity = EFFeedBackActivity;
            this.UsersBusiness = UsersBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        public FeedBackActivity Create(AddFeedBackActivityModel AddFeedBackActivityModel, int id)
        {
            FeedBackActivity FeedBackActivity = new FeedBackActivity
            {
                FeedbackId = AddFeedBackActivityModel.FeedbackId,
                UserId = AddFeedBackActivityModel.UserId,
                FeedbackTaskId = AddFeedBackActivityModel.FeedbackTaskId,                
                Type = AddFeedBackActivityModel.Type,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFFeedBackActivity.Insert(FeedBackActivity);

            return FeedBackActivity;
        }

        public FeedBackActivity Update(UpdateFeedBackActivityModel UpdateFeedBackActivityModel, int id)
        {
            FeedBackActivity FeedBackActivity = EFFeedBackActivity.GetById(b => b.Id == UpdateFeedBackActivityModel.id);
            FeedBackActivity.FeedbackId = UpdateFeedBackActivityModel.FeedbackId;
                FeedBackActivity.UserId = UpdateFeedBackActivityModel.UserId;
            FeedBackActivity.FeedbackTaskId = UpdateFeedBackActivityModel.FeedbackTaskId;
            FeedBackActivity.Type = UpdateFeedBackActivityModel.Type;
            FeedBackActivity.IsDeleted = false;
            FeedBackActivity.CreationTime = DateTime.Now.ToString();
            FeedBackActivity.CreatorUserId = id;
            FeedBackActivity.LastModificationTime = DateTime.Now.ToString();
            FeedBackActivity.LastModifierUserId = id;

            if (EFFeedBackActivity.Update(FeedBackActivity) == 1)
            {
                return EFFeedBackActivity.GetById(b => b.Id == FeedBackActivity.Id);
            }
            else
            {
                return null;
            }
        }

        public List<FeedBackActivity> FeedBackActivityList(PaginationModel paginationModel, out int total)
        {
            List<FeedBackActivity> FeedBackActivityList = new List<FeedBackActivity>();
            FeedBackActivityList = EFFeedBackActivity.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = FeedBackActivityList.Count();

                FeedBackActivityList = FeedBackActivityList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                return FeedBackActivityList;
            }

            total = FeedBackActivityList.Count();
            return EFFeedBackActivity.GetAll();
        }

        public ResponseFeedBackActivityModel getFeedBackActivityById(long id, string Certificate)
        {
            FeedBackActivity fa = EFFeedBackActivity.GetById(b => b.Id == id && b.IsDeleted != true);
            ResponseFeedBackActivityModel fam = new ResponseFeedBackActivityModel();
            fam.id = fa.Id;
            fam.FeedbackId = fa.FeedbackId;
            fam.FeedbackTaskId = fa.FeedbackTaskId;
            fam.Type = fa.Type;            

            User user = UsersBusiness.GetUserbyId(fa.UserId);
            FeedBackUser staffdetail = new FeedBackUser();
            List<Role> roles = UsersBusiness.Role(user);

            if (roles != null)
            {
                List<long> roleids = new List<long>();
                List<string> rolenames = new List<string>();
                foreach (var role in roles)
                {
                    roleids.Add(role.Id);
                    rolenames.Add(role.Name);
                }
                staffdetail.Roles = roleids;
                staffdetail.RoleName = rolenames;
            }

            staffdetail.Id = user.Id;
            staffdetail.Username = user.Username;
            staffdetail.FullName = user.FullName;
            staffdetail.Email = user.Email;
            staffdetail.Bio = user.Bio;

            if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                staffdetail.profilepicurl = LessonBusiness.geturl(user.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");

            fam.userid = staffdetail;

            return fam;
        }

        public FeedBackActivity Delete(int Id, int DeleterId)
        {
            FeedBackActivity FeedBackActivity = new FeedBackActivity();
            FeedBackActivity.Id = Id;
            FeedBackActivity.DeleterUserId = DeleterId;
            int i = EFFeedBackActivity.Delete(FeedBackActivity);
            FeedBackActivity deletedFeedBackActivity = EFFeedBackActivity.GetById(b => b.Id == Id);
            return deletedFeedBackActivity;
        }
    }
}
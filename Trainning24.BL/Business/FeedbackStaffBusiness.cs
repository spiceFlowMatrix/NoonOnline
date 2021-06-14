using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.FeedbackStaff;
using Trainning24.BL.ViewModels.Feedback;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class FeedbackStaffBusiness
    {
        private readonly EFFeedbackStaffRepository EFFeedBackStaff;
        private readonly UsersBusiness usersBusiness;
        private readonly LessonBusiness LessonBusiness;

        public FeedbackStaffBusiness(
            EFFeedbackStaffRepository EFFeedBackStaff,
            UsersBusiness usersBusiness,
            LessonBusiness LessonBusiness
        )
        {
            this.EFFeedBackStaff = EFFeedBackStaff;
            this.usersBusiness = usersBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        public async Task<ResponseFeedbackStaff> Create(AddFeedBackStaffModel AddFeedBackStaffModel, int id, string Certificate)
        {
            ResponseFeedbackStaff responseFeedbackStaff = new ResponseFeedbackStaff();
            FeedBackStaff FeedBackStaff = new FeedBackStaff
            {
                FeedBackId = AddFeedBackStaffModel.feedbackid,
                UserId = AddFeedBackStaffModel.userid,
                Type = AddFeedBackStaffModel.type,
                IsManager = AddFeedBackStaffModel.ismanager,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            await EFFeedBackStaff.InsertAsync(FeedBackStaff);

            FeedBackUser user = new FeedBackUser();
            User getUser = usersBusiness.GetUserbyId(FeedBackStaff.UserId);
            List<Role> roles = usersBusiness.Role(getUser);
            if (roles != null)
            {
                List<long> roleids = new List<long>();
                List<string> rolenames = new List<string>();
                foreach (var role in roles)
                {
                    roleids.Add(role.Id);
                    rolenames.Add(role.Name);
                }
                user.Roles = roleids;
                user.RoleName = rolenames;
            }
            user.Id = getUser.Id;
            user.Username = getUser.Username;
            user.FullName = getUser.FullName;
            user.Email = user.Email;
            user.Bio = user.Bio;
            if (!string.IsNullOrEmpty(getUser.ProfilePicUrl))
                user.profilepicurl = LessonBusiness.geturl(getUser.ProfilePicUrl, Certificate, "edg-primary-profile-image-storage");
            responseFeedbackStaff.Id = FeedBackStaff.Id;
            responseFeedbackStaff.CreationDate = FeedBackStaff.CreationTime;
            responseFeedbackStaff.Type = FeedBackStaff.Type;
            responseFeedbackStaff.User = user;
            responseFeedbackStaff.IsManager = FeedBackStaff.IsManager;
            return responseFeedbackStaff;
        }

        public FeedBackStaff Update(UpdateFeedBackStaffModel UpdateFeedBackStaffModel, int id)
        {
            FeedBackStaff FeedBackStaff = EFFeedBackStaff.GetById(b => b.Id == UpdateFeedBackStaffModel.id);
            FeedBackStaff.FeedBackId = UpdateFeedBackStaffModel.feedbackid;
            FeedBackStaff.UserId = UpdateFeedBackStaffModel.userid;
            FeedBackStaff.Type = UpdateFeedBackStaffModel.type;
            FeedBackStaff.LastModificationTime = DateTime.Now.ToString();
            FeedBackStaff.LastModifierUserId = id;

            if (EFFeedBackStaff.Update(FeedBackStaff) == 1)
            {
                return EFFeedBackStaff.GetById(b => b.Id == FeedBackStaff.Id);
            }
            else
            {
                return null;
            }
        }

        public List<FeedBackStaff> FeedBackStaffList(PaginationModel paginationModel, out int total)
        {
            List<FeedBackStaff> FeedBackStaffList = new List<FeedBackStaff>();
            FeedBackStaffList = EFFeedBackStaff.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = FeedBackStaffList.Count();

                FeedBackStaffList = FeedBackStaffList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                    return FeedBackStaffList;
            }

            total = FeedBackStaffList.Count();
            return EFFeedBackStaff.GetAll();
        }

        public FeedBackStaff getFeedBackStaffById(long id)
        {
            return EFFeedBackStaff.GetById(b => b.Id == id && b.IsDeleted != true);
        }

        public FeedBackStaff getFeedBackStaffByDetail(AddFeedBackStaffModel toberemoved)
        {
            return EFFeedBackStaff.ListQuery(b => b.FeedBackId == toberemoved.feedbackid && 
                                             b.UserId == toberemoved.userid && 
                                             b.Type == toberemoved.type && 
                                             b.IsManager == toberemoved.ismanager &&
                                             b.IsDeleted != true).SingleOrDefault();
        }


        public FeedBackStaff Removed(AddFeedBackStaffModel AddFeedBackStaffModel, int id)
        {
            FeedBackStaff FeedBackStaff = getFeedBackStaffByDetail(AddFeedBackStaffModel);   
            
            if(FeedBackStaff != null)
            {
                FeedBackStaff.DeleterUserId = id;
                EFFeedBackStaff.Delete(FeedBackStaff);
                return FeedBackStaff;
            }

            return null;
        }

        public FeedBackStaff Delete(int Id, int DeleterId)
        {
            FeedBackStaff FeedBackStaff = new FeedBackStaff();
            FeedBackStaff.Id = Id;
            FeedBackStaff.DeleterUserId = DeleterId;
            int i = EFFeedBackStaff.Delete(FeedBackStaff);
            FeedBackStaff deletedFeedBackStaff = EFFeedBackStaff.GetById(b => b.Id == Id);
            return deletedFeedBackStaff;
        }

        public FeedBackStaff CheckUserExists(long fid, long userid, bool ismanager)
        {
            FeedBackStaff feedBackStaff = new FeedBackStaff();
            if (ismanager)
            {
                feedBackStaff = EFFeedBackStaff.GetById(b => b.FeedBackId == fid && b.UserId == userid && b.IsManager == true && b.IsDeleted != true);
            }
            else
            {
                feedBackStaff = EFFeedBackStaff.GetById(b => b.FeedBackId == fid && b.UserId == userid && b.IsManager == false && b.IsDeleted != true);
            }
            return feedBackStaff;
        }

        public FeedBackStaff CheckManagerUserExists(long fid, long type, bool ismanager)
        {
            FeedBackStaff feedBackStaff = new FeedBackStaff();
                feedBackStaff = EFFeedBackStaff.GetById(b => b.FeedBackId == fid && b.Type == type && b.IsManager == true && b.IsDeleted != true);
            return feedBackStaff;
        }
    }
}

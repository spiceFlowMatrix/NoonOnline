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
using System.Globalization;
using Trainning24.BL.ViewModels.UserRole;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Language;
using Microsoft.Extensions.Options;

namespace Trainning24.BL.Business
{
    public class DiscussionTopicLikeBusiness
    {
        private readonly EFDiscussionTopicLikeRepository _eFDiscussionTopicLikeRepository;
        public DiscussionTopicLikeBusiness
        (
            EFDiscussionTopicLikeRepository eFDiscussionTopicLikeRepository
        )
        {
            _eFDiscussionTopicLikeRepository = eFDiscussionTopicLikeRepository;
        }

        public DiscussionTopicLikes AddLike(DiscussionTopicLikes obj)
        {
            _eFDiscussionTopicLikeRepository.Insert(obj);
            return obj;
        }

        public DiscussionTopicLikes UpdateLike(DiscussionTopicLikes obj)
        {
            DiscussionTopicLikes getTopicLike = _eFDiscussionTopicLikeRepository.GetById(b => b.TopicId == obj.TopicId && b.UserId == obj.UserId && b.IsDeleted != true);
            if (getTopicLike != null)
            {
                getTopicLike.Like = obj.Like;
                getTopicLike.DisLike = obj.DisLike;
                getTopicLike.LastModifierUserId = obj.LastModifierUserId;
                getTopicLike.LastModificationTime = obj.LastModificationTime;
                _eFDiscussionTopicLikeRepository.Update(getTopicLike);
                return getTopicLike;
            }
            else
            {
                return null;
            }
        }

        public DiscussionTopicLikes Exists(long topicid, long userid)
        {
            return _eFDiscussionTopicLikeRepository.GetById(b => b.TopicId == topicid && b.UserId == userid && b.IsDeleted != true);
        }

        public int TotalLikeCount(long topicid)
        {
            return _eFDiscussionTopicLikeRepository.ListQuery(b => b.TopicId == topicid && b.Like == true).Count();
        }
        public int TotalDisLikeCount(long topicid)
        {
            return _eFDiscussionTopicLikeRepository.ListQuery(b => b.TopicId == topicid && b.DisLike == true).Count();
        }

        public bool LikedByUser(long topicid, long userid)
        {
            bool liked = false;
            var like = _eFDiscussionTopicLikeRepository.GetById(b => b.TopicId == topicid && b.UserId == userid && b.Like == true);
            if (like != null)
            {
                liked = true;
            }
            return liked;
        }

        public bool DisLikedByUser(long topicid, long userid)
        {
            bool disliked = false;
            var dislike = _eFDiscussionTopicLikeRepository.GetById(b => b.TopicId == topicid && b.UserId == userid && b.DisLike == true);
            if (dislike != null)
            {
                disliked = true;
            }
            return disliked;
        }
    }
}

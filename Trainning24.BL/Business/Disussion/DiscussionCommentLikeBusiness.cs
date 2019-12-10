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
    public class DiscussionCommentLikeBusiness
    {
        private readonly EFDiscussionCommentLikeRepository _eFDiscussionCommentLikeRepository;
        public DiscussionCommentLikeBusiness
        (
            EFDiscussionCommentLikeRepository eFDiscussionCommentLikeRepository
        )
        {
            _eFDiscussionCommentLikeRepository = eFDiscussionCommentLikeRepository;
        }

        public DiscussionCommentLikes AddLike(DiscussionCommentLikes obj)
        {
            _eFDiscussionCommentLikeRepository.Insert(obj);
            return obj;
        }

        public DiscussionCommentLikes UpdateLike(DiscussionCommentLikes obj)
        {
            DiscussionCommentLikes getCommentLike = _eFDiscussionCommentLikeRepository.GetById(b => b.CommentId == obj.CommentId && b.UserId == obj.UserId && b.IsDeleted != true);
            if (getCommentLike != null)
            {
                getCommentLike.Like = obj.Like;
                getCommentLike.DisLike = obj.DisLike;
                getCommentLike.LastModificationTime = obj.LastModificationTime;
                getCommentLike.LastModifierUserId = obj.LastModifierUserId;
                _eFDiscussionCommentLikeRepository.Update(getCommentLike);
                return getCommentLike;
            }
            else
            {
                return null;
            }
        }

        public DiscussionCommentLikes Exists(long topicid, long userid)
        {
            return _eFDiscussionCommentLikeRepository.GetById(b => b.CommentId == topicid && b.UserId == userid && b.IsDeleted != true);
        }

        public int TotalLikeCount(long commentid)
        {
            return _eFDiscussionCommentLikeRepository.ListQuery(b => b.CommentId == commentid && b.Like == true).Count();
        }
        public int TotalDisLikeCount(long commentid)
        {
            return _eFDiscussionCommentLikeRepository.ListQuery(b => b.CommentId == commentid && b.DisLike == true).Count();
        }

        public bool LikedByUser(long commentid, long userid)
        {
            bool liked = false;
            var like = _eFDiscussionCommentLikeRepository.GetById(b => b.CommentId == commentid && b.UserId == userid && b.Like == true);
            if (like != null)
            {
                liked = true;
            }
            return liked;
        }

        public bool DisLikedByUser(long commentid, long userid)
        {
            bool disliked = false;
            var dislike = _eFDiscussionCommentLikeRepository.GetById(b => b.CommentId == commentid && b.UserId == userid && b.DisLike == true);
            if (dislike != null)
            {
                disliked = true;
            }
            return disliked;
        }
    }
}

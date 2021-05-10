package com.ibl.apps.Model;

public class NotiType6_7Object {

    private String Type;

    private User User;

    private Comments comments;

    private Discussion discussion;

    private String UserId;

    private Files[] files;

    private String FileId;

    private String Tag;

    private String IsRead;

    private String CreatorUserId;

    private String CommentId;

    public String getType() {
        return Type;
    }

    public void setType(String Type) {
        this.Type = Type;
    }

    public User getUser() {
        return User;
    }

    public void setUser(User User) {
        this.User = User;
    }

    public Comments getComments() {
        return comments;
    }

    public void setComments(Comments comments) {
        this.comments = comments;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String UserId) {
        this.UserId = UserId;
    }

    public Files[] getFiles() {
        return files;
    }

    public void setFiles(Files[] files) {
        this.files = files;
    }

    public String getFileId() {
        return FileId;
    }

    public void setFileId(String FileId) {
        this.FileId = FileId;
    }

    public String getTag() {
        return Tag;
    }

    public void setTag(String Tag) {
        this.Tag = Tag;
    }

    public String getIsRead() {
        return IsRead;
    }

    public void setIsRead(String IsRead) {
        this.IsRead = IsRead;
    }

    public String getCreatorUserId() {
        return CreatorUserId;
    }

    public void setCreatorUserId(String CreatorUserId) {
        this.CreatorUserId = CreatorUserId;
    }

    public String getCommentId() {
        return CommentId;
    }

    public void setCommentId(String CommentId) {
        this.CommentId = CommentId;
    }


    public Discussion getDiscussion() {
        return discussion;
    }

    public void setDiscussion(Discussion discussion) {
        this.discussion = discussion;
    }

    @Override
    public String toString() {
        return "ClassPojo [Type = " + Type + ", User = " + User + ", comments = " + comments + ", UserId = " + UserId + ", files = " + files + ", FileId = " + FileId + ", Tag = " + Tag + ", IsRead = " + IsRead + ", CreatorUserId = " + CreatorUserId + ", CommentId = " + CommentId + "]";
    }


    public class Discussion {
        private String isprivate;

        private String description;

        private String id;

        private String title;

        private String courseid;

        public String getIsprivate() {
            return isprivate;
        }

        public void setIsprivate(String isprivate) {
            this.isprivate = isprivate;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getTitle() {
            return title;
        }

        public void setTitle(String title) {
            this.title = title;
        }

        public String getCourseid() {
            return courseid;
        }

        public void setCourseid(String courseid) {
            this.courseid = courseid;
        }

        @Override
        public String toString() {
            return "ClassPojo [isprivate = " + isprivate + ", description = " + description + ", id = " + id + ", title = " + title + ", courseid = " + courseid + "]";
        }
    }

    public class Files {
        private String SignedUrl;

        private String FileName;

        private String TotalPages;

        private String Id;

        private String Url;

        private String FileTypeId;

        private String CommentId;

        private String Name;

        private String FileSize;

        public String getSignedUrl() {
            return SignedUrl;
        }

        public void setSignedUrl(String SignedUrl) {
            this.SignedUrl = SignedUrl;
        }

        public String getFileName() {
            return FileName;
        }

        public void setFileName(String FileName) {
            this.FileName = FileName;
        }

        public String getTotalPages() {
            return TotalPages;
        }

        public void setTotalPages(String TotalPages) {
            this.TotalPages = TotalPages;
        }

        public String getId() {
            return Id;
        }

        public void setId(String Id) {
            this.Id = Id;
        }

        public String getUrl() {
            return Url;
        }

        public void setUrl(String Url) {
            this.Url = Url;
        }

        public String getFileTypeId() {
            return FileTypeId;
        }

        public void setFileTypeId(String FileTypeId) {
            this.FileTypeId = FileTypeId;
        }

        public String getCommentId() {
            return CommentId;
        }

        public void setCommentId(String CommentId) {
            this.CommentId = CommentId;
        }

        public String getName() {
            return Name;
        }

        public void setName(String Name) {
            this.Name = Name;
        }

        public String getFileSize() {
            return FileSize;
        }

        public void setFileSize(String FileSize) {
            this.FileSize = FileSize;
        }

        @Override
        public String toString() {
            return "ClassPojo [SignedUrl = " + SignedUrl + ", FileName = " + FileName + ", TotalPages = " + TotalPages + ", Id = " + Id + ", Url = " + Url + ", FileTypeId = " + FileTypeId + ", CommentId = " + CommentId + ", Name = " + Name + ", FileSize = " + FileSize + "]";
        }
    }

    public class Comments {

        private String topicid;

        private String createtime;

        private String comment;

        private String Id;

        public String getTopicid() {
            return topicid;
        }

        public void setTopicid(String topicid) {
            this.topicid = topicid;
        }

        public String getCreatetime() {
            return createtime;
        }

        public void setCreatetime(String createtime) {
            this.createtime = createtime;
        }

        public String getComment() {
            return comment;
        }

        public void setComment(String comment) {
            this.comment = comment;
        }

        public String getId() {
            return Id;
        }

        public void setId(String Id) {
            this.Id = Id;
        }

        @Override
        public String toString() {
            return "ClassPojo [topicid = " + topicid + ", createtime = " + createtime + ", comment = " + comment + ", Id = " + Id + "]";
        }
    }

    public class User {
        private String roleid;

        private String phonenumber;

        private String id;

        private String fullname;

        private String is_skippable;

        private String email;

        private String username;

        public String getRoleid() {
            return roleid;
        }

        public void setRoleid(String roleid) {
            this.roleid = roleid;
        }

        public String getPhonenumber() {
            return phonenumber;
        }

        public void setPhonenumber(String phonenumber) {
            this.phonenumber = phonenumber;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getFullname() {
            return fullname;
        }

        public void setFullname(String fullname) {
            this.fullname = fullname;
        }

        public String getIs_skippable() {
            return is_skippable;
        }

        public void setIs_skippable(String is_skippable) {
            this.is_skippable = is_skippable;
        }

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        @Override
        public String toString() {
            return "ClassPojo [roleid = " + roleid + ", phonenumber = " + phonenumber + ", id = " + id + ", fullname = " + fullname + ", is_skippable = " + is_skippable + ", email = " + email + ", username = " + username + "]";
        }
    }


}

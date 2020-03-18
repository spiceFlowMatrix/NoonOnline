package com.ibl.apps.Model;

public class NotiType4Object {

    private String Type;

    private String DiscussionId;

    private User User;

    private String UserId;

    private String Tag;

    private Discussion discussion;

    private String IsRead;

    private String CreatorUserId;

    public String getType() {
        return Type;
    }

    public void setType(String Type) {
        this.Type = Type;
    }

    public String getDiscussionId() {
        return DiscussionId;
    }

    public void setDiscussionId(String DiscussionId) {
        this.DiscussionId = DiscussionId;
    }

    public NotiType4Object.User getUser() {
        return User;
    }

    public void setUser(NotiType4Object.User user) {
        User = user;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String UserId) {
        this.UserId = UserId;
    }

    public String getTag() {
        return Tag;
    }

    public void setTag(String Tag) {
        this.Tag = Tag;
    }

    public Discussion getDiscussion() {
        return discussion;
    }

    public void setDiscussion(Discussion discussion) {
        this.discussion = discussion;
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

    @Override
    public String toString() {
        return "ClassPojo [Type = " + Type + ", DiscussionId = " + DiscussionId + ", User = " + User + ", UserId = " + UserId + ", Tag = " + Tag + ", discussion = " + discussion + ", IsRead = " + IsRead + ", CreatorUserId = " + CreatorUserId + "]";
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

    public class User
    {
        private String roleid;

        private String profilepicurl;

        private String phonenumber;

        private String id;

        private String fullname;

        private String is_skippable;

        private String email;

        private String username;

        public String getRoleid ()
        {
            return roleid;
        }

        public void setRoleid (String roleid)
        {
            this.roleid = roleid;
        }

        public String getProfilepicurl ()
        {
            return profilepicurl;
        }

        public void setProfilepicurl (String profilepicurl)
        {
            this.profilepicurl = profilepicurl;
        }

        public String getPhonenumber ()
        {
            return phonenumber;
        }

        public void setPhonenumber (String phonenumber)
        {
            this.phonenumber = phonenumber;
        }

        public String getId ()
        {
            return id;
        }

        public void setId (String id)
        {
            this.id = id;
        }

        public String getFullname ()
        {
            return fullname;
        }

        public void setFullname (String fullname)
        {
            this.fullname = fullname;
        }

        public String getIs_skippable ()
        {
            return is_skippable;
        }

        public void setIs_skippable (String is_skippable)
        {
            this.is_skippable = is_skippable;
        }

        public String getEmail ()
        {
            return email;
        }

        public void setEmail (String email)
        {
            this.email = email;
        }

        public String getUsername ()
        {
            return username;
        }

        public void setUsername (String username)
        {
            this.username = username;
        }

        @Override
        public String toString()
        {
            return "ClassPojo [roleid = "+roleid+", profilepicurl = "+profilepicurl+", phonenumber = "+phonenumber+", id = "+id+", fullname = "+fullname+", is_skippable = "+is_skippable+", email = "+email+", username = "+username+"]";
        }
    }

}

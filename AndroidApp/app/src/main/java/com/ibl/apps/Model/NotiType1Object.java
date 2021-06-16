package com.ibl.apps.Model;

public class NotiType1Object {

    private String Type;

    private String CourseId;

    private User User;

    private String UserId;

    private String Id;

    private String Tag;

    private Course Course;

    private String IsRead;

    private String CreatorUserId;

    public String getType() {
        return Type;
    }

    public void setType(String Type) {
        this.Type = Type;
    }

    public String getCourseId() {
        return CourseId;
    }

    public void setCourseId(String CourseId) {
        this.CourseId = CourseId;
    }

    public User getUser() {
        return User;
    }

    public void setUser(User User) {
        this.User = User;
    }

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String UserId) {
        this.UserId = UserId;
    }

    public String getId() {
        return Id;
    }

    public void setId(String Id) {
        this.Id = Id;
    }

    public String getTag() {
        return Tag;
    }

    public void setTag(String Tag) {
        this.Tag = Tag;
    }

    public Course getCourse() {
        return Course;
    }

    public void setCourse(Course Course) {
        this.Course = Course;
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
        return "ClassPojo [Type = " + Type + ", CourseId = " + CourseId + ", User = " + User + ", UserId = " + UserId + ", Id = " + Id + ", Tag = " + Tag + ", Course = " + Course + ", IsRead = " + IsRead + ", CreatorUserId = " + CreatorUserId + "]";
    }

    public class Course {
        private String Description;

        private String Id;

        private String Image;

        private String Code;

        private String Name;

        public String getDescription() {
            return Description;
        }

        public void setDescription(String Description) {
            this.Description = Description;
        }

        public String getId() {
            return Id;
        }

        public void setId(String Id) {
            this.Id = Id;
        }

        public String getImage() {
            return Image;
        }

        public void setImage(String Image) {
            this.Image = Image;
        }

        public String getCode() {
            return Code;
        }

        public void setCode(String Code) {
            this.Code = Code;
        }

        public String getName() {
            return Name;
        }

        public void setName(String Name) {
            this.Name = Name;
        }

        @Override
        public String toString() {
            return "ClassPojo [Description = " + Description + ", Id = " + Id + ", Image = " + Image + ", Code = " + Code + ", Name = " + Name + "]";
        }
    }

    public class User {
        private String rolename;

        private String roleid;

        private String profilepicurl;

        private String bio;

        private String id;

        private String fullname;

        private String is_skippable;

        private String email;

        private String username;

        public String getRolename() {
            return rolename;
        }

        public void setRolename(String rolename) {
            this.rolename = rolename;
        }

        public String getRoleid() {
            return roleid;
        }

        public void setRoleid(String roleid) {
            this.roleid = roleid;
        }

        public String getProfilepicurl() {
            return profilepicurl;
        }

        public void setProfilepicurl(String profilepicurl) {
            this.profilepicurl = profilepicurl;
        }

        public String getBio() {
            return bio;
        }

        public void setBio(String bio) {
            this.bio = bio;
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
            return "ClassPojo [rolename = " + rolename + ", roleid = " + roleid + ", profilepicurl = " + profilepicurl + ", bio = " + bio + ", id = " + id + ", fullname = " + fullname + ", is_skippable = " + is_skippable + ", email = " + email + ", username = " + username + "]";
        }
    }


}


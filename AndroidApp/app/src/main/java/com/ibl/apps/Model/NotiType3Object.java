package com.ibl.apps.Model;

public class NotiType3Object {

    private User User;

    private String LessionId;

    private Chapter Chapter;

    private Lesson lesson;

    private String IsRead;

    private String Type;

    private String CourseId;

    private String UserId;

    private String ChapterId;

    private String FileId;

    private String Id;

    private String Tag;

    private Course Course;

    private String CreatorUserId;

    public User getUser() {
        return User;
    }

    public void setUser(User User) {
        this.User = User;
    }

    public String getLessionId() {
        return LessionId;
    }

    public void setLessionId(String LessionId) {
        this.LessionId = LessionId;
    }

    public Chapter getChapter() {
        return Chapter;
    }

    public void setChapter(Chapter Chapter) {
        this.Chapter = Chapter;
    }

    public Lesson getLesson() {
        return lesson;
    }

    public void setLesson(Lesson lesson) {
        this.lesson = lesson;
    }

    public String getIsRead() {
        return IsRead;
    }

    public void setIsRead(String IsRead) {
        this.IsRead = IsRead;
    }

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

    public String getUserId() {
        return UserId;
    }

    public void setUserId(String UserId) {
        this.UserId = UserId;
    }

    public String getChapterId() {
        return ChapterId;
    }

    public void setChapterId(String ChapterId) {
        this.ChapterId = ChapterId;
    }

    public String getFileId() {
        return FileId;
    }

    public void setFileId(String FileId) {
        this.FileId = FileId;
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

    public String getCreatorUserId() {
        return CreatorUserId;
    }

    public void setCreatorUserId(String CreatorUserId) {
        this.CreatorUserId = CreatorUserId;
    }

    @Override
    public String toString() {
        return "ClassPojo [User = " + User + ", LessionId = " + LessionId + ", Chapter = " + Chapter + ", lesson = " + lesson + ", IsRead = " + IsRead + ", Type = " + Type + ", CourseId = " + CourseId + ", UserId = " + UserId + ", ChapterId = " + ChapterId + ", FileId = " + FileId + ", Id = " + Id + ", Tag = " + Tag + ", Course = " + Course + ", CreatorUserId = " + CreatorUserId + "]";
    }

    public class Lesson {
        private String code;

        private String name;

        private String description;

        private String id;

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
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

        @Override
        public String toString() {
            return "ClassPojo [code = " + code + ", name = " + name + ", description = " + description + ", id = " + id + "]";
        }
    }

    public class Chapter {
        private String Id;

        private String courseid;

        private String Name;

        public String getId() {
            return Id;
        }

        public void setId(String Id) {
            this.Id = Id;
        }

        public String getCourseid() {
            return courseid;
        }

        public void setCourseid(String courseid) {
            this.courseid = courseid;
        }

        public String getName() {
            return Name;
        }

        public void setName(String Name) {
            this.Name = Name;
        }

        @Override
        public String toString() {
            return "ClassPojo [Id = " + Id + ", courseid = " + courseid + ", Name = " + Name + "]";
        }
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


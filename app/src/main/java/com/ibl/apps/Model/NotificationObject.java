package com.ibl.apps.Model;

import java.util.ArrayList;

public class NotificationObject {

    private String response_code;

    private ArrayList<Data> data;

    private String message;

    private String status;

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public ArrayList<Data> getData() {
        return data;
    }

    public void setData(ArrayList<Data> data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    @Override
    public String toString() {
        return "ClassPojo [response_code = " + response_code + ", data = " + data + ", message = " + message + ", status = " + status + "]";
    }

    public class Data {

        private Quiz quiz;

        private Chapter chapter;

        private String creatorUserId;

        private Comments comments;

        private String quizId;

        private String isRead;

        private Lesson lesson;

        private Discussion discussion;

        private String type;

        private String userId;

        private String assignmentId;

        private String lessionId;

        private String dateCreated;

        private String discussionId;

        private String chapterId;

        private String commentId;

        private Course course;

        private String files;

        private String id;

        private String tag;

        private String courseId;

        private User user;

        private String fileId;

        public Quiz getQuiz() {
            return quiz;
        }

        public void setQuiz(Quiz quiz) {
            this.quiz = quiz;
        }

        public Chapter getChapter() {
            return chapter;
        }

        public void setChapter(Chapter chapter) {
            this.chapter = chapter;
        }

        public String getCreatorUserId() {
            return creatorUserId;
        }

        public void setCreatorUserId(String creatorUserId) {
            this.creatorUserId = creatorUserId;
        }

        public Comments getComments() {
            return comments;
        }

        public void setComments(Comments comments) {
            this.comments = comments;
        }

        public String getQuizId() {
            return quizId;
        }

        public void setQuizId(String quizId) {
            this.quizId = quizId;
        }

        public String getIsRead() {
            return isRead;
        }

        public void setIsRead(String isRead) {
            this.isRead = isRead;
        }

        public Lesson getLesson() {
            return lesson;
        }

        public void setLesson(Lesson lesson) {
            this.lesson = lesson;
        }

        public Discussion getDiscussion() {
            return discussion;
        }

        public void setDiscussion(Discussion discussion) {
            this.discussion = discussion;
        }

        public String getType() {
            return type;
        }

        public void setType(String type) {
            this.type = type;
        }

        public String getUserId() {
            return userId;
        }

        public void setUserId(String userId) {
            this.userId = userId;
        }

        public String getAssignmentId() {
            return assignmentId;
        }

        public void setAssignmentId(String assignmentId) {
            this.assignmentId = assignmentId;
        }

        public String getLessionId() {
            return lessionId;
        }

        public void setLessionId(String lessionId) {
            this.lessionId = lessionId;
        }

        public String getDateCreated() {
            return dateCreated;
        }

        public void setDateCreated(String dateCreated) {
            this.dateCreated = dateCreated;
        }

        public String getDiscussionId() {
            return discussionId;
        }

        public void setDiscussionId(String discussionId) {
            this.discussionId = discussionId;
        }

        public String getChapterId() {
            return chapterId;
        }

        public void setChapterId(String chapterId) {
            this.chapterId = chapterId;
        }

        public String getCommentId() {
            return commentId;
        }

        public void setCommentId(String commentId) {
            this.commentId = commentId;
        }

        public Course getCourse() {
            return course;
        }

        public void setCourse(Course course) {
            this.course = course;
        }

        public String getFiles() {
            return files;
        }

        public void setFiles(String files) {
            this.files = files;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getTag() {
            return tag;
        }

        public void setTag(String tag) {
            this.tag = tag;
        }

        public String getCourseId() {
            return courseId;
        }

        public void setCourseId(String courseId) {
            this.courseId = courseId;
        }

        public User getUser() {
            return user;
        }

        public void setUser(User user) {
            this.user = user;
        }

        public String getFileId() {
            return fileId;
        }

        public void setFileId(String fileId) {
            this.fileId = fileId;
        }

        @Override
        public String toString() {
            return "ClassPojo [quiz = " + quiz + ", chapter = " + chapter + ", creatorUserId = " + creatorUserId + ", comments = " + comments + ", quizId = " + quizId + ", isRead = " + isRead + ", lesson = " + lesson + ", discussion = " + discussion + ", type = " + type + ", userId = " + userId + ", assignmentId = " + assignmentId + ", lessionId = " + lessionId + ", dateCreated = " + dateCreated + ", discussionId = " + discussionId + ", chapterId = " + chapterId + ", commentId = " + commentId + ", course = " + course + ", files = " + files + ", id = " + id + ", tag = " + tag + ", courseId = " + courseId + ", user = " + user + ", fileId = " + fileId + "]";
        }
    }

    public class User {

        private String rolename;

        private String roleid;

        private String profilepicurl;

        private String phonenumber;

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

        public String getPhonenumber() {
            return phonenumber;
        }

        public void setPhonenumber(String phonenumber) {
            this.phonenumber = phonenumber;
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
            return "ClassPojo [rolename = " + rolename + ", roleid = " + roleid + ", profilepicurl = " + profilepicurl + ", phonenumber = " + phonenumber + ", bio = " + bio + ", id = " + id + ", fullname = " + fullname + ", is_skippable = " + is_skippable + ", email = " + email + ", username = " + username + "]";
        }
    }

    public class Course {
        private String image;

        private String code;

        private String name;

        private String description;

        private String id;

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

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
            return "ClassPojo [image = " + image + ", code = " + code + ", name = " + name + ", description = " + description + ", id = " + id + "]";
        }
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

        private String name;

        private String id;

        private String courseid;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getCourseid() {
            return courseid;
        }

        public void setCourseid(String courseid) {
            this.courseid = courseid;
        }

        @Override
        public String toString() {
            return "ClassPojo [name = " + name + ", id = " + id + ", courseid = " + courseid + "]";
        }
    }

    public class Quiz {
        private String name;

        private String id;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        @Override
        public String toString() {
            return "ClassPojo [name = " + name + ", id = " + id + "]";
        }
    }

    public class Comments {
        private String topicid;

        private String createtime;

        private String comment;

        private String id;

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
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        @Override
        public String toString() {
            return "ClassPojo [topicid = " + topicid + ", createtime = " + createtime + ", comment = " + comment + ", id = " + id + "]";
        }
    }

    public class Discussion {
        //iseditable likecount dislikecount liked disliked
        private String isprivate;

        private String description;

        private String id;

        private String title;

        private String courseid;

        private boolean iseditable;

        private int likecount;

        private int dislikecount;

        private boolean liked;

        private boolean disliked;

        public boolean isIseditable() {
            return iseditable;
        }

        public void setIseditable(boolean iseditable) {
            this.iseditable = iseditable;
        }

        public int getLikecount() {
            return likecount;
        }

        public void setLikecount(int likecount) {
            this.likecount = likecount;
        }

        public int getDislikecount() {
            return dislikecount;
        }

        public void setDislikecount(int dislikecount) {
            this.dislikecount = dislikecount;
        }

        public boolean isLiked() {
            return liked;
        }

        public void setLiked(boolean liked) {
            this.liked = liked;
        }

        public boolean isDisliked() {
            return disliked;
        }

        public void setDisliked(boolean disliked) {
            this.disliked = disliked;
        }

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


}


package com.ibl.apps.Model;

import androidx.annotation.NonNull;

import java.util.ArrayList;

public class GetAllComment {

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

    @NonNull
    @Override
    public String toString() {
        return "ClassPojo [response_code = " + response_code + ", data = " + data + ", message = " + message + ", status = " + status + "]";
    }

    public static class Data {
        private String topicid;

        private String createtime;

        private String filesid;

        private ArrayList<Files> files;

        private String comment;

        private String id;

        private User user;

        private boolean ispublic;

        private int likecount;

        private int dislikecount;

        private boolean liked;

        private boolean disliked;

        public boolean isIspublic() {
            return ispublic;
        }

        public void setIspublic(boolean ispublic) {
            this.ispublic = ispublic;
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

        public String getFilesid() {
            return filesid;
        }

        public void setFilesid(String filesid) {
            this.filesid = filesid;
        }

        public ArrayList<Files> getFiles() {
            return files;
        }

        public void setFiles(ArrayList<Files> files) {
            this.files = files;
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

        public User getUser() {
            return user;
        }

        public void setUser(User user) {
            this.user = user;
        }

        @Override
        public String toString() {
            return "ClassPojo [topicid = " + topicid + ", createtime = " + createtime + ", filesid = " + filesid + ", files = " + files + ", comment = " + comment + ", id = " + id + ", user = " + user + "]";
        }
    }

    public class Files {
        private String duration;

        private String fileName;

        private String fileSize;

        private String name;

        private String totalPages;

        private String commentId;

        private String description;

        private String id;

        private String fileTypeId;

        private String signedUrl;

        private String url;

        public String getDuration() {
            return duration;
        }

        public void setDuration(String duration) {
            this.duration = duration;
        }

        public String getFileName() {
            return fileName;
        }

        public void setFileName(String fileName) {
            this.fileName = fileName;
        }

        public String getFileSize() {
            return fileSize;
        }

        public void setFileSize(String fileSize) {
            this.fileSize = fileSize;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getTotalPages() {
            return totalPages;
        }

        public void setTotalPages(String totalPages) {
            this.totalPages = totalPages;
        }

        public String getCommentId() {
            return commentId;
        }

        public void setCommentId(String commentId) {
            this.commentId = commentId;
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

        public String getFileTypeId() {
            return fileTypeId;
        }

        public void setFileTypeId(String fileTypeId) {
            this.fileTypeId = fileTypeId;
        }

        public String getSignedUrl() {
            return signedUrl;
        }

        public void setSignedUrl(String signedUrl) {
            this.signedUrl = signedUrl;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        @Override
        public String toString() {
            return "ClassPojo [duration = " + duration + ", fileName = " + fileName + ", fileSize = " + fileSize + ", name = " + name + ", totalPages = " + totalPages + ", commentId = " + commentId + ", description = " + description + ", id = " + id + ", fileTypeId = " + fileTypeId + ", signedUrl = " + signedUrl + ", url = " + url + "]";
        }
    }

    public class User {
        private String reminder;

        private String roles;

        private String phonenumber;

        private String fullName;

        private String bio;

        private String istimeouton;

        private String is_skippable;

        private String timeout;

        private String intervals;

        private String roleName;

        private String profilepicurl;

        private String id;

        private String email;

        private String username;

        public String getReminder() {
            return reminder;
        }

        public void setReminder(String reminder) {
            this.reminder = reminder;
        }

        public String getRoles() {
            return roles;
        }

        public void setRoles(String roles) {
            this.roles = roles;
        }

        public String getPhonenumber() {
            return phonenumber;
        }

        public void setPhonenumber(String phonenumber) {
            this.phonenumber = phonenumber;
        }

        public String getFullName() {
            return fullName;
        }

        public void setFullName(String fullName) {
            this.fullName = fullName;
        }

        public String getBio() {
            return bio;
        }

        public void setBio(String bio) {
            this.bio = bio;
        }

        public String getIstimeouton() {
            return istimeouton;
        }

        public void setIstimeouton(String istimeouton) {
            this.istimeouton = istimeouton;
        }

        public String getIs_skippable() {
            return is_skippable;
        }

        public void setIs_skippable(String is_skippable) {
            this.is_skippable = is_skippable;
        }

        public String getTimeout() {
            return timeout;
        }

        public void setTimeout(String timeout) {
            this.timeout = timeout;
        }

        public String getIntervals() {
            return intervals;
        }

        public void setIntervals(String intervals) {
            this.intervals = intervals;
        }

        public String getRoleName() {
            return roleName;
        }

        public void setRoleName(String roleName) {
            this.roleName = roleName;
        }

        public String getProfilepicurl() {
            return profilepicurl;
        }

        public void setProfilepicurl(String profilepicurl) {
            this.profilepicurl = profilepicurl;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
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
            return "ClassPojo [reminder = " + reminder + ", roles = " + roles + ", phonenumber = " + phonenumber + ", fullName = " + fullName + ", bio = " + bio + ", istimeouton = " + istimeouton + ", is_skippable = " + is_skippable + ", timeout = " + timeout + ", intervals = " + intervals + ", roleName = " + roleName + ", profilepicurl = " + profilepicurl + ", id = " + id + ", email = " + email + ", username = " + username + "]";
        }
    }


}

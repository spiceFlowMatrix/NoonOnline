package com.ibl.apps.Model;

public class AddComment {


    private String response_code;

    private Data data;

    private String message;

    private String status;

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
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

    public class Data
    {
        private String topicid;

        private String comment;

        private String id;

        private User user;

        public String getTopicid() {
            return topicid;
        }

        public void setTopicid(String topicid) {
            this.topicid = topicid;
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
            return "ClassPojo [topicid = " + topicid + ", comment = " + comment + ", id = " + id + ", user = " + user + "]";
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

        public String

        getRoles() {
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

        public String

        getBio() {
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

        public String

        getRoleName() {
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

package com.ibl.apps.Model;


import java.util.List;

public class UserObject {

    private String message;

    private String status;

    private String response_code;

    private Data data;

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

    @Override
    public String toString() {
        return "ClassPojo [message = " + message + ", status = " + status + ", response_code = " + response_code + ", data = " + data + "]";
    }

    public class Data {


        private String phonenumber;

        private String reminder;

        private List<String> roleName = null;

        private String intervals;

        private String istimeouton;

        private String id;

        private String username;

        private String bio;

        private String profilepicurl;

        private String email;

        private List<String> roles = null;

        private String fullName;

        private String is_skippable;

        private String timeout;

        private String is_discussion_authorized;

        private String is_assignment_authorized;

        private String is_library_authorized;

        public String getIs_assignment_authorized() {
            return is_assignment_authorized;
        }

        public void setIs_assignment_authorized(String is_assignment_authorized) {
            this.is_assignment_authorized = is_assignment_authorized;
        }

        public String getIs_library_authorized() {
            return is_library_authorized;
        }

        public void setIs_library_authorized(String is_library_authorized) {
            this.is_library_authorized = is_library_authorized;
        }

        public String getPhonenumber() {
            return phonenumber;
        }

        public void setPhonenumber(String phonenumber) {
            this.phonenumber = phonenumber;
        }

        public String getReminder() {
            return reminder;
        }

        public void setReminder(String reminder) {
            this.reminder = reminder;
        }

        public String getIntervals() {
            return intervals;
        }

        public void setIntervals(String intervals) {
            this.intervals = intervals;
        }

        public String getIstimeouton() {
            return istimeouton;
        }

        public void setIstimeouton(String istimeouton) {
            this.istimeouton = istimeouton;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        public String getBio() {
            return bio;
        }

        public void setBio(String bio) {
            this.bio = bio;
        }

        public String getProfilepicurl() {
            return profilepicurl;
        }

        public void setProfilepicurl(String profilepicurl) {
            this.profilepicurl = profilepicurl;
        }

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public List<String> getRoleName() {
            return roleName;
        }

        public void setRoleName(List<String> roleName) {
            this.roleName = roleName;
        }

        public List<String> getRoles() {
            return roles;
        }

        public void setRoles(List<String> roles) {
            this.roles = roles;
        }

        public String getFullName() {
            return fullName;
        }

        public void setFullName(String fullName) {
            this.fullName = fullName;
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

        public String getIs_discussion_authorized() {
            return is_discussion_authorized;
        }

        public void setIs_discussion_authorized(String is_discussion_authorized) {
            this.is_discussion_authorized = is_discussion_authorized;
        }

        @Override
        public String toString() {
            return "FillesData{" +
                    "phonenumber='" + phonenumber + '\'' +
                    ", reminder='" + reminder + '\'' +
                    ", roleName=" + roleName +
                    ", intervals='" + intervals + '\'' +
                    ", istimeouton='" + istimeouton + '\'' +
                    ", id='" + id + '\'' +
                    ", username='" + username + '\'' +
                    ", bio='" + bio + '\'' +
                    ", profilepicurl='" + profilepicurl + '\'' +
                    ", email='" + email + '\'' +
                    ", roles=" + roles +
                    ", fullName='" + fullName + '\'' +
                    ", is_skippable='" + is_skippable + '\'' +
                    ", timeout='" + timeout + '\'' +
                    ", is_discussion_authorized='" + is_discussion_authorized + '\'' +
                    ", is_assignment_authorized='" + is_assignment_authorized + '\'' +
                    ", is_library_authorized='" + is_library_authorized + '\'' +
                    '}';
        }
    }
}

package com.ibl.apps.Model.parent;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

@SuppressWarnings("unused")
public class ParentSpinnerModel {

    @Expose
    private List<Data> data;
    @Expose
    private String message;
    @SerializedName("response_code")
    private Long responseCode;
    @Expose
    private String status;

    public List<Data> getData() {
        return data;
    }

    public void setData(List<Data> data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Long getResponseCode() {
        return responseCode;
    }

    public void setResponseCode(Long responseCode) {
        this.responseCode = responseCode;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    @SuppressWarnings("unused")
    public static class Data {

        @Expose
        private Object bio;
        @Expose
        private String email;
        @Expose
        private Object fullName;
        @Expose
        private Long id;
        @Expose
        private Object profilepicurl;
        @Expose
        private List<String> roleName;
        @Expose
        private List<Long> roles;
        @Expose
        private String username;

        public Object getBio() {
            return bio;
        }

        public void setBio(Object bio) {
            this.bio = bio;
        }

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public Object getFullName() {
            return fullName;
        }

        public void setFullName(Object fullName) {
            this.fullName = fullName;
        }

        public Long getId() {
            return id;
        }

        public void setId(Long id) {
            this.id = id;
        }

        public Object getProfilepicurl() {
            return profilepicurl;
        }

        public void setProfilepicurl(Object profilepicurl) {
            this.profilepicurl = profilepicurl;
        }

        public List<String> getRoleName() {
            return roleName;
        }

        public void setRoleName(List<String> roleName) {
            this.roleName = roleName;
        }

        public List<Long> getRoles() {
            return roles;
        }

        public void setRoles(List<Long> roles) {
            this.roles = roles;
        }

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        @Override
        public String toString() {
            return username;
        }
    }
}

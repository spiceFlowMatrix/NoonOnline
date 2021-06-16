
package com.ibl.apps.Model.parent;

import com.google.gson.annotations.SerializedName;

import java.util.List;

@SuppressWarnings("unused")
public class ParentGraphPoints {

    @SerializedName("data")
    private List<Data> mData;
    @SerializedName("message")
    private String mMessage;
    @SerializedName("response_code")
    private Long mResponseCode;
    @SerializedName("status")
    private String mStatus;

    public List<Data> getData() {
        return mData;
    }

    public void setData(List<Data> data) {
        mData = data;
    }

    public String getMessage() {
        return mMessage;
    }

    public void setMessage(String message) {
        mMessage = message;
    }

    public Long getResponseCode() {
        return mResponseCode;
    }

    public void setResponseCode(Long responseCode) {
        mResponseCode = responseCode;
    }

    public String getStatus() {
        return mStatus;
    }

    public void setStatus(String status) {
        mStatus = status;
    }

    @SuppressWarnings("unused")
    public static class Data {

        @SerializedName("hours")
        private Double mHours;
        @SerializedName("userDetails")
        private UserDetails mUserDetails;

        public Double getHours() {
            return mHours;
        }

        public void setHours(Double hours) {
            mHours = hours;
        }

        public UserDetails getUserDetails() {
            return mUserDetails;
        }

        public void setUserDetails(UserDetails userDetails) {
            mUserDetails = userDetails;
        }

    }

    @Override
    public String toString() {
        return "ParentGraphPoints{" +
                "mData=" + mData +
                ", mMessage='" + mMessage + '\'' +
                ", mResponseCode=" + mResponseCode +
                ", mStatus='" + mStatus + '\'' +
                '}';
    }

    @SuppressWarnings("unused")
    public static class UserDetails {

        @SerializedName("bio")
        private Object mBio;
        @SerializedName("email")
        private String mEmail;
        @SerializedName("fullName")
        private String mFullName;
        @SerializedName("id")
        private Long mId;
        @SerializedName("profilepicurl")
        private Object mProfilepicurl;
        @SerializedName("roleName")
        private List<String> mRoleName;
        @SerializedName("roles")
        private List<Long> mRoles;
        @SerializedName("username")
        private String mUsername;

        public Object getBio() {
            return mBio;
        }

        public void setBio(Object bio) {
            mBio = bio;
        }

        public String getEmail() {
            return mEmail;
        }

        public void setEmail(String email) {
            mEmail = email;
        }

        public String getFullName() {
            return mFullName;
        }

        public void setFullName(String fullName) {
            mFullName = fullName;
        }

        public Long getId() {
            return mId;
        }

        public void setId(Long id) {
            mId = id;
        }

        public Object getProfilepicurl() {
            return mProfilepicurl;
        }

        public void setProfilepicurl(Object profilepicurl) {
            mProfilepicurl = profilepicurl;
        }

        public List<String> getRoleName() {
            return mRoleName;
        }

        public void setRoleName(List<String> roleName) {
            mRoleName = roleName;
        }

        public List<Long> getRoles() {
            return mRoles;
        }

        public void setRoles(List<Long> roles) {
            mRoles = roles;
        }

        public String getUsername() {
            return mUsername;
        }

        public void setUsername(String username) {
            mUsername = username;
        }

    }
}

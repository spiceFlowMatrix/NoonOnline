package com.ibl.apps.Model;

import android.arch.persistence.room.ColumnInfo;
import android.arch.persistence.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.List;

public class DiscssionsAllTopics {

    private String response_code;

    @ColumnInfo(name = "ListData")
    @TypeConverters(DataTypeConverter.class)
    private List<Data> data = null;

    private String message;

    private String status;

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

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

    public static class Data {
        private int courseid;
        private String createdUserId;

        private String comments;

        private String description;

        private String id;

        private String isPrivate;

        private String isprivate;

        private String title;

        private String createddate;

        private boolean ispublic;

        private User user;

        private boolean iseditable;

        private int likecount;

        private int dislikecount;

        private boolean liked;

        private boolean disliked;

        public String getCreatedUserId() {
            return createdUserId;
        }

        public void setCreatedUserId(String createdUserId) {
            this.createdUserId = createdUserId;
        }

        public String getComments() {
            return comments;
        }

        public void setComments(String comments) {
            this.comments = comments;
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

        public String getIsPrivate() {
            return isPrivate;
        }

        public void setIsPrivate(String isPrivate) {
            this.isPrivate = isPrivate;
        }

        public String getTitle() {
            return title;
        }

        public void setTitle(String title) {
            this.title = title;
        }

        @Override
        public String toString() {
            return "ClassPojo [createdUserId = " + createdUserId + ", comments = " + comments + ", description = " + description + ", id = " + id + ", isPrivate = " + isPrivate + ", title = " + title + "]";
        }

        public String getCreateddate() {
            return createddate;
        }

        public void setCreateddate(String createddate) {
            this.createddate = createddate;
        }

        public boolean isIspublic() {
            return ispublic;
        }

        public void setIspublic(boolean ispublic) {
            this.ispublic = ispublic;
        }

        public User getUser() {
            return user;
        }

        public void setUser(User user) {
            this.user = user;
        }

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

        public int getCourseid() {
            return courseid;
        }

        public void setCourseid(int courseid) {
            this.courseid = courseid;
        }

        public String getIsprivate() {
            return isprivate;
        }

        public void setIsprivate(String isprivate) {
            this.isprivate = isprivate;
        }
    }

    public static class User {
        int id;

        String username;

        String fullName;

        String roleName;

        String email;

        String roles;

        String bio;

        String profilepicurl;

        boolean is_skippable;

        int timeout;

        int reminder;

        int intervals;

        boolean istimeouton;

        String phonenumber;

        boolean is_discussion_authorized;

        boolean is_library_authorized;

        boolean is_assignment_authorized;

        int salesagentId;

        int agentCategoryId;

        int agreedMonthlyDeposit;

        String currencyCode;

        String focalPoint;

        String partnerBackgroud;

        String physicalAddress;

        String position;

        boolean isactive;

        public int getId() {
            return id;
        }

        public void setId(int id) {
            this.id = id;
        }

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        public String getFullName() {
            return fullName;
        }

        public void setFullName(String fullName) {
            this.fullName = fullName;
        }

        public String getRoleName() {
            return roleName;
        }

        public void setRoleName(String roleName) {
            this.roleName = roleName;
        }

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public String getRoles() {
            return roles;
        }

        public void setRoles(String roles) {
            this.roles = roles;
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

        public boolean isIs_skippable() {
            return is_skippable;
        }

        public void setIs_skippable(boolean is_skippable) {
            this.is_skippable = is_skippable;
        }

        public int getTimeout() {
            return timeout;
        }

        public void setTimeout(int timeout) {
            this.timeout = timeout;
        }

        public int getReminder() {
            return reminder;
        }

        public void setReminder(int reminder) {
            this.reminder = reminder;
        }

        public int getIntervals() {
            return intervals;
        }

        public void setIntervals(int intervals) {
            this.intervals = intervals;
        }

        public boolean isIstimeouton() {
            return istimeouton;
        }

        public void setIstimeouton(boolean istimeouton) {
            this.istimeouton = istimeouton;
        }

        public String getPhonenumber() {
            return phonenumber;
        }

        public void setPhonenumber(String phonenumber) {
            this.phonenumber = phonenumber;
        }

        public boolean isIs_discussion_authorized() {
            return is_discussion_authorized;
        }

        public void setIs_discussion_authorized(boolean is_discussion_authorized) {
            this.is_discussion_authorized = is_discussion_authorized;
        }

        public boolean isIs_library_authorized() {
            return is_library_authorized;
        }

        public void setIs_library_authorized(boolean is_library_authorized) {
            this.is_library_authorized = is_library_authorized;
        }

        public boolean isIs_assignment_authorized() {
            return is_assignment_authorized;
        }

        public void setIs_assignment_authorized(boolean is_assignment_authorized) {
            this.is_assignment_authorized = is_assignment_authorized;
        }

        public int getSalesagentId() {
            return salesagentId;
        }

        public void setSalesagentId(int salesagentId) {
            this.salesagentId = salesagentId;
        }

        public int getAgentCategoryId() {
            return agentCategoryId;
        }

        public void setAgentCategoryId(int agentCategoryId) {
            this.agentCategoryId = agentCategoryId;
        }

        public int getAgreedMonthlyDeposit() {
            return agreedMonthlyDeposit;
        }

        public void setAgreedMonthlyDeposit(int agreedMonthlyDeposit) {
            this.agreedMonthlyDeposit = agreedMonthlyDeposit;
        }

        public String getCurrencyCode() {
            return currencyCode;
        }

        public void setCurrencyCode(String currencyCode) {
            this.currencyCode = currencyCode;
        }

        public String getFocalPoint() {
            return focalPoint;
        }

        public void setFocalPoint(String focalPoint) {
            this.focalPoint = focalPoint;
        }

        public String getPartnerBackgroud() {
            return partnerBackgroud;
        }

        public void setPartnerBackgroud(String partnerBackgroud) {
            this.partnerBackgroud = partnerBackgroud;
        }

        public String getPhysicalAddress() {
            return physicalAddress;
        }

        public void setPhysicalAddress(String physicalAddress) {
            this.physicalAddress = physicalAddress;
        }

        public String getPosition() {
            return position;
        }

        public void setPosition(String position) {
            this.position = position;
        }

        public boolean isIsactive() {
            return isactive;
        }

        public void setIsactive(boolean isactive) {
            this.isactive = isactive;
        }
    }
}

package com.ibl.apps.Model;

import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by iblinfotech on 31/10/18.
 */

@Entity
public class TrileSignupObject {

    @PrimaryKey(autoGenerate = true)
    private int signupid;

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

    public class Data {
        private String is_assignment_authorized;

        private String[] roles;

        private String isactive;

        private String phonenumber;

        private String bio;

        private String istimeouton;

        private String is_skippable;

        private String focalPoint;

        private String timeout;

        private String partnerBackgroud;

        private String profilepicurl;

        private String physicalAddress;

        private String id;

        private String email;

        private String is_discussion_authorized;

        private String reminder;

        private String is_library_authorized;

        private String fullName;

        private String salesagentId;

        private String intervals;

        private String agreedMonthlyDeposit;

        private String[] roleName;

        private String position;

        private String currencyCode;

        private String agentCategoryId;

        private String username;

        public String getIs_assignment_authorized() {
            return is_assignment_authorized;
        }

        public void setIs_assignment_authorized(String is_assignment_authorized) {
            this.is_assignment_authorized = is_assignment_authorized;
        }

        public String[] getRoles() {
            return roles;
        }

        public void setRoles(String[] roles) {
            this.roles = roles;
        }

        public String getIsactive() {
            return isactive;
        }

        public void setIsactive(String isactive) {
            this.isactive = isactive;
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

        public String getFocalPoint() {
            return focalPoint;
        }

        public void setFocalPoint(String focalPoint) {
            this.focalPoint = focalPoint;
        }

        public String getTimeout() {
            return timeout;
        }

        public void setTimeout(String timeout) {
            this.timeout = timeout;
        }

        public String getPartnerBackgroud() {
            return partnerBackgroud;
        }

        public void setPartnerBackgroud(String partnerBackgroud) {
            this.partnerBackgroud = partnerBackgroud;
        }

        public String getProfilepicurl() {
            return profilepicurl;
        }

        public void setProfilepicurl(String profilepicurl) {
            this.profilepicurl = profilepicurl;
        }

        public String getPhysicalAddress() {
            return physicalAddress;
        }

        public void setPhysicalAddress(String physicalAddress) {
            this.physicalAddress = physicalAddress;
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

        public String getIs_discussion_authorized() {
            return is_discussion_authorized;
        }

        public void setIs_discussion_authorized(String is_discussion_authorized) {
            this.is_discussion_authorized = is_discussion_authorized;
        }

        public String getReminder() {
            return reminder;
        }

        public void setReminder(String reminder) {
            this.reminder = reminder;
        }

        public String getIs_library_authorized() {
            return is_library_authorized;
        }

        public void setIs_library_authorized(String is_library_authorized) {
            this.is_library_authorized = is_library_authorized;
        }

        public String getFullName() {
            return fullName;
        }

        public void setFullName(String fullName) {
            this.fullName = fullName;
        }

        public String getSalesagentId() {
            return salesagentId;
        }

        public void setSalesagentId(String salesagentId) {
            this.salesagentId = salesagentId;
        }

        public String getIntervals() {
            return intervals;
        }

        public void setIntervals(String intervals) {
            this.intervals = intervals;
        }

        public String getAgreedMonthlyDeposit() {
            return agreedMonthlyDeposit;
        }

        public void setAgreedMonthlyDeposit(String agreedMonthlyDeposit) {
            this.agreedMonthlyDeposit = agreedMonthlyDeposit;
        }

        public String[] getRoleName() {
            return roleName;
        }

        public void setRoleName(String[] roleName) {
            this.roleName = roleName;
        }

        public String getPosition() {
            return position;
        }

        public void setPosition(String position) {
            this.position = position;
        }

        public String getCurrencyCode() {
            return currencyCode;
        }

        public void setCurrencyCode(String currencyCode) {
            this.currencyCode = currencyCode;
        }

        public String getAgentCategoryId() {
            return agentCategoryId;
        }

        public void setAgentCategoryId(String agentCategoryId) {
            this.agentCategoryId = agentCategoryId;
        }

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        @Override
        public String toString() {
            return "ClassPojo [is_assignment_authorized = " + is_assignment_authorized + ", roles = " + roles + ", isactive = " + isactive + ", phonenumber = " + phonenumber + ", bio = " + bio + ", istimeouton = " + istimeouton + ", is_skippable = " + is_skippable + ", focalPoint = " + focalPoint + ", timeout = " + timeout + ", partnerBackgroud = " + partnerBackgroud + ", profilepicurl = " + profilepicurl + ", physicalAddress = " + physicalAddress + ", id = " + id + ", email = " + email + ", is_discussion_authorized = " + is_discussion_authorized + ", reminder = " + reminder + ", is_library_authorized = " + is_library_authorized + ", fullName = " + fullName + ", salesagentId = " + salesagentId + ", intervals = " + intervals + ", agreedMonthlyDeposit = " + agreedMonthlyDeposit + ", roleName = " + roleName + ", position = " + position + ", currencyCode = " + currencyCode + ", agentCategoryId = " + agentCategoryId + ", username = " + username + "]";
        }
    }


}

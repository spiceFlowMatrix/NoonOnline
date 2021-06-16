package com.ibl.apps.Model;

import java.util.ArrayList;

public class DiscussionsDetails {

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
        private String isprivate;

        private String comments;

        private String createduserid;

        private String filesid;

        private String description;

        private ArrayList<Files> files;

        private String id;

        private User user;

        private String title;
        private int courseid;
        private boolean ispublic;
        private boolean iseditable;
        private int likecount;
        private int dislikecount;
        private boolean liked;
        private boolean disliked;
        private String createddate;

        public String getIsprivate() {
            return isprivate;
        }

        public void setIsprivate(String isprivate) {
            this.isprivate = isprivate;
        }

        public String getComments() {
            return comments;
        }

        public void setComments(String comments) {
            this.comments = comments;
        }

        public String getCreateduserid() {
            return createduserid;
        }

        public void setCreateduserid(String createduserid) {
            this.createduserid = createduserid;
        }

        public String getFilesid() {
            return filesid;
        }

        public void setFilesid(String filesid) {
            this.filesid = filesid;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public ArrayList<Files> getFiles() {
            return files;
        }

        public void setFiles(ArrayList<Files> files) {
            this.files = files;
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

        public User getUser() {
            return user;
        }

        public void setUser(User user) {
            this.user = user;
        }

        public int getCourseid() {
            return courseid;
        }

        public void setCourseid(int courseid) {
            this.courseid = courseid;
        }

        public boolean isIspublic() {
            return ispublic;
        }

        public void setIspublic(boolean ispublic) {
            this.ispublic = ispublic;
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

        @Override
        public String toString() {
            return "ClassPojo [isprivate = " + isprivate + ", comments = " + comments + ", createduserid = " + createduserid + ", filesid = " + filesid + ", description = " + description + ", files = " + files + ", id = " + id + ", title = " + title + "]";
        }

        public String getCreateddate() {
            return createddate;
        }

        public void setCreateddate(String createddate) {
            this.createddate = createddate;
        }
    }

    public static class Files {

        private String duration;

        private String topicId;

        private String fileName;

        private String fileSize;

        private String name;

        private String totalPages;

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

        public String getTopicId() {
            return topicId;
        }

        public void setTopicId(String topicId) {
            this.topicId = topicId;
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
            return "ClassPojo [duration = " + duration + ", topicId = " + topicId + ", fileName = " + fileName + ", fileSize = " + fileSize + ", name = " + name + ", totalPages = " + totalPages + ", description = " + description + ", id = " + id + ", fileTypeId = " + fileTypeId + ", signedUrl = " + signedUrl + ", url = " + url + "]";
        }
    }

    public class User {

        private int id;
        private String username;
        private String fullName;
        private String roleName;
        private String email;
        private String roles;
        private String bio;
        private String profilepicurl;
        private boolean is_skippable;
        private int timeout;
        private int reminder;
        private int intervals;
        private boolean istimeouton;
        private String phonenumber;
        private boolean is_discussion_authorized;
        private boolean is_library_authorized;
        private boolean is_assignment_authorized;
        private int salesagentId;
        private int agentCategoryId;
        private int agreedMonthlyDeposit;
        private int currencyCode;
        private int focalPoint;
        private String partnerBackgroud;
        private String physicalAddress;
        private String position;
        private boolean isactive;

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

        public int getCurrencyCode() {
            return currencyCode;
        }

        public void setCurrencyCode(int currencyCode) {
            this.currencyCode = currencyCode;
        }

        public int getFocalPoint() {
            return focalPoint;
        }

        public void setFocalPoint(int focalPoint) {
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

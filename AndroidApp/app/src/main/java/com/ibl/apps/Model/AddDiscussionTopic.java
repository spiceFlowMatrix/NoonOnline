package com.ibl.apps.Model;

import java.util.ArrayList;

public class AddDiscussionTopic {

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

        private String[] filesid;

        private String description;

        private ArrayList<Files> files;

        private String id;

        private String title;

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

        public String[] getFilesid() {
            return filesid;
        }

        public void setFilesid(String[] filesid) {
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

        @Override
        public String toString() {
            return "ClassPojo [isprivate = " + isprivate + ", comments = " + comments + ", createduserid = " + createduserid + ", filesid = " + filesid + ", description = " + description + ", files = " + files + ", id = " + id + ", title = " + title + "]";
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


}



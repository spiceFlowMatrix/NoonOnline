package com.ibl.apps.Model;

/**
 * Created by iblinfotech on 07/12/18.
 */

public class AssignmentDetailObject {

    private String message;

    private String status;

    private String response_code;

    public Data data;

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

        private String id;

        private Chapter chapter;

        private String description;

        private String name;

        private Assignmentfiles[] assignmentfiles;

        private String code;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public Chapter getChapter() {
            return chapter;
        }

        public void setChapter(Chapter chapter) {
            this.chapter = chapter;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public Assignmentfiles[] getAssignmentfiles() {
            return assignmentfiles;
        }

        public void setAssignmentfiles(Assignmentfiles[] assignmentfiles) {
            this.assignmentfiles = assignmentfiles;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", chapter = " + chapter + ", description = " + description + ", name = " + name + ", assignmentfiles = " + assignmentfiles + ", code = " + code + "]";
        }
    }

    public static class Assignmentfiles {

        private String id;

        private Files files;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public Files getFiles() {
            return files;
        }

        public void setFiles(Files files) {
            this.files = files;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", files = " + files + "]";
        }
    }

    public static class Files {
        private String id;

        private String duration;

        private String filesize;

        private String description;

        private String name;

        private String totalpages;

        private String filetypename;

        private String filename;

        private String filetypeid;

        private String url;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getDuration() {
            return duration;
        }

        public void setDuration(String duration) {
            this.duration = duration;
        }

        public String getFilesize() {
            return filesize;
        }

        public void setFilesize(String filesize) {
            this.filesize = filesize;
        }

        public String getDescription() {
            return description;
        }

        public void setDescription(String description) {
            this.description = description;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getTotalpages() {
            return totalpages;
        }

        public void setTotalpages(String totalpages) {
            this.totalpages = totalpages;
        }

        public String getFiletypename() {
            return filetypename;
        }

        public void setFiletypename(String filetypename) {
            this.filetypename = filetypename;
        }

        public String getFilename() {
            return filename;
        }

        public void setFilename(String filename) {
            this.filename = filename;
        }

        public String getFiletypeid() {
            return filetypeid;
        }

        public void setFiletypeid(String filetypeid) {
            this.filetypeid = filetypeid;
        }

        public String getUrl() {
            return url;
        }

        public void setUrl(String url) {
            this.url = url;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", duration = " + duration + ", filesize = " + filesize + ", description = " + description + ", name = " + name + ", totalpages = " + totalpages + ", filetypename = " + filetypename + ", filename = " + filename + ", filetypeid = " + filetypeid + ", url = " + url + "]";
        }
    }

    public class Chapter {
        private String id;

        private String quizid;

        private String assignmentDetails;

        private String itemorder;

        private String name;

        private String code;

        private String courseid;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getQuizid() {
            return quizid;
        }

        public void setQuizid(String quizid) {
            this.quizid = quizid;
        }

        public String getAssignmentDetails() {
            return assignmentDetails;
        }

        public void setAssignmentDetails(String assignmentDetails) {
            this.assignmentDetails = assignmentDetails;
        }

        public String getItemorder() {
            return itemorder;
        }

        public void setItemorder(String itemorder) {
            this.itemorder = itemorder;
        }

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getCode() {
            return code;
        }

        public void setCode(String code) {
            this.code = code;
        }

        public String getCourseid() {
            return courseid;
        }

        public void setCourseid(String courseid) {
            this.courseid = courseid;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", quizid = " + quizid + ", assignmentDetails = " + assignmentDetails + ", itemorder = " + itemorder + ", name = " + name + ", code = " + code + ", courseid = " + courseid + "]";
        }
    }
}

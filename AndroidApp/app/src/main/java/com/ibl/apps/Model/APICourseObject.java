package com.ibl.apps.Model;

/**
 * Created by iblinfotech on 19/09/18.
 */

public class APICourseObject {

    private String totalcount;

    private String message;

    private String status;

    private String response_code;

    private Data[] data;

    public String getTotalcount() {
        return totalcount;
    }

    public void setTotalcount(String totalcount) {
        this.totalcount = totalcount;
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

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public Data[] getData() {
        return data;
    }

    public void setData(Data[] data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return "ClassPojo [totalcount = " + totalcount + ", message = " + message + ", status = " + status + ", response_code = " + response_code + ", data = " + data + "]";
    }

    public class Data {
        private String id;

        private String startDate;

        private String description;

        private String name;

        private String image;

        private String endDate;

        private String code;

        private String courseid;

        private String studentid;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getStartDate() {
            return startDate;
        }

        public void setStartDate(String startDate) {
            this.startDate = startDate;
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

        public String getImage() {
            return image;
        }

        public void setImage(String image) {
            this.image = image;
        }

        public String getEndDate() {
            return endDate;
        }

        public void setEndDate(String endDate) {
            this.endDate = endDate;
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

        public String getStudentid() {
            return studentid;
        }

        public void setStudentid(String studentid) {
            this.studentid = studentid;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", startDate = " + startDate + ", description = " + description + ", name = " + name + ", image = " + image + ", endDate = " + endDate + ", code = " + code + ", courseid = " + courseid + ", studentid = " + studentid + "]";
        }
    }
}

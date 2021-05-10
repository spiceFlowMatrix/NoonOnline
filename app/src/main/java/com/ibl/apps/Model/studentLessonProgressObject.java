package com.ibl.apps.Model;

/**
 * Created by iblinfotech on 27/09/18.
 */

public class studentLessonProgressObject {

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
        private String id;

        private String duration;

        private String lessonstatus;

        private String lessonprogress;

        private String lessonid;

        private String studentid;

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

        public String getLessonstatus() {
            return lessonstatus;
        }

        public void setLessonstatus(String lessonstatus) {
            this.lessonstatus = lessonstatus;
        }

        public String getLessonprogress() {
            return lessonprogress;
        }

        public void setLessonprogress(String lessonprogress) {
            this.lessonprogress = lessonprogress;
        }

        public String getLessonid() {
            return lessonid;
        }

        public void setLessonid(String lessonid) {
            this.lessonid = lessonid;
        }

        public String getStudentid() {
            return studentid;
        }

        public void setStudentid(String studentid) {
            this.studentid = studentid;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", duration = " + duration + ", lessonstatus = " + lessonstatus + ", lessonprogress = " + lessonprogress + ", lessonid = " + lessonid + ", studentid = " + studentid + "]";
        }
    }

}


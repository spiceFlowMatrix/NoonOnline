package com.ibl.apps.Model;

import java.util.ArrayList;

public class GradeSpinner {
    private String totalcount;

    private String response_code;

    private ArrayList<Grade> data;

    private String message;

    private String status;

    public String getTotalcount() {
        return totalcount;
    }

    public void setTotalcount(String totalcount) {
        this.totalcount = totalcount;
    }

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
    }

    public ArrayList<Grade> getGradeArrayList() {
        return data;
    }

    public void setGradeArrayList(ArrayList<Grade> gradeArrayList) {
        this.data = gradeArrayList;
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
        return "ClassPojo [totalcount = " + totalcount + ", response_code = " + response_code + ", data = " + data + ", message = " + message + ", status = " + status + "]";
    }


    public static class Grade {

        private String name;

        private String description;

        private String id;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
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

        @Override
        public String toString() {
            return name;
        }
    }
}

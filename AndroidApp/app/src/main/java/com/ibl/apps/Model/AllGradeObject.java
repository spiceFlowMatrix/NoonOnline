package com.ibl.apps.Model;

/**
 * Created by iblinfotech on 29/10/18.
 */

public class AllGradeObject {

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

    public static class Data {

        private String id;

        private String description;

        private String name;

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
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

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", description = " + description + ", name = " + name + "]";
        }
    }
}

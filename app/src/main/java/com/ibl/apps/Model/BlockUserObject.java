package com.ibl.apps.Model;


public class BlockUserObject {

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

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + "]";
        }
    }


}
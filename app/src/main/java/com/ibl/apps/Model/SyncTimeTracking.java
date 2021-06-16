package com.ibl.apps.Model;

public class SyncTimeTracking {
    private String response_code;

    private String message;

    private String status;
    private Data data;

    public String getResponse_code() {
        return response_code;
    }

    public void setResponse_code(String response_code) {
        this.response_code = response_code;
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

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
        this.data = data;
    }

    @Override
    public String toString() {
        return "ClassPojo [response_code = " + response_code + ", message = " + message + ", status = " + status + "]";
    }

    public class Data {

    }

}

package com.ibl.apps.Model;


import java.io.Serializable;

public class UpdateObject implements Serializable {

    private String message;

    private String status;

    private String response_code;

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

    @Override
    public String toString() {
        return "UpdateObject{" +
                "message='" + message + '\'' +
                ", status='" + status + '\'' +
                ", response_code='" + response_code + '\'' +
                '}';
    }
}
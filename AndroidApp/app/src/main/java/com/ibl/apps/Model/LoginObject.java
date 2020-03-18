package com.ibl.apps.Model;

import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by iblinfotech on 31/10/18.
 */

@Entity
public class LoginObject {

    @PrimaryKey(autoGenerate = true)
    private int loginid;

    private String message;

    private String token;

    private String status;

    private String response_code;

    private String email;

    private String password;

    private String userid;

    private String sub;

    public String getSub() {
        return sub;
    }

    public void setSub(String sub) {
        this.sub = sub;
    }

    public String getUserid() {
        return userid;
    }

    public void setUserid(String userid) {
        this.userid = userid;
    }

    public String getEmail() {
        return email;
    }

    public int getLoginid() {
        return loginid;
    }

    public void setLoginid(int loginid) {
        this.loginid = loginid;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getToken() {
        return token;
    }

    public void setToken(String token) {
        this.token = token;
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
        return "LoginObject{" +
                "loginid=" + loginid +
                ", message='" + message + '\'' +
                ", token='" + token + '\'' +
                ", status='" + status + '\'' +
                ", response_code='" + response_code + '\'' +
                ", email='" + email + '\'' +
                ", password='" + password + '\'' +
                ", userid='" + userid + '\'' +
                ", sub='" + sub + '\'' +
                '}';
    }
}

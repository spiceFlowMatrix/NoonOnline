package com.ibl.apps.devicemanagement;

import androidx.room.ColumnInfo;
import androidx.room.PrimaryKey;

public class User {
    @PrimaryKey(autoGenerate = true)
    private String no;

    private String firstName;

    private String lastName;

    private String password;

    private String userStatus;

    private String phone;

    @ColumnInfo(name = "uid")
    private String id;

    private String email;

    private String username;

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public String getPassword() {
        return password;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public String getUserStatus() {
        return userStatus;
    }

    public void setUserStatus(String userStatus) {
        this.userStatus = userStatus;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    @Override
    public String toString() {
        return "ClassPojo [firstName = " + firstName + ", lastName = " + lastName + ", password = " + password + ", userStatus = " + userStatus + ", phone = " + phone + ", id = " + id + ", email = " + email + ", username = " + username + "]";
    }

    public String getNo() {
        return no;
    }

    public void setNo(String no) {
        this.no = no;
    }
}


package com.ibl.apps.Model;

import com.google.gson.annotations.Expose;

public class User {

    @Expose
    private Object bio;
    @Expose
    private String email;
    @Expose
    private String fullName;
    @Expose
    private Long id;
    @Expose
    private Object profilepicurl;
    @Expose
    private Object roleName;
    @Expose
    private Object roles;
    @Expose
    private String username;

    public Object getBio() {
        return bio;
    }

    public void setBio(Object bio) {
        this.bio = bio;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Object getProfilepicurl() {
        return profilepicurl;
    }

    public void setProfilepicurl(Object profilepicurl) {
        this.profilepicurl = profilepicurl;
    }

    public Object getRoleName() {
        return roleName;
    }

    public void setRoleName(Object roleName) {
        this.roleName = roleName;
    }

    public Object getRoles() {
        return roles;
    }

    public void setRoles(Object roles) {
        this.roles = roles;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

}

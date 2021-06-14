
package com.ibl.apps.Model.feedback;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class User {

    @SerializedName("fullname")
    private String mFullname;
    @SerializedName("id")
    private Long mId;
    @SerializedName("profilepicurl")
    private String mProfilepicurl;
    @SerializedName("username")
    private String mUsername;

    public String getFullname() {
        return mFullname;
    }

    public void setFullname(String fullname) {
        mFullname = fullname;
    }

    public Long getId() {
        return mId;
    }

    public void setId(Long id) {
        mId = id;
    }

    public String getProfilepicurl() {
        return mProfilepicurl;
    }

    public void setProfilepicurl(String profilepicurl) {
        mProfilepicurl = profilepicurl;
    }

    public String getUsername() {
        return mUsername;
    }

    public void setUsername(String username) {
        mUsername = username;
    }

}

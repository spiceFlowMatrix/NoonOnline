package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.Arrays;
import java.util.List;

/**
 * Created by iblinfotech on 26/11/18.
 */

@Entity
public class UserDetails {

    @PrimaryKey(autoGenerate = true)
    private int userautoId;

    private String phonenumber = "";

    private String reminder = "";

    @ColumnInfo(name = "roleName")
    @TypeConverters(DataTypeConverter.class)
    private List<String> roleName = null;

    private String intervals = "";

    private String istimeouton = "";

    private String id = "0";

    private String username = "";

    private String bio = "";

    private String profilepicurl = "";

    private String email = "";

    @ColumnInfo(name = "roles")
    @TypeConverters(DataTypeConverter.class)
    private List<String> roles = null;

    private String fullName = "";

    private String is_skippable = "";

    private String timeout = "";

    private String sub = "";

    @ColumnInfo(typeAffinity = ColumnInfo.BLOB)
    private byte[] profilepicImage;

    private String is_discussion_authorized = "";

    private String is_assignment_authorized = "";

    private String is_library_authorized = "";
    private boolean isallowmap;

    public String getIs_assignment_authorized() {
        return is_assignment_authorized;
    }

    public void setIs_assignment_authorized(String is_assignment_authorized) {
        this.is_assignment_authorized = is_assignment_authorized;
    }

    public String getIs_library_authorized() {
        return is_library_authorized;
    }

    public void setIs_library_authorized(String is_library_authorized) {
        this.is_library_authorized = is_library_authorized;
    }

    public String getSub() {
        return sub;
    }

    public void setSub(String sub) {
        this.sub = sub;
    }

    public String getPhonenumber() {
        return phonenumber;
    }

    public void setPhonenumber(String phonenumber) {
        this.phonenumber = phonenumber;
    }

    public String getReminder() {
        return reminder;
    }

    public void setReminder(String reminder) {
        this.reminder = reminder;
    }

    public String getIntervals() {
        return intervals;
    }

    public void setIntervals(String intervals) {
        this.intervals = intervals;
    }

    public String getIstimeouton() {
        return istimeouton;
    }

    public void setIstimeouton(String istimeouton) {
        this.istimeouton = istimeouton;
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getBio() {
        return bio;
    }

    public void setBio(String bio) {
        this.bio = bio;
    }

    public String getProfilepicurl() {
        return profilepicurl;
    }

    public void setProfilepicurl(String profilepicurl) {
        this.profilepicurl = profilepicurl;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public int getUserautoId() {
        return userautoId;
    }

    public void setUserautoId(int userautoId) {
        this.userautoId = userautoId;
    }

    public List<String> getRoleName() {
        return roleName;
    }

    public void setRoleName(List<String> roleName) {
        this.roleName = roleName;
    }

    public List<String> getRoles() {
        return roles;
    }

    public void setRoles(List<String> roles) {
        this.roles = roles;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public String getIs_skippable() {
        return is_skippable;
    }

    public void setIs_skippable(String is_skippable) {
        this.is_skippable = is_skippable;
    }

    public String getTimeout() {
        return timeout;
    }

    public void setTimeout(String timeout) {
        this.timeout = timeout;
    }

    public byte[] getProfilepicImage() {
        return profilepicImage;
    }

    public void setProfilepicImage(byte[] profilepicImage) {
        this.profilepicImage = profilepicImage;
    }

    public String getIs_discussion_authorized() {
        return is_discussion_authorized;
    }

    public void setIs_discussion_authorized(String is_discussion_authorized) {
        this.is_discussion_authorized = is_discussion_authorized;
    }

    public boolean isIsallowmap() {
        return isallowmap;
    }

    public void setIsallowmap(boolean isallowmap) {
        this.isallowmap = isallowmap;
    }

    @Override
    public String toString() {
        return "UserDetails{" +
                "userautoId=" + userautoId +
                ", phonenumber='" + phonenumber + '\'' +
                ", reminder='" + reminder + '\'' +
                ", roleName=" + roleName +
                ", intervals='" + intervals + '\'' +
                ", istimeouton='" + istimeouton + '\'' +
                ", id='" + id + '\'' +
                ", username='" + username + '\'' +
                ", bio='" + bio + '\'' +
                ", profilepicurl='" + profilepicurl + '\'' +
                ", email='" + email + '\'' +
                ", roles=" + roles +
                ", fullName='" + fullName + '\'' +
                ", is_skippable='" + is_skippable + '\'' +
                ", timeout='" + timeout + '\'' +
                ", sub='" + sub + '\'' +
                ", profilepicImage=" + Arrays.toString(profilepicImage) +
                ", is_discussion_authorized='" + is_discussion_authorized + '\'' +
                ", is_assignment_authorized='" + is_assignment_authorized + '\'' +
                ", is_library_authorized='" + is_library_authorized + '\'' +
                ", isallowmap='" + isallowmap + '\'' +
                '}';
    }
}

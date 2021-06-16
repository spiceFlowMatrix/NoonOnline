package com.ibl.apps.RoomDatabase.entity;

import androidx.room.ColumnInfo;
import androidx.room.Entity;
import androidx.room.PrimaryKey;

@Entity
public class SyncTimeTrackingObject {
    @PrimaryKey(autoGenerate = true)
    public int id;

    @ColumnInfo(name = "latitude")
    private String latitude;

    @ColumnInfo(name = "longitude")
    private String longitude;

    @ColumnInfo(name = "serviceprovider")
    private String serviceprovider;

    @ColumnInfo(name = "school")
    private String school;

    @ColumnInfo(name = "subjectstaken")
    private String subjectstaken;

    @ColumnInfo(name = "grade")
    private String grade;

    @ColumnInfo(name = "hardwareplatform")
    private String hardwareplatform;

    @ColumnInfo(name = "operatingsystem")
    private String operatingsystem;

    @ColumnInfo(name = "version")
    private String version;

    @ColumnInfo(name = "activitytime")
    private String activitytime;

    @ColumnInfo(name = "outtime")
    private String outtime;

    private int userid;

    private String isp;

    private String networkspeed;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getLatitude() {
        return latitude;
    }

    public void setLatitude(String latitude) {
        this.latitude = latitude;
    }

    public String getLongitude() {
        return longitude;
    }

    public void setLongitude(String longitude) {
        this.longitude = longitude;
    }

    public String getServiceprovider() {
        return serviceprovider;
    }

    public void setServiceprovider(String serviceprovider) {
        this.serviceprovider = serviceprovider;
    }

    public String getSchool() {
        return school;
    }

    public void setSchool(String school) {
        this.school = school;
    }

    public String getSubjectstaken() {
        return subjectstaken;
    }

    public void setSubjectstaken(String subjectstaken) {
        this.subjectstaken = subjectstaken;
    }

    public String getGrade() {
        return grade;
    }

    public void setGrade(String grade) {
        this.grade = grade;
    }

    public String getHardwareplatform() {
        return hardwareplatform;
    }

    public void setHardwareplatform(String hardwareplatform) {
        this.hardwareplatform = hardwareplatform;
    }

    public String getOperatingsystem() {
        return operatingsystem;
    }

    public void setOperatingsystem(String operatingsystem) {
        this.operatingsystem = operatingsystem;
    }

    public String getVersion() {
        return version;
    }

    public void setVersion(String version) {
        this.version = version;
    }


    public String getActivitytime() {
        return activitytime;
    }

    public void setActivitytime(String activitytime) {
        this.activitytime = activitytime;
    }

    public String getOuttime() {
        return outtime;
    }

    public void setOuttime(String outtime) {
        this.outtime = outtime;
    }

    public int getUserid() {
        return userid;
    }

    public void setUserid(int userid) {
        this.userid = userid;
    }

    public String getIsp() {
        return isp;
    }

    public void setIsp(String isp) {
        this.isp = isp;
    }

    public String getNetworkspeed() {
        return networkspeed;
    }

    public void setNetworkspeed(String networkspeed) {
        this.networkspeed = networkspeed;
    }
}
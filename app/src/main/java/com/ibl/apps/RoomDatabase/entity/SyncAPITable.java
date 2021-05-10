package com.ibl.apps.RoomDatabase.entity;

import androidx.room.Entity;
import androidx.room.PrimaryKey;

@Entity
public class SyncAPITable {

    @PrimaryKey(autoGenerate = true)
    private int id;

    private int userid;

    private String api_name;

    private String endpoint_url;

    private String parameters;

    private String headers;

    private String status;

    private String description;

    private String created_time;

    private String chapterName;

    private String courseName;

    private String gradeName;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getUserid() {
        return userid;
    }

    public void setUserid(int userid) {
        this.userid = userid;
    }

    public String getApi_name() {
        return api_name;
    }

    public void setApi_name(String api_name) {
        this.api_name = api_name;
    }

    public String getEndpoint_url() {
        return endpoint_url;
    }

    public void setEndpoint_url(String endpoint_url) {
        this.endpoint_url = endpoint_url;
    }

    public String getParameters() {
        return parameters;
    }

    public void setParameters(String parameters) {
        this.parameters = parameters;
    }

    public String getHeaders() {
        return headers;
    }

    public void setHeaders(String headers) {
        this.headers = headers;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getCreated_time() {
        return created_time;
    }

    public void setCreated_time(String created_time) {
        this.created_time = created_time;
    }

    public String getChapterName() {
        return chapterName;
    }

    public void setChapterName(String chapterName) {
        this.chapterName = chapterName;
    }

    public String getCourseName() {
        return courseName;
    }

    public void setCourseName(String courseName) {
        this.courseName = courseName;
    }

    public String getGradeName() {
        return gradeName;
    }

    public void setGradeName(String gradeName) {
        this.gradeName = gradeName;
    }

    @Override
    public String toString() {
        return "SyncAPITable{" +
                "id=" + id +
                ", userid=" + userid +
                ", api_name='" + api_name + '\'' +
                ", endpoint_url='" + endpoint_url + '\'' +
                ", parameters='" + parameters + '\'' +
                ", headers='" + headers + '\'' +
                ", status='" + status + '\'' +
                ", description='" + description + '\'' +
                ", created_time='" + created_time + '\'' +
                ", chapterName='" + chapterName + '\'' +
                ", courseName='" + courseName + '\'' +
                ", gradeName='" + gradeName + '\'' +
                '}';
    }
}

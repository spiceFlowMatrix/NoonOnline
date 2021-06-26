package com.ibl.apps.QuizModule.models;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

public class ImageModel {

    @SerializedName("fileid")
    @Expose
    private long id;

    @SerializedName("url")
    @Expose
    private String url;

    public ImageModel() {
    }

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }
}

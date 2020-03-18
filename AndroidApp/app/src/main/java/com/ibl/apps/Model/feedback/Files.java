
package com.ibl.apps.Model.feedback;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class Files {

    @SerializedName("description")
    private Object mDescription;
    @SerializedName("duration")
    private Object mDuration;
    @SerializedName("filename")
    private String mFilename;
    @SerializedName("filesize")
    private Long mFilesize;
    @SerializedName("filetypeid")
    private Long mFiletypeid;
    @SerializedName("filetypename")
    private String mFiletypename;
    @SerializedName("id")
    private Long mId;
    @SerializedName("name")
    private String mName;
    @SerializedName("totalpages")
    private Long mTotalpages;
    @SerializedName("url")
    private String mUrl;

    public Object getDescription() {
        return mDescription;
    }

    public void setDescription(Object description) {
        mDescription = description;
    }

    public Object getDuration() {
        return mDuration;
    }

    public void setDuration(Object duration) {
        mDuration = duration;
    }

    public String getFilename() {
        return mFilename;
    }

    public void setFilename(String filename) {
        mFilename = filename;
    }

    public Long getFilesize() {
        return mFilesize;
    }

    public void setFilesize(Long filesize) {
        mFilesize = filesize;
    }

    public Long getFiletypeid() {
        return mFiletypeid;
    }

    public void setFiletypeid(Long filetypeid) {
        mFiletypeid = filetypeid;
    }

    public String getFiletypename() {
        return mFiletypename;
    }

    public void setFiletypename(String filetypename) {
        mFiletypename = filetypename;
    }

    public Long getId() {
        return mId;
    }

    public void setId(Long id) {
        mId = id;
    }

    public String getName() {
        return mName;
    }

    public void setName(String name) {
        mName = name;
    }

    public Long getTotalpages() {
        return mTotalpages;
    }

    public void setTotalpages(Long totalpages) {
        mTotalpages = totalpages;
    }

    public String getUrl() {
        return mUrl;
    }

    public void setUrl(String url) {
        mUrl = url;
    }

}

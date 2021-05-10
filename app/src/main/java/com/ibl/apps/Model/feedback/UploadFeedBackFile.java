package com.ibl.apps.Model.feedback;

import com.google.gson.annotations.SerializedName;

import java.util.List;

@SuppressWarnings("unused")
public class UploadFeedBackFile {

    @SerializedName("data")
    private Data mData;
    @SerializedName("message")
    private String mMessage;
    @SerializedName("response_code")
    private Long mResponseCode;
    @SerializedName("status")
    private String mStatus;

    public Data getData() {
        return mData;
    }

    public void setData(Data data) {
        mData = data;
    }

    public String getMessage() {
        return mMessage;
    }

    public void setMessage(String message) {
        mMessage = message;
    }

    public Long getResponseCode() {
        return mResponseCode;
    }

    public void setResponseCode(Long responseCode) {
        mResponseCode = responseCode;
    }

    public String getStatus() {
        return mStatus;
    }

    public void setStatus(String status) {
        mStatus = status;
    }

    public static class Data {

        @SerializedName("description")
        private Object mDescription;
        @SerializedName("feedbackId")
        private Long mFeedbackId;
        @SerializedName("feedbackTimeFiles")
        private List<FeedbackTimeFile> mFeedbackTimeFiles;
        @SerializedName("id")
        private Long mId;
        @SerializedName("time")
        private Object mTime;

        public Object getDescription() {
            return mDescription;
        }

        public void setDescription(Object description) {
            mDescription = description;
        }

        public Long getFeedbackId() {
            return mFeedbackId;
        }

        public void setFeedbackId(Long feedbackId) {
            mFeedbackId = feedbackId;
        }

        public List<FeedbackTimeFile> getFeedbackTimeFiles() {
            return mFeedbackTimeFiles;
        }

        public void setFeedbackTimeFiles(List<FeedbackTimeFile> feedbackTimeFiles) {
            mFeedbackTimeFiles = feedbackTimeFiles;
        }

        public Long getId() {
            return mId;
        }

        public void setId(Long id) {
            mId = id;
        }

        public Object getTime() {
            return mTime;
        }

        public void setTime(Object time) {
            mTime = time;
        }

    }

    public static class FeedbackTimeFile {

        @SerializedName("duration")
        private Object mDuration;
        @SerializedName("fileid")
        private Long mFileid;
        @SerializedName("url")
        private String mUrl;

        public Object getDuration() {
            return mDuration;
        }

        public void setDuration(Object duration) {
            mDuration = duration;
        }

        public Long getFileid() {
            return mFileid;
        }

        public void setFileid(Long fileid) {
            mFileid = fileid;
        }

        public String getUrl() {
            return mUrl;
        }

        public void setUrl(String url) {
            mUrl = url;
        }

    }
}
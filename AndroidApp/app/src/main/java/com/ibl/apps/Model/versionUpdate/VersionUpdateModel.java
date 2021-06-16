
package com.ibl.apps.Model.versionUpdate;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class VersionUpdateModel {

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

    @SuppressWarnings("unused")
    public static class Data {

        @SerializedName("isForceUpdate")
        private Boolean mIsForceUpdate;
        @SerializedName("version")
        private String mVersion;
        @SerializedName("versionCode")
        private String mVersionCode;

        public Boolean getIsForceUpdate() {
            return mIsForceUpdate;
        }

        public void setIsForceUpdate(Boolean isForceUpdate) {
            mIsForceUpdate = isForceUpdate;
        }

        public String getVersion() {
            return mVersion;
        }

        public void setVersion(String version) {
            mVersion = version;
        }

        public String getVersionCode() {
            return mVersionCode;
        }

        public void setVersionCode(String versionCode) {
            mVersionCode = versionCode;
        }

    }
}

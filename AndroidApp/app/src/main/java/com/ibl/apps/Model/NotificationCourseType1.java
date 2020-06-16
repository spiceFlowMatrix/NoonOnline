
package com.ibl.apps.Model;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class NotificationCourseType1 {

    @SerializedName("data")
    private Boolean mData;
    @SerializedName("message")
    private String mMessage;
    @SerializedName("response_code")
    private Long mResponseCode;
    @SerializedName("status")
    private String mStatus;

    public Boolean getData() {
        return mData;
    }

    public void setData(Boolean data) {
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

}

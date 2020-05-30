
package com.ibl.apps.Model.deviceManagement.requestQuotas;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class RequestQuotaModel {

    @SerializedName("data")
    private Long mData;
    @SerializedName("message")
    private String mMessage;
    @SerializedName("response_code")
    private Long mResponseCode;
    @SerializedName("status")
    private String mStatus;

    public Long getData() {
        return mData;
    }

    public void setData(Long data) {
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

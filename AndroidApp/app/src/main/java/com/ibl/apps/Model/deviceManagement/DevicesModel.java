
package com.ibl.apps.Model.deviceManagement;

import com.google.gson.annotations.SerializedName;

import java.util.List;

@SuppressWarnings("unused")
public class DevicesModel {

    @SerializedName("deviceToken")
    private String mDeviceToken;
    @SerializedName("id")
    private Long mId;
    @SerializedName("ipAddress")
    private String mIpAddress;
    @SerializedName("isActive")
    private Boolean mIsActive;
    @SerializedName("macAddress")
    private String mMacAddress;
    @SerializedName("modelName")
    private String mModelName;
    @SerializedName("modelNumber")
    private String mModelNumber;
    @SerializedName("operatingSystem")
    private OperatingSystem mOperatingSystem;
    @SerializedName("tags")
    private List<Tag> mTags;

    public String getDeviceToken() {
        return mDeviceToken;
    }

    public void setDeviceToken(String deviceToken) {
        mDeviceToken = deviceToken;
    }

    public Long getId() {
        return mId;
    }

    public void setId(Long id) {
        mId = id;
    }

    public String getIpAddress() {
        return mIpAddress;
    }

    public void setIpAddress(String ipAddress) {
        mIpAddress = ipAddress;
    }

    public Boolean getIsActive() {
        return mIsActive;
    }

    public void setIsActive(Boolean isActive) {
        mIsActive = isActive;
    }

    public String getMacAddress() {
        return mMacAddress;
    }

    public void setMacAddress(String macAddress) {
        mMacAddress = macAddress;
    }

    public String getModelName() {
        return mModelName;
    }

    public void setModelName(String modelName) {
        mModelName = modelName;
    }

    public String getModelNumber() {
        return mModelNumber;
    }

    public void setModelNumber(String modelNumber) {
        mModelNumber = modelNumber;
    }

    public OperatingSystem getOperatingSystem() {
        return mOperatingSystem;
    }

    public void setOperatingSystem(OperatingSystem operatingSystem) {
        mOperatingSystem = operatingSystem;
    }

    public List<Tag> getTags() {
        return mTags;
    }

    public void setTags(List<Tag> tags) {
        mTags = tags;
    }

}

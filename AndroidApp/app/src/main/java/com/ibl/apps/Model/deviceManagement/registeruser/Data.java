
package com.ibl.apps.Model.deviceManagement.registeruser;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class Data {

    @SerializedName("creationTime")
    private String mCreationTime;
    @SerializedName("creatorUserId")
    private Long mCreatorUserId;
    @SerializedName("deleterUserId")
    private String mDeleterUserId;
    @SerializedName("deletionTime")
    private String mDeletionTime;
    @SerializedName("deviceToken")
    private String mDeviceToken;
    @SerializedName("id")
    private Long mId;
    @SerializedName("ipAddress")
    private String mIpAddress;
    @SerializedName("isDeleted")
    private Boolean mIsDeleted;
    @SerializedName("lastModificationTime")
    private String mLastModificationTime;
    @SerializedName("lastModifierUserId")
    private String mLastModifierUserId;
    @SerializedName("macAddress")
    private String mMacAddress;
    @SerializedName("modelName")
    private String mModelName;
    @SerializedName("modelNumber")
    private String mModelNumber;
    @SerializedName("userId")
    private Long mUserId;

    public String getCreationTime() {
        return mCreationTime;
    }

    public void setCreationTime(String creationTime) {
        mCreationTime = creationTime;
    }

    public Long getCreatorUserId() {
        return mCreatorUserId;
    }

    public void setCreatorUserId(Long creatorUserId) {
        mCreatorUserId = creatorUserId;
    }

    public String getDeleterUserId() {
        return mDeleterUserId;
    }

    public void setDeleterUserId(String deleterUserId) {
        mDeleterUserId = deleterUserId;
    }

    public String getDeletionTime() {
        return mDeletionTime;
    }

    public void setDeletionTime(String deletionTime) {
        mDeletionTime = deletionTime;
    }

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

    public Boolean getIsDeleted() {
        return mIsDeleted;
    }

    public void setIsDeleted(Boolean isDeleted) {
        mIsDeleted = isDeleted;
    }

    public String getLastModificationTime() {
        return mLastModificationTime;
    }

    public void setLastModificationTime(String lastModificationTime) {
        mLastModificationTime = lastModificationTime;
    }

    public String getLastModifierUserId() {
        return mLastModifierUserId;
    }

    public void setLastModifierUserId(String lastModifierUserId) {
        mLastModifierUserId = lastModifierUserId;
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

    public Long getUserId() {
        return mUserId;
    }

    public void setUserId(Long userId) {
        mUserId = userId;
    }

}

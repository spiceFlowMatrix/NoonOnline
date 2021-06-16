
package com.ibl.apps.Model.deviceManagement;

import com.google.gson.annotations.SerializedName;

import java.util.List;


@SuppressWarnings("unused")
public class Data {

    @SerializedName("currentConsumption")
    private Long mCurrentConsumption;

    @SerializedName("deviceLimit")
    private Long mDeviceLimit;

    @SerializedName("isPendingRequest")
    private boolean isPendingRequest;

    @SerializedName("devicesModel")
    private List<DevicesModel> mDevicesModel;

    public Long getCurrentConsumption() {
        return mCurrentConsumption;
    }

    public void setCurrentConsumption(Long currentConsumption) {
        mCurrentConsumption = currentConsumption;
    }

    public Long getDeviceLimit() {
        return mDeviceLimit;
    }

    public void setDeviceLimit(Long deviceLimit) {
        mDeviceLimit = deviceLimit;
    }

    public List<DevicesModel> getDevicesModel() {
        return mDevicesModel;
    }

    public void setDevicesModel(List<DevicesModel> devicesModel) {
        mDevicesModel = devicesModel;
    }

    public boolean isPendingRequest() {
        return isPendingRequest;
    }

    public void setPendingRequest(boolean pendingRequest) {
        isPendingRequest = pendingRequest;
    }
}

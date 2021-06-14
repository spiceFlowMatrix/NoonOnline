
package com.ibl.apps.Model.deviceManagement;

import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class OperatingSystem {

    @SerializedName("deviceId")
    private Long mDeviceId;
    @SerializedName("name")
    private String mName;
    @SerializedName("version")
    private String mVersion;

    public Long getDeviceId() {
        return mDeviceId;
    }

    public void setDeviceId(Long deviceId) {
        mDeviceId = deviceId;
    }

    public String getName() {
        return mName;
    }

    public void setName(String name) {
        mName = name;
    }

    public String getVersion() {
        return mVersion;
    }

    public void setVersion(String version) {
        mVersion = version;
    }

}

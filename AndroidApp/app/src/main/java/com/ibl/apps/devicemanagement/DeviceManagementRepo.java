package com.ibl.apps.devicemanagement;

import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;


/**
 * This class contains API endpoints implementation of Device Managements.
 * This class is used by {@link DeviceManagementActivity}
 * */
public class DeviceManagementRepo implements DeviceManagementNetworking {
    private DeviceManagementNetworking deviceManagementNetworking;

    /**
     * This constructor initializes the retrofit client with application context for API calls.
     */
    public DeviceManagementRepo() {
        deviceManagementNetworking = ApiClient.getClient(NoonApplication.getContext()).create(DeviceManagementNetworking.class);
    }

    @Override
    public Single<UserDeviceModel> getAllDevices() {
        return deviceManagementNetworking.getAllDevices();
    }

    @Override
    public Single<UserDeviceProfileModel> getDeviceQuotaProfile(String userId) {
        return deviceManagementNetworking.getDeviceQuotaProfile(userId);
    }

    @Override
    public Single<UserDeviceModel> registerDeviceQuota(String userId) {
        return deviceManagementNetworking.registerDeviceQuota(userId);
    }

    @Override
    public Single<DeviceActivateDeActivate> updateDeviceQuota(String userId, String deviceId) {
        return deviceManagementNetworking.updateDeviceQuota(userId, deviceId);
    }

    @Override
    public Single<DeviceActivateDeActivate> deleteDeviceQuota(String userId, String deviceId) {
        return deviceManagementNetworking.deleteDeviceQuota(userId, deviceId);
    }
}

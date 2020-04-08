package com.ibl.apps.devicemanagement;

import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;


public class DeviceManagementRepository implements DeviceManagementNetworking {
    DeviceManagementNetworking deviceManagementNetworking;

    public DeviceManagementRepository() {
        deviceManagementNetworking = ApiClient.getClient(NoonApplication.getContext()).create(DeviceManagementNetworking.class);
    }

    @Override
    public Single<DeviceData> getAllDevice() {
        return deviceManagementNetworking.getAllDevice();
    }

    @Override
    public Single<DeviceProfile> getDeviceQuotaProfile(String userId) {
        return deviceManagementNetworking.getDeviceQuotaProfile(userId);
    }

    @Override
    public Single<DeviceData> registerDeviceQuota(String userId) {
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

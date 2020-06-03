package com.ibl.apps.DeviceManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.deviceManagement.DeviceListModel;
import com.ibl.apps.Model.deviceManagement.registeruser.DeviceRegisterModel;
import com.ibl.apps.Model.deviceManagement.requestQuotas.RequestQuotaModel;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;
import retrofit2.Response;

public
class DeviceManagementRepository implements DeviceManagementApiService {
    private DeviceManagementApiService deviceManagementApiService;

    public DeviceManagementRepository() {
        deviceManagementApiService = ApiClient.getClient(NoonApplication.getContext()).create(DeviceManagementApiService.class);
    }

    @Override
    public Single<DeviceListModel> fetchDeviceList() {
        return deviceManagementApiService.fetchDeviceList();
    }

    @Override
    public Single<Response<DeviceListModel>> chaneDeviceStatus(String DeviceId) {
        return deviceManagementApiService.chaneDeviceStatus(DeviceId);
    }

    @Override
    public Single<RequestQuotaModel> requestDeviceQuotas(String deviceId) {
        return deviceManagementApiService.requestDeviceQuotas(deviceId);
    }

    @Override
    public Single<Response<DeviceRegisterModel>> registerDeviceDetail(JsonObject body) {
        return deviceManagementApiService.registerDeviceDetail(body);
    }

}

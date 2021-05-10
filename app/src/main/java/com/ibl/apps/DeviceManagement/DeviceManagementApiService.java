package com.ibl.apps.DeviceManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.deviceManagement.DeviceListModel;
import com.ibl.apps.Model.deviceManagement.registeruser.DeviceRegisterModel;
import com.ibl.apps.Model.deviceManagement.requestQuotas.RequestQuotaModel;

import io.reactivex.Single;
import retrofit2.Response;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;

public interface DeviceManagementApiService {
    @GET("Device")
    Single<DeviceListModel> fetchDeviceList();

    @PUT("Device/ChaneDeviceStatus/{id}")
    Single<Response<DeviceListModel>> chaneDeviceStatus(@Path("id") String userId);

    @POST("DeviceQuotas/{id}")
    Single<RequestQuotaModel> requestDeviceQuotas(@Path("id") String userId);

    @POST("Device")
    Single<Response<DeviceRegisterModel>> registerDeviceDetail(@Body JsonObject body);
}

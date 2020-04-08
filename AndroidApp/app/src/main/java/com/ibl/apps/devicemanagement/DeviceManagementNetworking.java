package com.ibl.apps.devicemanagement;


import io.reactivex.Single;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;
import retrofit2.http.Query;

/**
* This Class defines API endpoints of Device Managements.
* It's contains all endpoints related to device networking.
* This interface is implemented in {@link DeviceManagementRepo}
 * By implementing this interface you can call these API endpoints.
* */
public interface DeviceManagementNetworking{
    @GET("device")
    Single<UserDeviceModel> getAllDevices();

    @GET("device/{userId}")
    Single<UserDeviceProfileModel> getDeviceQuotaProfile(@Path("userId") String userId);

    @POST("device/{userId}")
    Single<UserDeviceModel> registerDeviceQuota(@Path("userId") String userId);
    @PUT("device/{userId}")
    Single<DeviceActivateDeActivate> updateDeviceQuota(@Path("userId") String userId, @Query("deviceId") String deviceId);

    @DELETE("device/{userId}")
    Single<DeviceActivateDeActivate> deleteDeviceQuota(@Path("userId") String userId, @Query("deviceId") String deviceId);
}

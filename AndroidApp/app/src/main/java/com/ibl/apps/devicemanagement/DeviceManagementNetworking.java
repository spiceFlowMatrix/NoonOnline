package com.ibl.apps.devicemanagement;


import io.reactivex.Single;
import retrofit2.http.DELETE;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.PUT;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface DeviceManagementNetworking {
    @GET("device")
    Single<DeviceData> getAllDevice();

    @GET("device/{userId}")
    Single<DeviceProfile> getDeviceQuotaProfile(@Path("userId") String userId);

    @POST("device/{userId}")
    Single<DeviceData> registerDeviceQuota(@Path("userId") String userId);

    @PUT("device/{userId}")
    Single<DeviceActivateDeActivate> updateDeviceQuota(@Path("userId") String userId, @Query("deviceId") String deviceId);

    @DELETE("device/{userId}")
    Single<DeviceActivateDeActivate> deleteDeviceQuota(@Path("userId") String userId, @Query("deviceId") String deviceId);

}

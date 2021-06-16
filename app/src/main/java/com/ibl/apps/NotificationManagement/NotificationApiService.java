package com.ibl.apps.NotificationManagement;

import com.ibl.apps.Model.NotificationObject;

import io.reactivex.Single;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface NotificationApiService {
    // Notification List
    @GET("Notification/GetAllNotifications")
    Single<NotificationObject> fetchNotification(@Query("pagenumber") String pagenumber,
                                                 @Query("perpagerecord") String perpagerecord);
}

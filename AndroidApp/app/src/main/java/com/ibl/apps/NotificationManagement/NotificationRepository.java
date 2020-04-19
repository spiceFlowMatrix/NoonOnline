package com.ibl.apps.NotificationManagement;

import com.ibl.apps.Model.NotificationObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class NotificationRepository implements NotificationApiService {
    private NotificationApiService notificationApiService;

    public NotificationRepository() {
        notificationApiService = ApiClient.getClient(NoonApplication.getContext()).create(NotificationApiService.class);
    }

    @Override
    public Single<NotificationObject> fetchNotification(String pagenumber, String perpagerecord) {
        return notificationApiService.fetchNotification(pagenumber, perpagerecord);
    }
}

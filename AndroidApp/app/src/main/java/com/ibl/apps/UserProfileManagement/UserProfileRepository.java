package com.ibl.apps.UserProfileManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Observable;
import io.reactivex.Single;
import okhttp3.MultipartBody;
import retrofit2.Call;

public class UserProfileRepository implements UserProfileApiService {
    private UserProfileApiService userProfileApiService;

    public UserProfileRepository() {
        userProfileApiService = ApiClient.getClient(NoonApplication.getContext()).create(UserProfileApiService.class);
    }

    @Override
    public Single<UserObject> changePassword(JsonObject body) {
        return userProfileApiService.changePassword(body);
    }

    @Override
    public Single<UserObject> updateProfile(JsonObject body) {
        return userProfileApiService.updateProfile(body);
    }

    @Override
    public Observable<String> userExiest(String Username) {
        return userProfileApiService.userExiest(Username);
    }

    @Override
    public Call<UploadImageObject> uploadImage(MultipartBody.Part image) {
        return userProfileApiService.uploadImage(image);
    }

    @Override
    public Single<StatisticsObject> StatisticUser() {
        return userProfileApiService.StatisticUser();
    }

    @Override
    public Single<UserObject> fetchUser(String userId) {
        return userProfileApiService.fetchUser(userId);
    }
}

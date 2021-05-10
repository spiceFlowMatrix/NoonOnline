package com.ibl.apps.UserCredentialsManagement;

import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.ibl.apps.Model.CheckForgetKey;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.SyncRecords;
import com.ibl.apps.Model.SyncTimeTracking;
import com.ibl.apps.Model.TemsCondition;
import com.ibl.apps.Model.TrileSignupObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UserObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class UserRepository implements UserApiService {
    private UserApiService userApiService;

    public UserRepository() {
        userApiService = ApiClient.getClient(NoonApplication.getContext()).create(UserApiService.class);
    }

    @Override
    public Single<LoginObject> loginUser(JsonObject body) {
        return userApiService.loginUser(body);
    }

    @Override
    public Single<TrileSignupObject> SignupTrialUser(JsonObject body) {
        return userApiService.SignupTrialUser(body);
    }

    @Override
    public Single<UserObject> fetchUser(String userId) {
        return userApiService.fetchUser(userId);
    }

    @Override
    public Single<SyncRecords> GetSyncRecords() {
        return userApiService.GetSyncRecords();
    }

    @Override
    public Single<TemsCondition> getTerms() {
        return userApiService.getTerms();
    }

    @Override
    public Single<UploadImageObject> ForgotPasswordEmail(String email) {
        return userApiService.ForgotPasswordEmail(email);
    }

    @Override
    public Single<CheckForgetKey> CheckForgetKey(String passcode) {
        return userApiService.CheckForgetKey(passcode);
    }

    @Override
    public Single<UploadImageObject> UpdatePassword(JsonObject body) {
        return userApiService.UpdatePassword(body);
    }

    @Override
    public Single<SyncTimeTracking> getSyncTimeTracking(JsonArray body) {
        return userApiService.getSyncTimeTracking(body);
    }

    @Override
    public Single<UserObject> logout() {
        return userApiService.logout();
    }
}

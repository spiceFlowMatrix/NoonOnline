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

import io.reactivex.Single;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface UserApiService {
    // Login user
    @POST("Account")
    Single<LoginObject> loginUser(@Body JsonObject body);

    // Signup user
    @POST("Users/SignupTrialUser")
    Single<TrileSignupObject> SignupTrialUser(@Body JsonObject body);

    // UserData
    @GET("Users/{id}")
    Single<UserObject> fetchUser(@Path("id") String userId);

    // Get All App FillesData
    @GET("ProgessSync/GetSyncRecords")
    Single<SyncRecords> GetSyncRecords();

    @GET("Users/GetTerms")
    Single<TemsCondition> getTerms();

    // ForgotPassword
    @GET("Password/ForgotPassword/{email}")
    Single<UploadImageObject> ForgotPasswordEmail(@Path("email") String email);

    // ForgotPassword
    @GET("Password/CheckForgetKey/{passcode}")
    Single<CheckForgetKey> CheckForgetKey(@Path("passcode") String passcode);

    // Change Password
    @POST("Password/UpdatePassword")
    Single<UploadImageObject> UpdatePassword(@Body JsonObject body);

    @POST("ProgessSync/AppTimeTrack")
    Single<SyncTimeTracking> getSyncTimeTracking(@Body JsonArray body);

    // Logout User
    @GET("Account")
    Single<UserObject> logout();
}

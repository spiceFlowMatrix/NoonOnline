package com.ibl.apps.UserProfileManagement;

import com.google.gson.JsonObject;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.Model.UploadImageObject;
import com.ibl.apps.Model.UserObject;

import io.reactivex.Observable;
import io.reactivex.Single;
import okhttp3.MultipartBody;
import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.Multipart;
import retrofit2.http.PUT;
import retrofit2.http.Part;
import retrofit2.http.Path;
import retrofit2.http.Query;

public interface UserProfileApiService {
    // Change Password
    @PUT("Password")
    Single<UserObject> changePassword(@Body JsonObject body);

    // Update Profile
    @PUT("Profile")
    Single<UserObject> updateProfile(@Body JsonObject body);

    // Check Username is exiest or not
    @GET("Profile")
    Observable<String> userExiest(@Query("Username") String Username);

    // Upload Profile picture of specific login user
    @Multipart
    @PUT("Upload/UploadProfilePicture")
    Call<UploadImageObject> uploadImage(@Part MultipartBody.Part image);

    // Get Statistic
    @GET("Users/GetStatistic")
    Single<StatisticsObject> StatisticUser();

    // UserData
    @GET("Users/{id}")
    Single<UserObject> fetchUser(@Path("id") String userId);
}

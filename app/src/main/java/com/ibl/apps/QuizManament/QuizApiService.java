package com.ibl.apps.QuizManament;

import com.google.gson.JsonArray;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.RestResponse;

import io.reactivex.Single;
import okhttp3.ResponseBody;
import retrofit2.Call;

import retrofit2.http.Body;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Path;

public interface QuizApiService {

    @POST("QuizProgress/QuizProgressSync")
    Single<RestResponse> getQuizProgressSync(@Body JsonArray body);

    @POST("UserQuizResult/UserQuizResultSync")
    Single<RestResponse> getUserQuizResultSync(@Body JsonArray body);

    // QuizData
    @GET("Quiz/Quizpriview/{id}")
    Single<QuizMainObject> fetchQuizData(@Path("id") String Id);

    @GET("Quiz/CompleteQuizPreview/{id}")
    Call<ResponseBody> fetchAllQuestionData(@Path("id") String Id);
}

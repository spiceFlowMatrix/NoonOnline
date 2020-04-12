package com.ibl.apps.QuizManament;

import com.google.gson.JsonArray;
import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class QuizRepository implements QuizApiService {
    private QuizApiService quizApiService;

    public QuizRepository() {
        quizApiService = ApiClient.getClient(NoonApplication.getContext()).create(QuizApiService.class);
    }

    @Override
    public Single<RestResponse> getQuizProgressSync(JsonArray body) {
        return quizApiService.getQuizProgressSync(body);
    }

    @Override
    public Single<RestResponse> getUserQuizResultSync(JsonArray body) {
        return quizApiService.getUserQuizResultSync(body);
    }

    @Override
    public Single<QuizMainObject> fetchQuizData(String Id) {
        return quizApiService.fetchQuizData(Id);
    }
}

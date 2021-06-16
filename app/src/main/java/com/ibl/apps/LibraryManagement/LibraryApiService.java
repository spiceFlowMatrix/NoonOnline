package com.ibl.apps.LibraryManagement;

import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;

import io.reactivex.Single;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface LibraryApiService {
    // Book List
    @GET("Library/GetBooksApp")
    Single<LibraryObject> fetchBookList(@Query("search") String search);

    // Book List
    @GET("Library/GetBooksGradeWiseApp")
    Single<LibraryGradeObject> fetchBooksGradevise(@Query("search") String search);

}

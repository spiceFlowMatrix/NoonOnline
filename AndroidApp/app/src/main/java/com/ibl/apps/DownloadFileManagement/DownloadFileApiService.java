package com.ibl.apps.DownloadFileManagement;

import com.ibl.apps.Model.SignedUrlObject;

import io.reactivex.Single;
import retrofit2.http.GET;
import retrofit2.http.Query;

public interface DownloadFileApiService {
    // CoursePriviewGradeWise
    @GET("Files/GetSignedUrl")
    Single<SignedUrlObject> fetchSignedUrl(@Query("fileid") String fileid,
                                           @Query("lessionid") String lessionid);
}

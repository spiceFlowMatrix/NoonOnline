package com.ibl.apps.versionUpdateManagement;

import com.ibl.apps.Model.versionUpdate.VersionUpdateModel;

import io.reactivex.Single;
import retrofit2.http.GET;

public interface VersionUpdateApiService {
    @GET("AppVersion")
    Single<VersionUpdateModel> getVersionUpdate();
}

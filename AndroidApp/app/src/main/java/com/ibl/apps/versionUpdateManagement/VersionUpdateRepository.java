package com.ibl.apps.versionUpdateManagement;

import com.ibl.apps.Model.versionUpdate.VersionUpdateModel;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class VersionUpdateRepository implements VersionUpdateApiService {
    private VersionUpdateApiService versionUpdateApiService;

    public VersionUpdateRepository() {
        versionUpdateApiService = ApiClient.getClient(NoonApplication.getContext()).create(VersionUpdateApiService.class);
    }

    @Override
    public Single<VersionUpdateModel> getVersionUpdate() {
        return versionUpdateApiService.getVersionUpdate();
    }
}

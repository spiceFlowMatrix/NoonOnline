package com.ibl.apps.DownloadFileManagement;

import com.ibl.apps.Model.SignedUrlObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class DownloadFileRepository implements DownloadFileApiService {
    private DownloadFileApiService downloadFileApiService;

    public DownloadFileRepository() {
        downloadFileApiService = ApiClient.getClient(NoonApplication.getContext()).create(DownloadFileApiService.class);
    }

    @Override
    public Single<SignedUrlObject> fetchSignedUrl(String fileid, String lessionid) {
        return downloadFileApiService.fetchSignedUrl(fileid, lessionid);
    }
}

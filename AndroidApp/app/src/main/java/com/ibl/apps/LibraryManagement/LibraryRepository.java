package com.ibl.apps.LibraryManagement;

import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.noon.NoonApplication;

import io.reactivex.Single;

public class LibraryRepository implements LibraryApiService {
    private LibraryApiService libraryApiService;

    public LibraryRepository() {
        libraryApiService = ApiClient.getClient(NoonApplication.getContext()).create(LibraryApiService.class);
    }

    @Override
    public Single<LibraryObject> fetchBookList(String search) {
        return libraryApiService.fetchBookList(search);
    }

    @Override
    public Single<LibraryGradeObject> fetchBooksGradevise(String search) {
        return libraryApiService.fetchBooksGradevise(search);
    }
}

package com.ibl.apps.RoomDatabase.dao.userManagementDatabse;

import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.Model.LoginObject;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.NoonApplication;

import java.util.List;

public class UserDatabaseRepository {
    private AppDatabase database;

    public UserDatabaseRepository() {
        database = AppDatabase.getAppDatabase(NoonApplication.getContext());
    }

    public void insertAuthTokenData(AuthTokenObject... authTokenObjects) {
        database.authTokenDao().insertAll(authTokenObjects);
    }

    public AuthTokenObject getAuthTokenData(String sub) {
        return database.authTokenDao().getauthTokenData(sub);
    }


    public void updateAuthToken(String sub, String accessToken, String idToken, Long expiresIn, String scope, String expiresAt, String exp) {
        database.authTokenDao().updateToken(sub, accessToken, idToken, expiresIn, scope, expiresAt, exp);
    }

    public void insertLoginData(LoginObject... loginObjects) {
        database.loginDao().insertAll(loginObjects);
    }

    public LoginObject getLoginDetail(String email, String password) {
        return database.loginDao().getLogin(email, password);
    }

    public LoginObject getLoginEmailDetail(String email) {
        return database.loginDao().getLoginEmail(email);
    }

    public LoginObject getLoginPasswordDetail(String password) {
        return database.loginDao().getLoginPassword(password);
    }

    public LoginObject getUserLoginModel() {
        return database.loginDao().getLoginModel();
    }

    public LoginObject getUserLoginModelById(String userId) {
        return database.loginDao().getLoginModelById(userId);
    }

    public void updateLoginModel(LoginObject... loginObject) {
        database.loginDao().updateModel(loginObject);
    }

    public void insertUserDetails(UserDetails... userObjects) {
        database.userDetailDao().insertAll(userObjects);
    }

    public UserDetails getUserDetails(String sub) {
        return database.userDetailDao().getUserDetials(sub);
    }

    public List<UserDetails> getAllUserDetails() {
        return database.userDetailDao().getAllUserDetials();
    }

    public void updateUserDetails(String id, String username, String fullName, String phonenumber) {
        database.userDetailDao().updateUserDetails(id, username, fullName, phonenumber);
    }

    public void updateUserPhoto(String id, String profilepicurl, byte[] profilepicImage) {
        database.userDetailDao().updateUserPhoto(id, profilepicurl, profilepicImage);
    }

    public void deleteByUserId(String sub) {
        database.userDetailDao().deleteByUserId(sub);
    }

    public void insertStatisticData(StatisticsObject... statisticsObjects) {
        database.statisticsDao().insertAll(statisticsObjects);
    }

    public StatisticsObject getStatisticsObjectData(String userId) {
        return database.statisticsDao().getStatisticsObject(userId);
    }
}

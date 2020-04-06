package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.Model.AuthTokenObject;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface AuthTokenDao {

    @Insert
    void insertAll(AuthTokenObject... authTokenObjects);

    @Query("SELECT * FROM AuthTokenObject WHERE sub =:sub")
    AuthTokenObject getauthTokenData(String sub);

    @Query("UPDATE AuthTokenObject SET sub = :sub, accessToken = :accessToken, idToken = :idToken , expiresIn = :expiresIn,scope = :scope, expiresAt = :expiresAt, exp = :exp WHERE sub =:sub")
    void updateToken(String sub, String accessToken, String idToken, Long expiresIn, String scope, String expiresAt, String exp);
}

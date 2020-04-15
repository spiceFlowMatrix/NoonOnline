package com.ibl.apps.RoomDatabase.dao;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.Model.LoginObject;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface LoginDao {

    @Insert
    void insertAll(LoginObject... loginObjects);

    @Query("SELECT * FROM LoginObject WHERE email = :email AND password = :password")
    LoginObject getLogin(String email, String password);

    @Query("SELECT * FROM LoginObject WHERE email =:email")
    LoginObject getLoginEmail(String email);

    @Query("SELECT * FROM LoginObject WHERE password =:password ")
    LoginObject getLoginPassword(String password);

    @Query("SELECT * FROM LoginObject ")
    LoginObject getLoginModel();

    @Query("SELECT * FROM LoginObject WHERE userid =:userId  ")
    LoginObject getLoginModelById(String userId);

    @Update
    void updateModel(LoginObject...loginObject);
}

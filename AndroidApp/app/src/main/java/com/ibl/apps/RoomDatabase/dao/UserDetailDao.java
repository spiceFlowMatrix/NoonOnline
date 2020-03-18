package com.ibl.apps.RoomDatabase.dao;

import android.arch.persistence.room.Dao;
import android.arch.persistence.room.Insert;
import android.arch.persistence.room.Query;

import com.ibl.apps.RoomDatabase.entity.UserDetails;

import java.util.List;


/**
 * Created by lukegjpotter on 25/11/2017.
 */
@Dao
public interface UserDetailDao {

    @Insert
    void insertAll(UserDetails... userObjects);

    @Query("SELECT * FROM UserDetails WHERE sub = :sub")
    UserDetails getUserDetials(String sub);

    @Query("SELECT * FROM UserDetails")
    List<UserDetails> getAllUserDetials();

    @Query("UPDATE UserDetails SET phonenumber = :phonenumber,username = :username,fullName = :fullName  WHERE id = :id")
    void updateUserDetails(String id, String username, String fullName, String phonenumber);

    @Query("UPDATE UserDetails SET profilepicurl = :profilepicurl, profilepicImage = :profilepicImage WHERE id = :id")
    void updateUserPhoto(String id, String profilepicurl, byte[] profilepicImage);

    @Query("DELETE FROM UserDetails WHERE sub = :sub")
    void deleteByUserId(String sub);
}

package com.ibl.apps.RoomDatabase.dao.deviceManagementDatabase;

import androidx.room.Dao;
import androidx.room.Insert;
import androidx.room.Query;

import com.ibl.apps.DeviceManagement.UserDeviceModel;

/**
 * This interface contains database queries implementation of Device Managements.
 * This class is used by {@link DeviceManagementRepository}
 * */
@Dao
public interface DeviceManagementDataDao {
    @Insert
    void insertAll(UserDeviceModel... userDeviceData);

    @Query("SELECT * FROM UserDeviceModel WHERE userId=:userid")
    UserDeviceModel getDeviceData(String userid);

    @Query("SELECT * FROM UserDeviceModel ")
    UserDeviceModel getDeviceData();

}

package com.ibl.apps.RoomDatabase.dao.deviceManagementDatabase;

import androidx.room.Dao;
import androidx.room.Query;
import androidx.room.Update;

import com.ibl.apps.DeviceManagement.UserDeviceProfileModel;
/**
 * This interface contains database queries implementation of Device Managements.
 * This class is used by {@link DeviceManagementRepository}
 * */
@Dao
public interface DeviceManagementProfileDao {

    @Query("SELECT * FROM UserDeviceProfileModel WHERE userId=:userid")
    UserDeviceProfileModel getDeviceProfile(String userid);

    @Query("DELETE  FROM UserDeviceProfileModel WHERE userId=:userid AND deviceId=:deviceId")
    void deleteDevice(String userid, String deviceId);

    @Update
    void updateDevice(UserDeviceProfileModel... userDeviceProfileModels);
}

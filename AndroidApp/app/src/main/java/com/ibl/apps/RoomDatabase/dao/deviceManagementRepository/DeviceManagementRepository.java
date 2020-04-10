package com.ibl.apps.RoomDatabase.dao.deviceManagementRepository;

import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.DeviceManagement.DeviceManagementActivity;
import com.ibl.apps.DeviceManagement.UserDeviceModel;
import com.ibl.apps.DeviceManagement.UserDeviceProfileModel;
import com.ibl.apps.noon.NoonApplication;

/**
 * This class contains local database implementation of Device Managements.
 * This class is used by {@link DeviceManagementActivity}
 * */
public class DeviceManagementRepository implements DeviceManagementDataDao, DeviceManagementProfileDao {
    private DeviceManagementDataDao deviceManagementDataDao;
    private DeviceManagementProfileDao deviceManagementProfileDao;

    public DeviceManagementRepository() {
        deviceManagementDataDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).deviceManagementDataDao();
        deviceManagementProfileDao = AppDatabase.getAppDatabase(NoonApplication.getContext()).deviceManagementProfileDao();
    }


    @Override
    public void insertAll(UserDeviceModel... userDeviceData) {
        deviceManagementDataDao.insertAll(userDeviceData);
    }

    @Override
    public UserDeviceModel getDeviceData(String userid) {
        return deviceManagementDataDao.getDeviceData(userid);
    }

    @Override
    public UserDeviceModel getDeviceData() {
        return deviceManagementDataDao.getDeviceData();
    }

    @Override
    public UserDeviceProfileModel getDeviceProfile(String userid) {
        return deviceManagementProfileDao.getDeviceProfile(userid);
    }

    @Override
    public void deleteDevice(String userid, String deviceId) {
        deviceManagementProfileDao.deleteDevice(userid, deviceId);
    }

    @Override
    public void updateDevice(UserDeviceProfileModel... userDeviceProfileModels) {
        deviceManagementProfileDao.updateDevice(userDeviceProfileModels);
    }
}

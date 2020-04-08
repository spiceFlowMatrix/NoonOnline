package com.ibl.apps.devicemanagement;

import android.content.Intent;
import android.os.Bundle;

import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.RoomDatabase.dao.deviceManagementRepository.DeviceManagementRepository;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;


public class DeviceManagementActivity extends BaseActivity {
    DeviceManagementRepo deviceManagementRepo;
    CompositeDisposable disposable = new CompositeDisposable();
    DeviceManagementRepository deviceManagementRepository;
    private String userId,deviceId;

    @Override
    protected int getContentView() {
        return 0;
    }


    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        deviceManagementRepo = new DeviceManagementRepo();
        deviceManagementRepository = new DeviceManagementRepository();

        //Fetch list from all device
        disposable.add(deviceManagementRepo.getAllDevices()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UserDeviceModel>() {
                    @Override
                    public void onSuccess(UserDeviceModel userDeviceModel) {
                        //Here is getting successful response of user device information

                        //insert user device information in local database from API response
                        deviceManagementRepository.insertAll(userDeviceModel);
                    }

                    @Override
                    public void onError(Throwable e) {
                        //failures can possibly is network error and server error
                        //other failures can perform 401 Not authorized
                        hideDialog();
                        showError(e);
                    }
                }));

        //Get device profile
        disposable.add(deviceManagementRepo.getDeviceQuotaProfile(userId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UserDeviceProfileModel>() {
                    @Override
                    public void onSuccess(UserDeviceProfileModel userDeviceProfileModel) {
                        //Here is getting successful response of user device profile

                        //Get all information from API response & store in local database
                        deviceManagementRepository.getDeviceData();

                        //Here we can update user device information in local database
                        deviceManagementRepository.updateDevice(userDeviceProfileModel);
                    }

                    @Override
                    public void onError(Throwable e) {
                        //failures can possibly is network error and server error
                        //other failures can perform 405 Invalid input
                        hideDialog();
                        showError(e);
                    }
                }));

        //Register device quota for user
        disposable.add(deviceManagementRepo.registerDeviceQuota(userId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<UserDeviceModel>() {
                    @Override
                    public void onSuccess(UserDeviceModel userDeviceModel) {
                        //Here is getting successful response of user register device quota

                        //Get all information from API response & store in local database using userID
                        deviceManagementRepository.getDeviceData(userId);
                    }

                    @Override
                    public void onError(Throwable e) {
                        //failures can possibly is network error and server error
                        //other failures can perform 405 Invalid input
                        hideDialog();
                        showError(e);
                    }
                }));

        //Reactivate an existing device
        disposable.add(deviceManagementRepo.updateDeviceQuota(userId, deviceId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceActivateDeActivate>() {
                    @Override
                    public void onSuccess(DeviceActivateDeActivate deviceData) {
                        //Here is getting successful response of user device status like reactivate an existing device
                    }

                    @Override
                    public void onError(Throwable e) {
                        //failures can possibly is network error and server error
                        //other failures can perform by :
                        //1.400 Invalid ID supplied
                        //2.404 Device not found
                        //3.405 Validation exception
                        hideDialog();
                        showError(e);
                    }
                }));

        //Deactivate an existing device
        disposable.add(deviceManagementRepo.deleteDeviceQuota(userId, deviceId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceActivateDeActivate>() {
                    @Override
                    public void onSuccess(DeviceActivateDeActivate deviceData) {
                        //Here is getting successful response of user device status like Deactivate an existing device

                        //delete user device data from local database
                        deviceManagementRepository.deleteDevice(userId, deviceId);
                    }

                    @Override
                    public void onError(Throwable e) {
                        //failures can possibly is network error and server error
                        //other failures can perform by :
                        //1.400 Invalid ID supplied
                        //2.404 Device not found
                        //3.405 Validation exception
                        hideDialog();
                        showError(e);
                    }
                }));
    }

}

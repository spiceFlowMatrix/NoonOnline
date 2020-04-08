package com.ibl.apps.devicemanagement;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.os.Bundle;

import com.ibl.apps.Base.BaseActivity;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

@SuppressLint("Registered")
public class DeviceManagementActivity extends BaseActivity {
    DeviceManagementRepository deviceManagementRepository;
    CompositeDisposable disposable = new CompositeDisposable();
    private String userId;
    private String deviceId;

    @Override
    protected int getContentView() {
        return 0;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        deviceManagementRepository = new DeviceManagementRepository();

        //Fetch list of all device
        disposable.add(deviceManagementRepository.getAllDevice()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceData>() {
                    @Override
                    public void onSuccess(DeviceData deviceData) {

                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));

        //Get device profile
        disposable.add(deviceManagementRepository.getDeviceQuotaProfile(userId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceProfile>() {
                    @Override
                    public void onSuccess(DeviceProfile deviceData) {

                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));

        //Register device quota for user
        disposable.add(deviceManagementRepository.registerDeviceQuota(userId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceData>() {
                    @Override
                    public void onSuccess(DeviceData deviceData) {

                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));

        //Reactivate an existing device
        disposable.add(deviceManagementRepository.updateDeviceQuota(userId, deviceId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceActivateDeActivate>() {
                    @Override
                    public void onSuccess(DeviceActivateDeActivate deviceData) {

                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));

        //Deactivate an existing device
        disposable.add(deviceManagementRepository.deleteDeviceQuota(userId, deviceId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DeviceActivateDeActivate>() {
                    @Override
                    public void onSuccess(DeviceActivateDeActivate deviceData) {

                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));
    }
}

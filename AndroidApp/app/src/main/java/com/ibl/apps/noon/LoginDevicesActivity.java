package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;

import com.ibl.apps.Adapter.AllDeviceListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.noon.databinding.ActivityLoginDevicesBinding;

public class LoginDevicesActivity extends BaseActivity {
    ActivityLoginDevicesBinding binding;
    AllDeviceListAdapter allDeviceListAdapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_login_devices;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityLoginDevicesBinding) getBindObj();
        apiService = ApiClient.getClient(getApplicationContext()).create(ApiService.class);
        setToolbar(binding.toolBar);
        showBackArrow("Logins");
        allDeviceListAdapter = new AllDeviceListAdapter(this);
        binding.rcVerticalLayout.rcVertical.setAdapter(allDeviceListAdapter);
    }
}

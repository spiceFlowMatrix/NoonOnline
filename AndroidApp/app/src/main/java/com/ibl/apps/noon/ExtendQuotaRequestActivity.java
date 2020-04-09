package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;

import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.noon.databinding.ActivityExtendQuotaRequestBinding;

public class ExtendQuotaRequestActivity extends BaseActivity {
    ActivityExtendQuotaRequestBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_extend_quota_request;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityExtendQuotaRequestBinding) getBindObj();
        apiService = ApiClient.getClient(getApplicationContext()).create(ApiService.class);
    }
}

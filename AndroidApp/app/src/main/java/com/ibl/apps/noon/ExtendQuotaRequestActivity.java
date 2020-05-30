package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DeviceManagement.DeviceManagementRepository;
import com.ibl.apps.Model.deviceManagement.requestQuotas.RequestQuotaModel;
import com.ibl.apps.noon.databinding.ActivityExtendQuotaRequestBinding;
import com.ibl.apps.util.Validator;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

public class ExtendQuotaRequestActivity extends BaseActivity implements View.OnClickListener {
    ActivityExtendQuotaRequestBinding binding;
    private String deviceId;
    private CompositeDisposable disposable = new CompositeDisposable();
    private DeviceManagementRepository deviceManagementRepository;

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

        if (getIntent() != null) {
            deviceId = getIntent().getStringExtra("deviceId");
        }
        deviceManagementRepository = new DeviceManagementRepository();
        setToolbar(binding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.extend_quote_request));

        binding.toolbarLayout.toolBar.setNavigationOnClickListener(view -> {
            finish();
        });

        setonClickListener();
    }

    private void setonClickListener() {
        binding.txtSendClick.setOnClickListener(this);
        binding.hourArrowUp.setOnClickListener(this);
        binding.hourArrowDown.setOnClickListener(this);
    }

    public boolean validateFields() {
        if (binding.tvQuotaLimit.getText().toString().trim().equals("0")) {
            hideKeyBoard(binding.tvQuotaLimit);
            binding.viewHour.setBackgroundColor(getResources().getColor(R.color.colorRed));
            return false;
        } else {
            binding.viewHour.setBackgroundColor(getResources().getColor(R.color.colorPrimary));
            hideKeyBoard(binding.tvQuotaLimit);
        }

        if (!Validator.checkEmpty(binding.descrption)) {
            hideKeyBoard(binding.descrption);
            binding.descrptionWrapper.setError(getString(R.string.validation_enterDecription));
            return false;
        } else {
            hideKeyBoard(binding.descrption);
            binding.descrptionWrapper.setErrorEnabled(false);
        }

        return true;
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.txtSendClick:
                if (validateFields()) {
                    String description = binding.descrption.getText().toString().trim();
                    String quotaLimit = binding.tvQuotaLimit.getText().toString().trim();

                    if (isNetworkAvailable(ExtendQuotaRequestActivity.this)) {
                        callApiExtendQuotaRequest(description, quotaLimit);
                    } else {
                        showNetworkAlert(ExtendQuotaRequestActivity.this);
                    }
                }
                break;

            case R.id.hour_arrow_up:
                int current_quotaLimit_add = Integer.parseInt(binding.tvQuotaLimit.getText().toString());
                if (current_quotaLimit_add < 100) {
                    current_quotaLimit_add++;
                }
                binding.tvQuotaLimit.setText(String.valueOf(current_quotaLimit_add));
                /*if (current_min_add <= 9) {
                    binding.tvQuotaLimit.setText(String.valueOf(0).concat(String.valueOf(current_min_add)));
                } else {
                    binding.tvQuotaLimit.setText(String.valueOf(current_min_add));
                }*/
                break;

            case R.id.hour_arrow_down:
                int current_quotaLimit_sub = Integer.parseInt(binding.tvQuotaLimit.getText().toString());
                if (current_quotaLimit_sub > 0) {
                    current_quotaLimit_sub--;
                }

                binding.tvQuotaLimit.setText(String.valueOf(current_quotaLimit_sub));
              /*  if (currentsub_min_sub <= 9) {
                    binding.tvQuotaLimit.setText(String.valueOf(0).concat(String.valueOf(currentsub_min_sub)));
                } else {
                    binding.tvQuotaLimit.setText(String.valueOf(currentsub_min_sub));
                }*/
                break;
        }
    }

    private void callApiExtendQuotaRequest(String description, String quotaLimit) {
        try {
            showDialog(NoonApplication.getContext().getResources().getString(R.string.loading));
            disposable.add(deviceManagementRepository.requestDeviceQuotas(deviceId)
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<RequestQuotaModel>() {
                        @Override
                        public void onSuccess(RequestQuotaModel deviceListModel) {
                            if (deviceListModel != null)
                                if (deviceListModel.getResponseCode() == 0) {
                                    finish();
                                }
                            hideDialog();
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}

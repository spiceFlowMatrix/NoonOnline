package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.noon.databinding.ActivityExtendQuotaRequestBinding;
import com.ibl.apps.util.Validator;

import java.util.Objects;

public class ExtendQuotaRequestActivity extends BaseActivity implements View.OnClickListener {
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
                    String description = Objects.requireNonNull(binding.descrption.getText()).toString().trim();
                    String hour = binding.tvQuotaLimit.getText().toString().trim();
                }
                break;

            case R.id.hour_arrow_up:
                int current_min_add = Integer.parseInt(binding.tvQuotaLimit.getText().toString());
                if (current_min_add < 100) {
                    current_min_add++;
                }
                binding.tvQuotaLimit.setText(String.valueOf(current_min_add));
                /*if (current_min_add <= 9) {
                    binding.tvQuotaLimit.setText(String.valueOf(0).concat(String.valueOf(current_min_add)));
                } else {
                    binding.tvQuotaLimit.setText(String.valueOf(current_min_add));
                }*/
                break;

            case R.id.hour_arrow_down:
                int currentsub_min_sub = Integer.parseInt(binding.tvQuotaLimit.getText().toString());
                if (currentsub_min_sub > 0) {
                    currentsub_min_sub--;
                }

                binding.tvQuotaLimit.setText(String.valueOf(currentsub_min_sub));
              /*  if (currentsub_min_sub <= 9) {
                    binding.tvQuotaLimit.setText(String.valueOf(0).concat(String.valueOf(currentsub_min_sub)));
                } else {
                    binding.tvQuotaLimit.setText(String.valueOf(currentsub_min_sub));
                }*/
                break;
        }
    }
}

package com.ibl.apps.noon;

import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.text.SpannableStringBuilder;
import android.util.Log;
import android.util.TypedValue;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.LinearLayout;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.appcompat.app.AlertDialog;
import androidx.databinding.DataBindingUtil;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.google.gson.Gson;
import com.ibl.apps.Adapter.AllDeviceListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DeviceManagement.DeviceManagementRepository;
import com.ibl.apps.Model.ProgressItem;
import com.ibl.apps.Model.deviceManagement.DeviceListModel;
import com.ibl.apps.Model.deviceManagement.registeruser.DeviceRegisterModel;
import com.ibl.apps.noon.databinding.ActivateDeavtivateBottomSheetBinding;
import com.ibl.apps.noon.databinding.ActivityLoginDevicesBinding;

import java.io.IOException;
import java.util.ArrayList;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.Response;

import static com.ibl.apps.util.Const.deviceStatus;

public class LoginDevicesActivity extends BaseActivity implements View.OnClickListener, AllDeviceListAdapter.ShowBottomInterface {
    ActivityLoginDevicesBinding binding;
    private AllDeviceListAdapter allDeviceListAdapter;
    private ArrayList<ProgressItem> progressItemList = new ArrayList<>();
    private ProgressItem mProgressItem;
    private CompositeDisposable disposable = new CompositeDisposable();
    private DeviceManagementRepository deviceManagementRepository;
    private String deviceId;
    private BottomSheetDialog mBottomSheetDialog;
    private AllDeviceListAdapter.ShowBottomInterface showBottomInterface;

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
        showBottomInterface = this;
        deviceManagementRepository = new DeviceManagementRepository();

        setToolbar(binding.toolBar);
        showBackArrow(getString(R.string.logins));

//        int aTrueProgress = (int) ((aTrueCount) * 100 / quizQuestionsObjectList.size());

        callApiDeviceList();
        //getSeekbarProgress();

        setOnClickListners();
        binding.toolBar.setNavigationOnClickListener(view -> {
            onBackPressed();
        });
    }

    private void callApiDeviceList() {
        try {
            showDialog(NoonApplication.getContext().getResources().getString(R.string.loading));
            disposable.add(deviceManagementRepository.fetchDeviceList()
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<DeviceListModel>() {

                        @Override
                        public void onSuccess(DeviceListModel deviceListModel) {
                            if (deviceListModel != null && deviceListModel.getData() != null) {
                                if (deviceListModel.getData().getDevicesModel() != null)
                                    for (int i = 0; i < deviceListModel.getData().getDevicesModel().size(); i++) {
                                        deviceId = String.valueOf(deviceListModel.getData().getDevicesModel().get(i).getId());
                                    }
                                if (!deviceListModel.getData().isPendingRequest()) {
                                    binding.txtExtendQuota.setVisibility(View.VISIBLE);
                                    binding.txtAlreadyPendingQuota.setVisibility(View.GONE);
                                } else {
                                    binding.txtAlreadyPendingQuota.setVisibility(View.VISIBLE);
                                    binding.txtExtendQuota.setVisibility(View.GONE);
                                }
                                if (deviceListModel.getData().getDeviceLimit() == 0)
                                    return;

                                progressQuota(deviceListModel.getData().getCurrentConsumption(), deviceListModel.getData().getDeviceLimit());

                                allDeviceListAdapter = new AllDeviceListAdapter(LoginDevicesActivity.this, deviceListModel.getData().getDevicesModel(), showBottomInterface);
                                binding.rcVerticalLayout.rcVertical.setAdapter(allDeviceListAdapter);
                            }

                            hideDialog();
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();
                        }
                    }));

        } catch (
                Exception e) {
            e.printStackTrace();
        }

    }

    private void progressQuota(Long currentConsumption, Long deviceLimit) {
        int weight = (int) ((float) currentConsumption / deviceLimit * 100);
        LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WRAP_CONTENT, weight);
        binding.targetProgressTextLay.setLayoutParams(params);
        LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WRAP_CONTENT, 100 - weight);
        binding.trueProgressTextLay.setLayoutParams(params1);

        binding.tvTotalQuota.setText(getString(R.string.device_total_quota) + "\n" + deviceLimit);

        if (currentConsumption != 0) {
            binding.tvCurrentQuotaCount.setText(currentConsumption + "\n" + getString(R.string.device_current_quota));
            binding.tvCurrentQuotaCount.setVisibility(View.VISIBLE);
            binding.tvCurrentQuota0.setVisibility(View.GONE);
            binding.viewSky0.setVisibility(View.GONE);
            binding.viewSky.setVisibility(View.VISIBLE);
        } else {
            binding.tvCurrentQuota0.setText("0\n" + getString(R.string.device_current_quota));
            binding.viewSky0.setVisibility(View.VISIBLE);
            binding.viewSky.setVisibility(View.GONE);
            binding.tvCurrentQuotaCount.setVisibility(View.GONE);
            binding.tvCurrentQuota0.setVisibility(View.VISIBLE);
        }
    }

    private int convertDipToPixels(float dip) {
        float px = TypedValue.applyDimension(TypedValue.COMPLEX_UNIT_DIP, dip, getResources().getDisplayMetrics());
        return (int) px;
    }

    /*private void getSeekbarProgress() {
        int aTrueProgress = (int) ((60) * 100 / 60);
        int intpassingMarks = Integer.parseInt("60");
        int TargetProgress = intpassingMarks - aTrueProgress;
        int LeftProgress = 100 - intpassingMarks;

       *//* if (aTrueProgress > intpassingMarks) {
            LeftProgress = LeftProgress + (TargetProgress);
            TargetProgress = 0;
        }*//*

        //Log.e(Const.LOG_NOON_TAG, "===passingMarks===" + passingMarks);
        //Log.e(Const.LOG_NOON_TAG, "===aTrueProgress===" + aTrueProgress);
        //Log.e(Const.LOG_NOON_TAG, "===TargetProgress===" + TargetProgress);
        //Log.e(Const.LOG_NOON_TAG, "===LeftProgress===" + LeftProgress);

        //Total Quota
        ViewTreeObserver vto = binding.trueProgressTextLay.getViewTreeObserver();
        vto.addOnGlobalLayoutListener(new ViewTreeObserver.OnGlobalLayoutListener() {
            @Override
            public void onGlobalLayout() {
                if (Build.VERSION.SDK_INT < Build.VERSION_CODES.JELLY_BEAN) {
                    binding.trueProgressTextLay.getViewTreeObserver().removeGlobalOnLayoutListener(this);
                } else {
                    binding.trueProgressTextLay.getViewTreeObserver().removeOnGlobalLayoutListener(this);
                }
                int width = binding.trueProgressTextLay.getMeasuredWidth();
                int height = binding.trueProgressTextLay.getMeasuredHeight();

                int leftMarginFromInt = ((width * aTrueProgress) / 100);
                if (leftMarginFromInt != 0) {

                    LinearLayout.LayoutParams paramsview = new LinearLayout.LayoutParams(5, 30);
                    paramsview.setMargins(leftMarginFromInt - 3, 0, 0, 0);
                    View view = new View(LoginDevicesActivity.this);
                    view.setBackgroundColor(getResources().getColor(R.color.colorRed));
                    binding.trueProgressTextLay.addView(view, paramsview);

                    int margSpace = 100;

                    LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                    params.setMargins(leftMarginFromInt - margSpace, 0, 0, 0);
                    TextView aTrueText = new TextView(LoginDevicesActivity.this);
                    aTrueText.setText(4 + "\n" + "Total Quota");
                    aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                    aTrueText.setTextSize(12);
                    aTrueText.setGravity(Gravity.CENTER);
                    binding.trueProgressTextLay.addView(aTrueText, params);
                }
            }
        });

        //Current Quota
        ViewTreeObserver vto1 = binding.targetProgressTextLay.getViewTreeObserver();
        vto1.addOnGlobalLayoutListener(new ViewTreeObserver.OnGlobalLayoutListener() {
            @Override
            public void onGlobalLayout() {
                if (Build.VERSION.SDK_INT < Build.VERSION_CODES.JELLY_BEAN) {
                    binding.targetProgressTextLay.getViewTreeObserver().removeGlobalOnLayoutListener(this);
                } else {
                    binding.targetProgressTextLay.getViewTreeObserver().removeOnGlobalLayoutListener(this);
                }
                int width = binding.targetProgressTextLay.getMeasuredWidth();
                int height = binding.targetProgressTextLay.getMeasuredHeight();

                int targetMarginFromInt = ((width * intpassingMarks) / 100);
                if (targetMarginFromInt != 0) {

                    LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                    params.setMargins(targetMarginFromInt - 95, 0, 0, 0);
                    TextView aTrueText = new TextView(LoginDevicesActivity.this);
//                    aTrueText.setText("Total Quota" + "\n" + intpassingMarks + "%");
                    aTrueText.setText("Current Quota" + "\n" + 6);
                    aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                    aTrueText.setTextSize(12);
                    aTrueText.setGravity(Gravity.CENTER);
                    binding.targetProgressTextLay.addView(aTrueText, params);

                    LinearLayout.LayoutParams paramsview = new LinearLayout.LayoutParams(5, 30);
                    paramsview.setMargins(targetMarginFromInt - 4, 0, 0, 0);
                    View view = new View(LoginDevicesActivity.this);

                  *//*  if (aTrueProgress > intpassingMarks) {
                        view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                    } else {
                        view.setBackgroundColor(getResources().getColor(R.color.colorRed));
                    }*//*
                    view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                    binding.targetProgressTextLay.addView(view, paramsview);
                }
            }
        });

        mProgressItem = new ProgressItem();
        mProgressItem.progressItemPercentage = 60;
        mProgressItem.color = R.color.colorProgress;
        progressItemList.add(mProgressItem);

       *//* mProgressItem = new ProgressItem();
        mProgressItem.progressItemPercentage = TargetProgress;
        mProgressItem.color = R.color.colorRed;
        progressItemList.add(mProgressItem);*//*

        mProgressItem = new ProgressItem();
        mProgressItem.progressItemPercentage = 40;
        mProgressItem.color = R.color.colorMoreDarkGray;
        progressItemList.add(mProgressItem);

        binding.customProgressbar.getThumb().mutate().setAlpha(0);
        binding.customProgressbar.initData(progressItemList);
        binding.customProgressbar.invalidate();
    }*/

    private void setOnClickListners() {
        binding.txtExtendQuota.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.txtExtendQuota:
                Intent intent = new Intent(LoginDevicesActivity.this, ExtendQuotaRequestActivity.class);
                intent.putExtra("deviceId", deviceId);
                startActivityForResult(intent, 100);
                break;
        }
    }

    @Override
    public void showBottomDialog(Long id) {
        showBottom(id);
    }

    private void showBottom(Long DeviceId) {
        mBottomSheetDialog = new BottomSheetDialog(LoginDevicesActivity.this);
        ActivateDeavtivateBottomSheetBinding bottomSheetBinding = DataBindingUtil.inflate(LayoutInflater.from(LoginDevicesActivity.this), R.layout.activate_deavtivate_bottom_sheet, null, false);

        mBottomSheetDialog.setContentView(bottomSheetBinding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) bottomSheetBinding.getRoot().getParent());
        CompositeDisposable disposable = new CompositeDisposable();
        DeviceManagementRepository deviceManagementRepository = new DeviceManagementRepository();

        bottomSheetBinding.cardViewActivateDeactivate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showDialog(LoginDevicesActivity.this.getResources().getString(R.string.loading));
                disposable.add(deviceManagementRepository.chaneDeviceStatus(String.valueOf(DeviceId))
                        .subscribeOn(Schedulers.io())
                        .observeOn(AndroidSchedulers.mainThread())
                        .subscribeWith(new DisposableSingleObserver<Response<DeviceListModel>>() {
                            @Override
                            public void onSuccess(Response<DeviceListModel> deviceListModel) {
                                if (deviceListModel.body() != null && deviceListModel.body().getMessage() != null)
                                    Toast.makeText(LoginDevicesActivity.this, deviceListModel.body().getMessage(), Toast.LENGTH_LONG).show();
                                mBottomSheetDialog.dismiss();
                                callApiDeviceList();
                                hideDialog();
                                if (deviceListModel != null) {
                                    if (deviceListModel.body() != null) {
                                        if (deviceListModel.body().getMessage().contains("Device activated.")) {
                                            deviceStatus = 0;
                                            SharedPreferences deviceStatusPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                            SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                            editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                            editor.apply();
                                            Log.e("activated", "Device activated: " + String.valueOf(deviceStatus));
                                        } else if (deviceListModel.body().getMessage().contains("Device deactivated.")) {
                                            deviceStatus = 2;
                                            SharedPreferences deviceStatusPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                            SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                            editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                            editor.apply();
                                            Log.e("activated", "Device deactivated: " + String.valueOf(deviceStatus));
                                        }
                                    }

                                    if ((deviceListModel.errorBody() != null)) {

                                        Long errorCode = null;
                                        try {
                                            errorCode = new Gson().fromJson(deviceListModel.errorBody().string(), DeviceRegisterModel.class).getResponseCode();
                                        } catch (IOException e) {
                                            e.printStackTrace();
                                        }
                                        if (errorCode == 3) {
                                            deviceStatus = 3;
                                            SharedPreferences deviceStatusPreferences = getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                            SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                            editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                            editor.apply();
                                            Log.e("activated", "Device out of quota: " + String.valueOf(deviceStatus));
                                            showOutOfDeviceQuota();
                                        }
                                    }
                                }

                            }

                            @Override
                            public void onError(Throwable e) {
                                hideDialog();
                            }
                        }));
            }
        });

        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(bottomSheetBinding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
    }

    private void showOutOfDeviceQuota() {
        try {
            SpannableStringBuilder message = setTypeface(LoginDevicesActivity.this, getString(R.string.out_of_quota_message));
            AlertDialog.Builder builder = new AlertDialog.Builder(LoginDevicesActivity.this);
            builder.setTitle(R.string.validation_warning);
            builder.setMessage(message)
                    .setPositiveButton(R.string.button_ok, new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                        }
                    });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 100 && resultCode == RESULT_OK) {
            binding.txtAlreadyPendingQuota.setVisibility(View.VISIBLE);
            binding.txtExtendQuota.setVisibility(View.GONE);
        }
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finishAffinity();
        startActivity(new Intent(LoginDevicesActivity.this, MainDashBoardActivity.class));
    }
}

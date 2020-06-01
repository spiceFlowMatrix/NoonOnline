package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.Gravity;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.LinearLayout;
import android.widget.TextView;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.ibl.apps.Adapter.AllDeviceListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DeviceManagement.DeviceManagementRepository;
import com.ibl.apps.Model.ProgressItem;
import com.ibl.apps.Model.deviceManagement.DeviceListModel;
import com.ibl.apps.noon.databinding.ActivateDeavtivateBottomSheetBinding;
import com.ibl.apps.noon.databinding.ActivityLoginDevicesBinding;

import java.util.ArrayList;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

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
            finish();
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
                                progressItemList.clear();
                                mProgressItem = new ProgressItem();
                                mProgressItem.progressItemPercentage = deviceListModel.getData().getCurrentConsumption() / deviceListModel.getData().getDeviceLimit() * 100;
                                mProgressItem.color = R.color.colorProgress;
                                progressItemList.add(mProgressItem);

                                /*mProgressItem = new ProgressItem();
                                mProgressItem.progressItemPercentage = TargetProgress;
                                mProgressItem.color = R.color.colorRed;
                                progressItemList.add(mProgressItem);*/

                                mProgressItem = new ProgressItem();
                                mProgressItem.progressItemPercentage = 100 - (deviceListModel.getData().getCurrentConsumption() / deviceListModel.getData().getDeviceLimit() * 100);
                                mProgressItem.color = R.color.colorMoreDarkGray;
                                progressItemList.add(mProgressItem);

                                binding.customProgressbar.getThumb().mutate().setAlpha(0);
                                binding.customProgressbar.initData(progressItemList);
                                binding.customProgressbar.invalidate();

                                binding.targetProgressTextLay.removeAllViews();
                                float a1 = progressItemList.get(0).progressItemPercentage;
                                int width = binding.targetProgressTextLay.getMeasuredWidth();
                                int height = binding.targetProgressTextLay.getMeasuredHeight();

                                int targetMarginFromInt = (int) ((width * a1) / 100);
                                //CurrentConsumption
                                LinearLayout.LayoutParams params = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                                if (a1 == 100) {
                                    params.setMargins(targetMarginFromInt - 120, 0, 0, 0);
                                } else {
                                    params.setMargins(targetMarginFromInt, 0, 0, 0);
                                }
                                TextView aTrueText = new TextView(LoginDevicesActivity.this);
                                String currentConsumption = getString(R.string.device_current_quota) + "\n" + deviceListModel.getData().getCurrentConsumption();
                                aTrueText.setText(currentConsumption);
                                aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                                aTrueText.setTextSize(12);
                                aTrueText.setGravity(Gravity.CENTER);
                                binding.targetProgressTextLay.addView(aTrueText, params);
                                LinearLayout.LayoutParams paramsview = new LinearLayout.LayoutParams(5, 30);
                                if (a1 == 100) {
                                    paramsview.setMargins(targetMarginFromInt - 5, 0, 0, 0);
                                } else {
                                    paramsview.setMargins(targetMarginFromInt, 0, 0, 0);
                                }
                                View view = new View(LoginDevicesActivity.this);
                                view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                                binding.targetProgressTextLay.addView(view, paramsview);

                                //Total Quota
                                binding.trueProgressTextLay.removeAllViews();
                                LinearLayout.LayoutParams paramsviewTotal = new LinearLayout.LayoutParams(5, 30);
                                paramsviewTotal.setMargins(940, 0, 0, 0);
                                View Total = new View(LoginDevicesActivity.this);
                                Total.setBackgroundColor(getResources().getColor(R.color.colorRed));
                                binding.trueProgressTextLay.addView(Total, paramsviewTotal);

                                LinearLayout.LayoutParams params1 = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
                                params1.setMargins(800, 0, 0, 0);
                                TextView aTrueText1 = new TextView(LoginDevicesActivity.this);
                                String totalQuota = deviceListModel.getData().getDeviceLimit() + "\n" + getString(R.string.device_total_quota);
                                aTrueText1.setText(totalQuota);
                                aTrueText1.setTextColor(getResources().getColor(R.color.colorDarkGray));
                                aTrueText1.setTextSize(12);
                                aTrueText1.setGravity(Gravity.CENTER);
                                binding.trueProgressTextLay.addView(aTrueText1, params1);



                                /*if (deviceListModel.getData().getDeviceLimit() != null)
                                    totalQuotaCount(deviceListModel.getData().getDeviceLimit());

                                if (deviceListModel.getData().getCurrentConsumption() != null)
                                    currentDeviceConsumption(deviceListModel.getData().getCurrentConsumption());*/


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

/*    private void currentDeviceConsumption(Long currentConsumption) {
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
                    aTrueText.setText(getString(R.string.device_current_quota) + "\n" + currentConsumption);
                    aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                    aTrueText.setTextSize(12);
                    aTrueText.setGravity(Gravity.CENTER);
                    binding.targetProgressTextLay.addView(aTrueText, params);

                    LinearLayout.LayoutParams paramsview = new LinearLayout.LayoutParams(5, 30);
                    paramsview.setMargins(targetMarginFromInt - 4, 0, 0, 0);
                    View view = new View(LoginDevicesActivity.this);

                    if (aTrueProgress > intpassingMarks) {
                        view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                    } else {
                        view.setBackgroundColor(getResources().getColor(R.color.colorRed));
                    }
                    view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                    binding.targetProgressTextLay.addView(view, paramsview);
                }
            }
        });
    }*/

   /* private void totalQuotaCount(Long deviceLimit) {
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

                    aTrueText.setText(deviceLimit + "\n" + getString(R.string.device_total_quota));
                    aTrueText.setTextColor(getResources().getColor(R.color.colorDarkGray));
                    aTrueText.setTextSize(12);
                    aTrueText.setGravity(Gravity.CENTER);
                    binding.trueProgressTextLay.addView(aTrueText, params);
                }
            }
        });
    }*/

    private void getSeekbarProgress() {
        int aTrueProgress = (int) ((60) * 100 / 60);
        int intpassingMarks = Integer.parseInt("60");
        int TargetProgress = intpassingMarks - aTrueProgress;
        int LeftProgress = 100 - intpassingMarks;

       /* if (aTrueProgress > intpassingMarks) {
            LeftProgress = LeftProgress + (TargetProgress);
            TargetProgress = 0;
        }*/

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

                  /*  if (aTrueProgress > intpassingMarks) {
                        view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                    } else {
                        view.setBackgroundColor(getResources().getColor(R.color.colorRed));
                    }*/
                    view.setBackgroundColor(getResources().getColor(R.color.colorProgress));
                    binding.targetProgressTextLay.addView(view, paramsview);
                }
            }
        });

        mProgressItem = new ProgressItem();
        mProgressItem.progressItemPercentage = 60;
        mProgressItem.color = R.color.colorProgress;
        progressItemList.add(mProgressItem);

       /* mProgressItem = new ProgressItem();
        mProgressItem.progressItemPercentage = TargetProgress;
        mProgressItem.color = R.color.colorRed;
        progressItemList.add(mProgressItem);*/

        mProgressItem = new ProgressItem();
        mProgressItem.progressItemPercentage = 40;
        mProgressItem.color = R.color.colorMoreDarkGray;
        progressItemList.add(mProgressItem);

        binding.customProgressbar.getThumb().mutate().setAlpha(0);
        binding.customProgressbar.initData(progressItemList);
        binding.customProgressbar.invalidate();
    }

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
                        .subscribeWith(new DisposableSingleObserver<DeviceListModel>() {
                            @Override
                            public void onSuccess(DeviceListModel deviceListModel) {
                                if (deviceListModel != null && deviceListModel.getMessage() != null)
                                    Toast.makeText(LoginDevicesActivity.this, deviceListModel.getMessage(), Toast.LENGTH_LONG).show();
                                mBottomSheetDialog.dismiss();
                                callApiDeviceList();
                                hideDialog();
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

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 100 && resultCode == RESULT_OK) {
            binding.txtAlreadyPendingQuota.setVisibility(View.VISIBLE);
            binding.txtExtendQuota.setVisibility(View.GONE);
        }
    }
}

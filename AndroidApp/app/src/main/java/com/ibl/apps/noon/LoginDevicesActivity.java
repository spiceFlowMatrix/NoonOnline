package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.view.Gravity;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.ibl.apps.Adapter.AllDeviceListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.ProgressItem;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.noon.databinding.ActivityLoginDevicesBinding;

import java.util.ArrayList;

public class LoginDevicesActivity extends BaseActivity implements View.OnClickListener {
    ActivityLoginDevicesBinding binding;
    AllDeviceListAdapter allDeviceListAdapter;
    ArrayList<ProgressItem> progressItemList = new ArrayList<>();
    ProgressItem mProgressItem;

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
//        int aTrueProgress = (int) ((aTrueCount) * 100 / quizQuestionsObjectList.size());

        getSeekbarProgress();

        setOnClickListners();
        binding.toolBar.setNavigationOnClickListener(view -> {
            finish();
        });
    }

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
                startActivity(new Intent(this, ExtendQuotaRequestActivity.class));
                break;
        }
    }
}

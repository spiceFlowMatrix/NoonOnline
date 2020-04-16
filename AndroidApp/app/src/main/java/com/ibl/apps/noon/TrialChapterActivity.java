package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;

import com.ibl.apps.Adapter.TrialCourseItemListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.noon.databinding.ActivityChapterTrialBinding;

public class TrialChapterActivity extends BaseActivity implements View.OnClickListener {
    ActivityChapterTrialBinding binding;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_chapter_trial;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityChapterTrialBinding) getBindObj();

        binding.btnbackcourseItem.setOnClickListener(this);
        binding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
        TrialCourseItemListAdapter adapter = new TrialCourseItemListAdapter(TrialChapterActivity.this);
        binding.rcVerticalLayout.rcVertical.setAdapter(adapter);
        setOnClickListener();
    }

    private void setOnClickListener() {

    }

    @Override
    public void onClick(View v) {
        switch (v.getId()) {
            case R.id.btnbackcourseItem:
                finish();
        }
    }
}

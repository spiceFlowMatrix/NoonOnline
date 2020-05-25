package com.ibl.apps.noon;

import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;

import androidx.databinding.DataBindingUtil;
import androidx.fragment.app.Fragment;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.ibl.apps.Adapter.FeedBackPagerAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Fragment.CompletedFragment;
import com.ibl.apps.Fragment.ProgressFragment;
import com.ibl.apps.Fragment.QueueFragment;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.noon.databinding.ActivityFeedBackBinding;
import com.ibl.apps.noon.databinding.BottomSheetBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.GlideApp;

import java.util.ArrayList;

public class FeedBackActivity extends BaseActivity implements View.OnClickListener {

    private ActivityFeedBackBinding binding;
    ArrayList<Fragment> fragments = new ArrayList<>();
    ArrayList<String> titles = new ArrayList<>();
    private String ErrorSync;


    @Override
    protected int getContentView() {
        return R.layout.activity_feed_back;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityFeedBackBinding) getBindObj();
        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
        String userId = sharedPreferencesuser.getString("uid", "");
        setToolbar(binding.toolbarLayout.toolBar);
        showBackArrow(getResources().getString(R.string.feedback));
        SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
        binding.toolbarLayout.toolBar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                finish();
                Intent intent1 = new Intent(FeedBackActivity.this, MainDashBoardActivity.class);
                startActivity(intent1);
            }
        });
        setUpViewPager();
        binding.toolbarLayout.cacheEventsStatusBtn.setVisibility(View.VISIBLE);
        SharedPreferences sharedPreferenceCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
        String flagStatus = sharedPreferenceCache.getString("FlagStatus", "");
        switch (flagStatus) {
            case "1":
                binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_pending);
                break;
            case "2":
                binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_error);
                break;
            case "3":
                binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_syncing);
                break;
            case "4":
                GlideApp.with(FeedBackActivity.this)
                        .load(R.drawable.ic_cache_empty)
                        .error(R.drawable.ic_cache_empty)
                        .into(binding.toolbarLayout.cacheEventsStatusBtn);
                break;
        }

        binding.toolbarLayout.cacheEventsStatusBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(FeedBackActivity.this, CacheEventsListActivity.class));
            }
        });

        if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() >= 50) {
            showHitLimitDialog(FeedBackActivity.this);
        }
        //setOnClick();
    }


    private void setOnClick() {
        binding.layReportProblem.setOnClickListener(this);
    }

    private void setUpViewPager() {
        fragments.add(0, new QueueFragment());
        fragments.add(1, new ProgressFragment());
        fragments.add(2, new CompletedFragment());

        titles.add(0, getResources().getString(R.string.in_queue));
        titles.add(1, getResources().getString(R.string.in_progress));
        titles.add(2, getResources().getString(R.string.completed));

        FeedBackPagerAdapter adapter = new FeedBackPagerAdapter(getSupportFragmentManager());
        adapter.setFragmentList(fragments);
        adapter.setTitles(titles);
        binding.viewpager.setAdapter(adapter);
        binding.tablay.setupWithViewPager(binding.viewpager);
    }

    @Override
    public void onClick(View view) {
        Intent reportIntent = new Intent(FeedBackActivity.this, ReportProblemActivity.class);
        Intent filesIntent = new Intent(FeedBackActivity.this, FilesFeedbackActivity.class);

        switch (view.getId()) {
            case R.id.lay_report_problem:
                try {
                    showBottomSheet();
                } catch (Exception e) {
                    e.printStackTrace();
                }

                break;
            case R.id.lay_isnt_working:
//                reportIntent.putExtra(Const.Flag, getResources().getString(R.string.something_isn_t_working_app));
                finish();
                reportIntent.putExtra(Const.Flag, 1);
                startActivity(reportIntent);
                break;
            case R.id.lay_video_issue:
//                filesIntent.putExtra(Const.Flag, getResources().getString(R.string.video_issue));
                finish();
                filesIntent.putExtra(Const.Flag, 2);
                startActivity(filesIntent);
                break;
            case R.id.lay_text_issue:
//                filesIntent.putExtra(Const.Flag, getResources().getString(R.string.text_issue));
                finish();
                filesIntent.putExtra(Const.Flag, 3);
                startActivity(filesIntent);
                break;
            case R.id.lay_lesson_assignmnet:
//                filesIntent.putExtra(Const.Flag, getResources().getString(R.string.lesson_assignment_issue));
                finish();
                filesIntent.putExtra(Const.Flag, 4);
                startActivity(filesIntent);
                break;
            case R.id.lay_chapter_assignment:
//                filesIntent.putExtra(Const.Flag, getResources().getString(R.string.chapter_assignment_issue));
                finish();
                filesIntent.putExtra(Const.Flag, 5);
                startActivity(filesIntent);
                break;
            case R.id.lay_quiz_issue:
//                filesIntent.putExtra(Const.Flag, getResources().getString(R.string.quiz_issue));
                finish();
                filesIntent.putExtra(Const.Flag, 6);
                startActivity(filesIntent);
                break;
            case R.id.lay_general_feedback:
                finish();
                reportIntent.putExtra(Const.Flag, 7);
                startActivity(reportIntent);
                break;
        }
    }

    private void showBottomSheet() {
        BottomSheetDialog mBottomSheetDialog = new BottomSheetDialog(this);
        BottomSheetBinding binding = DataBindingUtil.inflate(LayoutInflater.from(this), R.layout.bottom_sheet, null, false);
        binding.layIsntWorking.setOnClickListener(this::onClick);
        binding.layVideoIssue.setOnClickListener(this);
        binding.layTextIssue.setOnClickListener(this);
        binding.layLessonAssignmnet.setOnClickListener(this);
        binding.layChapterAssignment.setOnClickListener(this);
        binding.layQuizIssue.setOnClickListener(this);
        binding.layGeneralFeedback.setOnClickListener(this);

        mBottomSheetDialog.setContentView(binding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) binding.getRoot().getParent());
        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(binding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finish();
        Intent intent1 = new Intent(FeedBackActivity.this, MainDashBoardActivity.class);
        startActivity(intent1);
    }

    //    @Override
//    public void finish() {
//        ViewGroup view = (ViewGroup) getWindow().getDecorView();
//        view.removeAllViews();
//        super.finish();
//    }

}

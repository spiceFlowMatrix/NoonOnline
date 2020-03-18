package com.ibl.apps.noon;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.BottomNavigationView;
import android.support.v4.view.ViewPager;
import android.view.MenuItem;
import android.view.View;

import com.downloader.PRDownloader;
import com.google.gson.Gson;
import com.ibl.apps.Adapter.ViewPagerAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Fragment.CourseItemFragment;
import com.ibl.apps.Fragment.DiscussionsFragment;
import com.ibl.apps.Fragment.LibraryFragment;
import com.ibl.apps.Interface.BackInterface;
import com.ibl.apps.Interface.ToolbarHideInterface;
import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.noon.databinding.ChapterMainBinding;

import java.util.Objects;


public class ChapterActivity extends BaseActivity implements View.OnClickListener, ToolbarHideInterface, BackInterface {

    ChapterMainBinding chapterMainBinding;

    //Fragments
    CourseItemFragment courseItemFragment;
    //    AssignmentFragment assignmentFragment;
    LibraryFragment libraryFragment;
    DiscussionsFragment discussionsFragment;
    //ReportFragment progressReportFragment;


    MenuItem prevMenuItem;
    UserDetails userDetailsObject = new UserDetails();
    String GradeId, CourseName, ActivityFlag, LessonID, QuizID;
    ViewPagerAdapter adapter;
    Boolean isNotification = false;
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";
    private String profileurl;
    private String userName;
    private String cretedTime;
    private boolean iseditable;
    private int likeCounts;
    private int dislikeCount;
    private boolean isLiked;
    private boolean isDisliked;

    @Override
    protected int getContentView() {
        return R.layout.chapter_main;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);

        chapterMainBinding = (ChapterMainBinding) getBindObj();

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    setupViewPager(chapterMainBinding.fragmentViewpager);
                }
                return null;
            }
        }).execute();

        chapterMainBinding.bottomNavigation.setOnNavigationItemSelectedListener(
                new BottomNavigationView.OnNavigationItemSelectedListener() {
                    @Override
                    public boolean onNavigationItemSelected(@NonNull MenuItem item) {

                        switch (item.getItemId()) {
                            case R.id.action_item1:
                                chapterMainBinding.fragmentViewpager.setCurrentItem(0);
                                break;
                            case R.id.action_item3:
                                chapterMainBinding.fragmentViewpager.setCurrentItem(1);
                                break;
                            case R.id.action_item4:
                                chapterMainBinding.fragmentViewpager.setCurrentItem(2);
                                break;
                        }
                        return false;
                    }
                });

        chapterMainBinding.fragmentViewpager.addOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {

                switch (position) {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }

            @Override
            public void onPageSelected(int position) {
                if (prevMenuItem != null) {
                    prevMenuItem.setChecked(false);
                } else {
                    chapterMainBinding.bottomNavigation.getMenu().getItem(0).setChecked(false);
                }
                //Log.d("page", "onPageSelected: " + position);
                chapterMainBinding.bottomNavigation.getMenu().getItem(position).setChecked(true);
                prevMenuItem = chapterMainBinding.bottomNavigation.getMenu().getItem(position);

                chapterMainBinding.toolbarLay.setVisibility(View.GONE);

                switch (position) {
                    case 0:
                        break;
                    case 1:
                        //discussionsFragment.DiscussionsFragment();
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                }
            }

            @Override
            public void onPageScrollStateChanged(int state) {

            }
        });

        if (getIntent() != null) {
            GradeId = getIntent().getStringExtra(Const.GradeID);
            CourseName = getIntent().getStringExtra(Const.CourseName);
            ActivityFlag = getIntent().getStringExtra(Const.ActivityFlag);
            LessonID = getIntent().getStringExtra(Const.LessonID);
            QuizID = getIntent().getStringExtra(Const.QuizID);
            isNotification = Objects.requireNonNull(getIntent().getExtras()).getBoolean(Const.isNotification, false);
            AddtionalLibrary = getIntent().getStringExtra(Const.AddtionalLibrary);
            AddtionalAssignment = getIntent().getStringExtra(Const.AddtionalAssignment);
            AddtionalDiscussions = getIntent().getStringExtra(Const.AddtionalDiscussions);
            profileurl = getIntent().getStringExtra(Const.profileurl);
            userName = getIntent().getStringExtra(Const.userName);
            cretedTime = getIntent().getStringExtra(Const.createdtime);
            iseditable = getIntent().getBooleanExtra(Const.iseditable, false);
            likeCounts = getIntent().getIntExtra(Const.likecount, 0);
            dislikeCount = getIntent().getIntExtra(Const.dislikecount, 0);
            isLiked = getIntent().getBooleanExtra(Const.like, false);
            isDisliked = getIntent().getBooleanExtra(Const.dislike, false);

            if (getIntent().hasExtra(Const.Addtionalservice)) {
                Gson gson = new Gson();
                CourseObject.Addtionalservice model = gson.fromJson(getIntent().getExtras().getString(Const.Addtionalservice, ""), CourseObject.Addtionalservice.class);

                if (model != null) {
                    AddtionalLibrary = model.getLibrary();
                    AddtionalAssignment = model.getAssignment();
                    AddtionalDiscussions = model.getDiscussion();
                }
            }
        }
        setToolbar(chapterMainBinding.toolbarLayout.toolBar);
        showBackArrow(CourseName);
        setOnClickListener();

        chapterMainBinding.toolbarLayout.toolBar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finishActivity();
            }
        });
    }

    private void setupViewPager(ViewPager viewPager) {

        adapter = new ViewPagerAdapter(getSupportFragmentManager());
        courseItemFragment = new CourseItemFragment();

        discussionsFragment = new DiscussionsFragment();
        libraryFragment = new LibraryFragment();

//        assignmentFragment = new AssignmentFragment();
        //progressReportFragment = new ReportFragment();

        Bundle bundle = new Bundle();
        bundle.putString(Const.GradeID, GradeId);
        bundle.putString(Const.CourseName, CourseName);
        bundle.putString(Const.ActivityFlag, ActivityFlag);
        bundle.putString(Const.LessonID, LessonID);
        bundle.putString(Const.QuizID, QuizID);
        bundle.putString(Const.AddtionalLibrary, AddtionalLibrary);
        bundle.putString(Const.AddtionalAssignment, AddtionalAssignment);
        bundle.putString(Const.AddtionalDiscussions, AddtionalDiscussions);
        bundle.putBoolean(Const.isNotification, isNotification);

        courseItemFragment.setArguments(bundle);

        libraryFragment.setArguments(bundle);
        discussionsFragment.setArguments(bundle);
//        assignmentFragment.setArguments(bundle);
        //progressReportFragment.setArguments(bundle);

        adapter.addFragment(courseItemFragment);

        adapter.addFragment(discussionsFragment);
        adapter.addFragment(libraryFragment);
//        adapter.addFragment(assignmentFragment);
        //adapter.addFragment(progressReportFragment);
        viewPager.setAdapter(adapter);

        if (getIntent().hasExtra(Const.isDiscussions)) {
            Boolean isDiscussions = getIntent().getExtras().getBoolean(Const.isDiscussions, false);
            if (isDiscussions) {
                viewPager.setCurrentItem(1);
                if (getIntent().hasExtra(Const.isNotification)) {
                    Boolean isNotification = getIntent().getExtras().getBoolean(Const.isNotification, false);
                    if (isNotification) {
                        Intent i = new Intent(getApplicationContext(), DiscussionsDetailActivity.class);
                        i.putExtra(Const.topicId, getIntent().getStringExtra(Const.topicId));
                        i.putExtra(Const.topicname, getIntent().getStringExtra(Const.topicname));
                        i.putExtra(Const.isprivate, getIntent().getStringExtra(Const.isprivate));
                        i.putExtra(Const.GradeID, GradeId);
                        i.putExtra(Const.CourseName, CourseName);
                        i.putExtra(Const.ActivityFlag, ActivityFlag);
                        i.putExtra(Const.LessonID, LessonID);
                        i.putExtra(Const.QuizID, QuizID);
                        i.putExtra(Const.profileurl, profileurl);
                        i.putExtra(Const.userName, userName);
                        i.putExtra(Const.createdtime, cretedTime);
                        i.putExtra(Const.iseditable, iseditable);
                        i.putExtra(Const.likecount, likeCounts);
                        i.putExtra(Const.dislikecount, dislikeCount);
                        i.putExtra(Const.liked, isLiked);
                        i.putExtra(Const.disliked, isDisliked);
                        startActivity(i);
                        finish();
                    }
                }
            }
        }
    }

    public void setOnClickListener() {
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case 0:
                break;
        }
    }

    @Override
    public void onBackPressed() {
        if (getSupportFragmentManager().getBackStackEntryCount() > 0) {
            getSupportFragmentManager().popBackStack();
        } else {
            super.onBackPressed();
            finishActivity();
        }
    }

    @Override
    public void toolbarHide(Context ctx, boolean flag, String LessonName) {
        if (flag) {
            chapterMainBinding.toolbarLay.setVisibility(View.GONE);
        } else {
            chapterMainBinding.toolbarLay.setVisibility(View.VISIBLE);
            chapterMainBinding.toolbarLayout.toolBar.setSubtitle(LessonName);
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (adapter != null) {
            if (adapter.getCount() != 0) {
                adapter.notifyDataSetChanged();
                PRDownloader.cancelAll();
            }
        }
    }

    public void finishActivity() {
        PRDownloader.cancelAll();
        Intent intent = new Intent(getApplicationContext(), MainDashBoardActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivity(intent);
        finish();
    }

    @Override
    public void backfragment() {
        finishActivity();
    }


}

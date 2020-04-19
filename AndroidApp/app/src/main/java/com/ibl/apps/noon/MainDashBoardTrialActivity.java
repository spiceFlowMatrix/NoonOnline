package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.text.TextUtils;
import android.view.MenuItem;
import android.view.View;

import androidx.annotation.NonNull;
import androidx.viewpager.widget.ViewPager;

import com.google.android.material.bottomnavigation.BottomNavigationView;
import com.ibl.apps.Adapter.ViewPagerAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Fragment.TrialGradeFragment;
import com.ibl.apps.Fragment.TrialProfileFragment;
import com.ibl.apps.noon.databinding.ActivityMainDashBoardTrialBinding;

public class MainDashBoardTrialActivity extends BaseActivity {

    ActivityMainDashBoardTrialBinding binding;
    TrialProfileFragment trialProfileFragment;
    TrialGradeFragment trialGradeFragment;
    private MenuItem prevMenuItem;
    private boolean hideVisible = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_main_dash_board_trial;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityMainDashBoardTrialBinding) getBindObj();

        binding.toolBar.setNavigationIcon(null);
        binding.toolbarTitle.setText(getResources().getString(R.string.my_courses));
        binding.toolbarButtonLay.setVisibility(View.VISIBLE);
        setupViewPager(binding.courseViewpager);
        initListener();
    }

    private void initListener() {
        binding.bottomNavigation.setOnNavigationItemSelectedListener(
                new BottomNavigationView.OnNavigationItemSelectedListener() {
                    @Override
                    public boolean onNavigationItemSelected(@NonNull MenuItem item) {
                        switch (item.getItemId()) {
                            case R.id.action_item1:
                                binding.courseViewpager.setCurrentItem(0);
                                binding.toolbarTitle.setText(getResources().getString(R.string.my_courses));
                                binding.toolbarButtonLay.setVisibility(View.VISIBLE);
                                binding.toolbarresetPassLay.setVisibility(View.GONE);
                                binding.toolBar.setVisibility(View.VISIBLE);
                                break;
                            case R.id.action_item3:
                                binding.toolBar.setVisibility(View.VISIBLE);
                                binding.courseViewpager.setCurrentItem(1);
                                break;
                        }
                        return false;
                    }
                });

        binding.courseViewpager.addOnPageChangeListener(new ViewPager.OnPageChangeListener() {
            @Override
            public void onPageScrolled(int position, float positionOffset, int positionOffsetPixels) {
                switch (position) {
                    case 0:
                        break;
                    case 1:
                        binding.toolBar.setVisibility(View.VISIBLE);
                        setPos2View();
                        break;
                }
            }

            @Override
            public void onPageSelected(int position) {
                switch (position) {
                    case 0:
                        binding.toolBar.setVisibility(View.VISIBLE);
                        binding.toolbarButtonLay.setVisibility(View.VISIBLE);
                        binding.toolbarresetPassLay.setVisibility(View.GONE);
                        break;
                    case 1:
                        binding.toolbarButtonLay.setVisibility(View.GONE);
                        binding.toolbarresetPassLay.setVisibility(View.VISIBLE);
                        binding.toolBar.setVisibility(View.VISIBLE);
                        break;
                }
                if (prevMenuItem != null) {
                    prevMenuItem.setChecked(false);
                } else {
                    binding.bottomNavigation.getMenu().getItem(0).setChecked(false);
                }
                binding.bottomNavigation.getMenu().getItem(position).setChecked(true);
                prevMenuItem = binding.bottomNavigation.getMenu().getItem(position);
            }

            @Override
            public void onPageScrollStateChanged(int state) {

            }

        });
    }


    public void setPos2View() {
        binding.toolbarButtonLay.setVisibility(View.GONE);
        binding.toolbarresetPassLay.setVisibility(View.VISIBLE);

       /* PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {

            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {

                if (userDetails != null) {
                    if (userDetails.getUsername() != null) {
                        String ProfileUsername = userDetails.getUsername();
                        if (!TextUtils.isEmpty(ProfileUsername)) {
                            binding.toolbarTitle.setText(ProfileUsername);
                        }
                    }
                }
                return null;
            }

        }).execute();*/

//        trialProfileFragment.checkCurrScreenApi();
        binding.editprofileBtn.setImageResource(R.drawable.ic_check_black);
        trialProfileFragment.hideVisibleLay(true);
        hideVisible = false;
    }


    private void setupViewPager(ViewPager viewPager) {
        ViewPagerAdapter adapter = new ViewPagerAdapter(getSupportFragmentManager());
        trialGradeFragment = new TrialGradeFragment();
        trialProfileFragment = new TrialProfileFragment();
        adapter.addFragment(trialGradeFragment);
        adapter.addFragment(trialProfileFragment);
        viewPager.setAdapter(adapter);
    }
}
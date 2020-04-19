package com.ibl.apps.Fragment;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;

import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentTrialProfileBinding;

public class TrialProfileFragment extends BaseFragment {
    FragmentTrialProfileBinding binding;

    public TrialProfileFragment() {
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        binding = DataBindingUtil.inflate(inflater, R.layout.fragment_trial_profile, container, false);
        return binding.getRoot();
    }

    public void hideVisibleLay(boolean flag) {

        binding.userProfileDataLay.setVisibility(View.VISIBLE);

        /*if (flag) {
            profileLayoutBinding.userStatisticsLay.setVisibility(View.VISIBLE);
            profileLayoutBinding.userProfileDataLay.setVisibility(View.GONE);
        } else {
            profileLayoutBinding.userStatisticsLay.setVisibility(View.GONE);
            profileLayoutBinding.userProfileDataLay.setVisibility(View.VISIBLE);
        }*/
    }

    @Override
    protected void setUp(View view) {

    }
}

package com.ibl.apps.Fragment;

import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;

import com.ibl.apps.Adapter.TrialCourseListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentTrialGradeBinding;

public class TrialGradeFragment extends BaseFragment {
    private FragmentTrialGradeBinding binding;
    private TrialCourseListAdapter adp;

    public TrialGradeFragment() {
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        binding = DataBindingUtil.inflate(inflater, R.layout.fragment_trial_grade, container, false);
        return binding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        binding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
        adp = new TrialCourseListAdapter(getActivity());
        binding.rcVerticalLayout.rcVertical.setAdapter(adp);
    }
}

package com.ibl.apps.Fragment;


import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentReportBinding;

import java.util.Objects;

public class ReportFragment extends BaseFragment {


    public ReportFragment() {
        // Required empty public constructor
    }
    FragmentReportBinding reportBinding;

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        reportBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_report,container,false);
        return reportBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        Objects.requireNonNull(getActivity()).getSupportFragmentManager()
                .beginTransaction()
                .replace(R.id.frame, new ProgressReportFragment(),"ProgressReportFragment")
                .commit();
    }
}

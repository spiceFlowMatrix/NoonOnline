package com.ibl.apps.Fragment;

import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Interface.IOnBackPressed;
import com.ibl.apps.Model.APICourseObject;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentComplaintLayoutBinding;

import java.util.ArrayList;

import io.reactivex.disposables.CompositeDisposable;


public class ComplaintFragment extends BaseFragment implements View.OnClickListener, IOnBackPressed {

    FragmentComplaintLayoutBinding fragmentComplaintLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    ArrayList<APICourseObject.Data> courseList = new ArrayList<>();

    public ComplaintFragment() {
        // Required empty public constructor
    }

    public static ComplaintFragment newInstance(String param1, String param2) {
        ComplaintFragment fragment = new ComplaintFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        fragmentComplaintLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_complaint_layout, container, false);
        return fragmentComplaintLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {


        setOnClickListener();


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
    public boolean onBackPressed() {
        return false;
    }


}

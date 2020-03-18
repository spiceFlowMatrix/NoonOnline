package com.ibl.apps.Fragment;

import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.CourseAllocateListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Interface.IOnBackPressed;
import com.ibl.apps.Model.APICourseObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentCourseItemLayoutBinding;

import java.util.ArrayList;
import java.util.Collections;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

import static com.ibl.apps.Base.BaseActivity.apiService;


public class CourseFragment extends BaseFragment implements View.OnClickListener, IOnBackPressed {

    FragmentCourseItemLayoutBinding fragmentCourseItemLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    UserDetails userDetailsObject =new UserDetails();
    String userId = "0";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    ArrayList<APICourseObject.Data> courseList = new ArrayList<>();

    public CourseFragment() {
        //Required empty public constructor
    }

    public static CourseFragment newInstance(String param1, String param2) {
        CourseFragment fragment = new CourseFragment();
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
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState) {
        fragmentCourseItemLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_course_item_layout, container, false);
        return fragmentCourseItemLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                }
                return null;
            }

        }).execute();
        loadData();
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

    private void loadData() {
        try {

            showDialog(getString(R.string.loading));
            disposable.add(apiService.fetchcourse(userId, "1", "10")
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<APICourseObject>() {
                        @Override
                        public void onSuccess(APICourseObject apiCourseObject) {
                            hideDialog();

                            Collections.addAll(courseList, apiCourseObject.getData());
                            fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                            CourseAllocateListAdapter adp = new CourseAllocateListAdapter(getActivity(), courseList);
                            fragmentCourseItemLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                            adp.notifyDataSetChanged();

                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, libraryObject.getMessage());
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();

                            try {
                                HttpException error = (HttpException) e;
                                APICourseObject apiCourseObject = new Gson().fromJson(error.response().errorBody().string(), APICourseObject.class);
                                showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, apiCourseObject.getMessage());
                            } catch (Exception e1) {
                                showError(e);
                            }
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}

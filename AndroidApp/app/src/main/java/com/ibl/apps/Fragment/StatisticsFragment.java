package com.ibl.apps.Fragment;

import android.databinding.DataBindingUtil;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.gson.Gson;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.StatisticsObject;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentStatisticsLayoutBinding;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

import static com.ibl.apps.Base.BaseActivity.apiService;


public class StatisticsFragment extends BaseFragment implements View.OnClickListener {

    FragmentStatisticsLayoutBinding fragmentStatisticsLayoutBinding;

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    private CompositeDisposable disposable = new CompositeDisposable();

    public StatisticsFragment() {
        // Required empty public constructor
    }

    public static StatisticsFragment newInstance(String param1, String param2) {
        StatisticsFragment fragment = new StatisticsFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        setUserVisibleHint(false);
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }


    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        fragmentStatisticsLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_statistics_layout, container, false);
        return fragmentStatisticsLayoutBinding.getRoot();
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

    public void callApiStatistics() {

        showDialog(getString(R.string.loading));
        disposable.add(apiService.StatisticUser()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<StatisticsObject>() {
                    @Override
                    public void onSuccess(StatisticsObject user) {
                        fragmentStatisticsLayoutBinding.txtunreadNoti.setText(user.getData().getUnreadnotifications());
                        fragmentStatisticsLayoutBinding.txtaverageQuizScore.setText(user.getData().getAvragequizscore());
                        fragmentStatisticsLayoutBinding.txtuAssignmentScore.setText(user.getData().getAvrageassignmentscore());
                        fragmentStatisticsLayoutBinding.CompliteCourse.setText(user.getData().getComplatecourse());
                        fragmentStatisticsLayoutBinding.txtAvailableCourse.setText(user.getData().getTotalavailablescore());
                        fragmentStatisticsLayoutBinding.txtTerminatedCourse.setText(user.getData().getTerminatedcourse());
                        fragmentStatisticsLayoutBinding.txtTotalCourse.setText(user.getData().getTotalcourse());
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        try {
                            HttpException error = (HttpException) e;
                            StatisticsObject userObject = new Gson().fromJson(error.response().errorBody().string(), StatisticsObject.class);
                            //Log.e(Const.LOG_NOON_TAG, "==Refresh Token==" + userObject.getMessage());
                        } catch (Exception e1) {
                            //Log.e(Const.LOG_NOON_TAG, "==Refresh Token==" + e.getMessage());
                        }
                    }
                }));
    }

    public void callAPIforUser(Boolean flag) {
        if (flag) {
            callApiStatistics();
        }
    }

}

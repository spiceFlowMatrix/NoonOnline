package com.ibl.apps.Fragment;

import android.databinding.DataBindingUtil;
import android.os.AsyncTask;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.AssignmentListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.AssignmentObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.noon.AssignmentAddActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentAssignmentLayoutBinding;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

import static com.ibl.apps.Base.BaseActivity.apiService;


public class AssignmentFragment extends BaseFragment implements View.OnClickListener {

    FragmentAssignmentLayoutBinding fragmentAssignmentLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();


    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    String GradeId, CourseName;
    AssignmentListAdapter adp;
    UserDetails userDetailsObject = new UserDetails();
    String userRoleName = "";
    String userId = "0";

    public AssignmentFragment() {
        // Required empty public constructor
    }

    public static AssignmentFragment newInstance(String param1, String param2) {
        AssignmentFragment fragment = new AssignmentFragment();
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
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        fragmentAssignmentLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_assignment_layout, container, false);
        return fragmentAssignmentLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {

        Bundle bundle = this.getArguments();
        if (bundle != null) {
            GradeId = bundle.getString(Const.GradeID, "");
            CourseName = bundle.getString(Const.CourseName, "");
        }
        setToolbar(fragmentAssignmentLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(CourseName);
        fragmentAssignmentLayoutBinding.toolbarLayout.toolBar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                getActivity().finish();
            }
        });

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                    if (userDetails.getRoleName() != null) {
                        userRoleName = userDetails.getRoleName().get(0);
                        if (!TextUtils.isEmpty(userRoleName)) {
                            if (userRoleName.equals(Const.TeacherKEY)) {
                                fragmentAssignmentLayoutBinding.fabAssignmentReview.setVisibility(View.VISIBLE);
                            }
                        }
                    }

                    if (isNetworkAvailable(getActivity())) {
                        CallApiAssignmentList();
                    }
                }
                return null;
            }

        }).execute();

        setOnClickListener();
    }

    public void setOnClickListener() {
        fragmentAssignmentLayoutBinding.fabAssignmentReview.setOnClickListener(this::onClick);
    }

    private void CallApiAssignmentList() {

        showDialog(getActivity().getString(R.string.loading));
        disposable.add(apiService.fetchAssignments(GradeId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AssignmentObject>() {
                    @Override
                    public void onSuccess(AssignmentObject newassignmentObject) {

                        new setAssignmentTask(new AssignmentAsyncResponse() {
                            @Override
                            public AssignmentObject getAssignmentObject(AssignmentObject assignmentObject) {

                                if (assignmentObject.getData().size() == 0) {
                                    String myjson = "{\"response_code\":0,\"message\":\"Assignment Detail\",\"status\":\"Success\",\"data\":[{\"id\":9,\"name\":\"lesson-1cc\",\"description\":\"sadasd\",\"code\":\"AS-1559885237645\",\"chapterid\":129,\"submissioncount\":0,\"status\":0}]}";

                                    Gson gson = new Gson();
                                    assignmentObject = gson.fromJson(myjson, AssignmentObject.class);
                                }

                                fragmentAssignmentLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                adp = new AssignmentListAdapter(getActivity(), assignmentObject.getData(), userDetailsObject);
                                fragmentAssignmentLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
                                adp.notifyDataSetChanged();
                                hideDialog();

                                return null;
                            }
                        }, newassignmentObject).execute();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            AssignmentObject assignmentObject = new Gson().fromJson(error.response().errorBody().string(), AssignmentObject.class);
                            //Toast.makeText(getActivity(), assignmentObject.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.fabAssignmentReview:
                openActivity(AssignmentAddActivity.class);
                break;

        }
    }

    public class setAssignmentTask extends AsyncTask<Void, Void, AssignmentObject> {

        AssignmentObject assignmentObject;
        AssignmentAsyncResponse delegate = null;

        public setAssignmentTask(AssignmentAsyncResponse delegate, AssignmentObject assignmentObject) {
            this.assignmentObject = assignmentObject;
            this.delegate = delegate;
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected AssignmentObject doInBackground(Void... params) {
            return assignmentObject;
        }

        @Override
        protected void onPostExecute(AssignmentObject assignmentObject) {
            delegate.getAssignmentObject(assignmentObject);
        }
    }

    public interface AssignmentAsyncResponse {
        AssignmentObject getAssignmentObject(AssignmentObject assignmentObject);
    }

   /* @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                getActivity().finish();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }*/
}

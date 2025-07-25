package com.ibl.apps.Fragment;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.SearchView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.GeneralDiscussionsListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.DiscussionManagement.DiscussionRepository;
import com.ibl.apps.Interface.BackInterface;
import com.ibl.apps.Model.DiscssionsAllTopics;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.GeneralDiscussionsAddActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentDiscussionsLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.util.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.util.PrefUtils;

import java.lang.reflect.Field;
import java.util.ArrayList;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public class GeneralDiscussionsFragment extends BaseFragment implements View.OnClickListener {

    FragmentDiscussionsLayoutBinding fragmentDiscussionsLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    String GradeId = "0", CourseName;
    //AssignmentListAdapter libraryListAdapter;
    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    String ActivityFlag = "";
    String LessonID = "";
    String QuizID = "";
    private RecyclerViewLoadMoreScroll scrollListener;
    List<DiscssionsAllTopics.Data> discussionAllData = new ArrayList<>();
    boolean isLoad = true;
    int pageNumber = 1;
    String perpagerecord = "10";
    String userRoleName = "";
    String discussion_authorized = "";
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";

    GeneralDiscussionsListAdapter generalDiscussionsListAdapter;
    BackInterface backInterface;
    private DiscussionRepository discussionRepository;


    public GeneralDiscussionsFragment() {
        // Required empty public constructor
    }

    public static GeneralDiscussionsFragment newInstance(String param1, String param2) {
        GeneralDiscussionsFragment fragment = new GeneralDiscussionsFragment();
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
        fragmentDiscussionsLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.fragment_discussions_layout, container, false);
        return fragmentDiscussionsLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        discussionRepository = new DiscussionRepository();
        Bundle bundle = this.getArguments();
        if (bundle != null) {
            GradeId = bundle.getString(Const.GradeID, "");
            CourseName = bundle.getString(Const.CourseName, "");
            ActivityFlag = bundle.getString(Const.ActivityFlag, "");
            LessonID = bundle.getString(Const.LessonID, "");
            QuizID = bundle.getString(Const.QuizID, "");
            AddtionalLibrary = bundle.getString(Const.AddtionalLibrary, "");
            AddtionalAssignment = bundle.getString(Const.AddtionalAssignment, "");
            AddtionalDiscussions = bundle.getString(Const.AddtionalDiscussions, "");
        }

        backInterface = (BackInterface) getActivity();
        setToolbar(fragmentDiscussionsLayoutBinding.toolBar);
        //showBackArrow(getString(R.string.item_4));
        fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.VISIBLE);

        fragmentDiscussionsLayoutBinding.toolBar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                backInterface.backfragment();
            }
        });

        fragmentDiscussionsLayoutBinding.serachDisccusionIcon.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                fragmentDiscussionsLayoutBinding.discussionText.setVisibility(View.GONE);
                fragmentDiscussionsLayoutBinding.serachDisccusionIcon.setVisibility(View.GONE);
                fragmentDiscussionsLayoutBinding.disccusionsearchview.setVisibility(View.VISIBLE);
                fragmentDiscussionsLayoutBinding.disccusionsearchview.onActionViewExpanded();


                try {
                    Field mDrawable = SearchView.class.getDeclaredField("mSearchHintIcon");
                    mDrawable.setAccessible(true);
                    Drawable drawable = (Drawable) mDrawable.get(fragmentDiscussionsLayoutBinding.disccusionsearchview);
                    drawable.setBounds(0, 0, 0, 0);
                } catch (Exception e) {
                    e.printStackTrace();
                }
            }
        });

        fragmentDiscussionsLayoutBinding.btnbackDiscussion.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (fragmentDiscussionsLayoutBinding.disccusionsearchview.getVisibility() == View.VISIBLE) {
                    fragmentDiscussionsLayoutBinding.serachDisccusionIcon.setVisibility(View.VISIBLE);
                    fragmentDiscussionsLayoutBinding.discussionText.setVisibility(View.VISIBLE);
                    fragmentDiscussionsLayoutBinding.disccusionsearchview.setVisibility(View.GONE);
                } else {
                    backInterface.backfragment();
                }
            }
        });

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                    discussion_authorized = userDetailsObject.getIs_discussion_authorized();
                    if (userDetails.getRoleName() != null) {
                        userRoleName = userDetails.getRoleName().get(0);
                    }

                    fragmentDiscussionsLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                    LinearLayoutManager linearLayoutManager = new LinearLayoutManager(getActivity());
                    linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
                    fragmentDiscussionsLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(linearLayoutManager);

                    generalDiscussionsListAdapter = new GeneralDiscussionsListAdapter(getActivity(),
                            discussionAllData,
                            userDetailsObject,
                            GradeId,
                            CourseName,
                            ActivityFlag,
                            LessonID,
                            QuizID,
                            AddtionalLibrary,
                            AddtionalAssignment,
                            AddtionalDiscussions);
                    fragmentDiscussionsLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(generalDiscussionsListAdapter);

                    scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);
                    scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
                        @Override
                        public void onLoadMore() {
                            if (isLoad) {
                                fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                                CallApiDiscussionsList();
                            } else {
                                fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                            }
                        }
                    });
                    fragmentDiscussionsLayoutBinding.rcVerticalLayout.rcVertical.addOnScrollListener(scrollListener);

                    DiscussionsFragment();
                }
                return null;
            }
        }).execute();


        setOnClickListener();
    }

    public void DiscussionsFragment() {
        if (isNetworkAvailable(getActivity())) {
           /* if (userRoleName.equals("Student")) {
                if (discussion_authorized != null) {
                    if (discussion_authorized.equals("true")) {
                        if (isNetworkAvailable(getActivity())) {
                            showDialog(getActivity().getString(R.string.loading));
                            CallApiDiscussionsList();
                        } else {
                            showNetworkAlert(getActivity());
                            fragmentDiscussionsLayoutBinding.noAccessDiscussion.setVisibility(View.GONE);
                            fragmentDiscussionsLayoutBinding.layDiscussionList.setVisibility(View.GONE);
                            fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.GONE);
                            fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        }
                    } else {
                        fragmentDiscussionsLayoutBinding.noAccessDiscussion.setVisibility(View.VISIBLE);
                        fragmentDiscussionsLayoutBinding.layDiscussionList.setVisibility(View.GONE);
                        fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.GONE);
                        fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                    }
                } else {
                    fragmentDiscussionsLayoutBinding.noAccessDiscussion.setVisibility(View.VISIBLE);
                    fragmentDiscussionsLayoutBinding.layDiscussionList.setVisibility(View.GONE);
                    fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.GONE);
                    fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            } else {*/
            if (isNetworkAvailable(getActivity())) {
                showDialog(getActivity().getString(R.string.loading));
                CallApiDiscussionsList();
            } else {
                showNetworkAlert(getActivity());
                fragmentDiscussionsLayoutBinding.noAccessDiscussion.setVisibility(View.GONE);
                fragmentDiscussionsLayoutBinding.layDiscussionList.setVisibility(View.GONE);
                fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.GONE);
                fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
            }
            // }
        } else {
            showNetworkAlert(getActivity());
            fragmentDiscussionsLayoutBinding.noAccessDiscussion.setVisibility(View.GONE);
            fragmentDiscussionsLayoutBinding.layDiscussionList.setVisibility(View.GONE);
            fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.GONE);
            fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
        }

    }

    public void setOnClickListener() {
        fragmentDiscussionsLayoutBinding.addButton.setOnClickListener(this);
    }

    private void CallApiDiscussionsList() {

        fragmentDiscussionsLayoutBinding.noAccessDiscussion.setVisibility(View.GONE);
        fragmentDiscussionsLayoutBinding.layDiscussionList.setVisibility(View.VISIBLE);
        fragmentDiscussionsLayoutBinding.addButton.setVisibility(View.VISIBLE);

        disposable.add(discussionRepository.GetAllTopics(String.valueOf(pageNumber), perpagerecord, GradeId, true)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<DiscssionsAllTopics>() {
                    @Override
                    public void onSuccess(DiscssionsAllTopics discssionsAllTopics) {

                        if (discssionsAllTopics.getData() != null) {

                            if (discssionsAllTopics.getData().size() != 0) {
                                pageNumber++;
                                discussionAllData.addAll(discssionsAllTopics.getData());
                                generalDiscussionsListAdapter.notifyDataSetChanged();
                                scrollListener.setLoaded();
                            } else {
                                isLoad = false;
                            }
                        }
                        hideDialog();
                        fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        fragmentDiscussionsLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                        try {
                            HttpException error = (HttpException) e;
                            DiscssionsAllTopics discssionsAllTopics = new Gson().fromJson(error.response().errorBody().string(), DiscssionsAllTopics.class);
                            Toast.makeText(getActivity(), discssionsAllTopics.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    @SuppressLint("RestrictedApi")
    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.addButton:

                Intent i = new Intent(getContext(), GeneralDiscussionsAddActivity.class);
                i.putExtra(Const.GradeID, GradeId);
                i.putExtra(Const.CourseName, CourseName);
                i.putExtra(Const.ActivityFlag, ActivityFlag);
                i.putExtra(Const.LessonID, LessonID);
                i.putExtra(Const.QuizID, QuizID);
                i.putExtra(Const.AddtionalLibrary, AddtionalLibrary);
                i.putExtra(Const.AddtionalAssignment, AddtionalAssignment);
                i.putExtra(Const.AddtionalDiscussions, AddtionalDiscussions);

                startActivity(i);
                //getActivity().finish();
                break;
        }
    }

    /*@Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                backInterface.backfragment();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }*/
}

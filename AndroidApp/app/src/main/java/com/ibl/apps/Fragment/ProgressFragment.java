package com.ibl.apps.Fragment;


import androidx.databinding.DataBindingUtil;
import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.fragment.app.Fragment;
import androidx.recyclerview.widget.LinearLayoutManager;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.ibl.apps.Adapter.QueueListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.FeedbackManagement.FeedbackApiService;
import com.ibl.apps.FeedbackManagement.FeedbackRepository;
import com.ibl.apps.Model.feedback.FeebBackTask;
import com.ibl.apps.Network.ApiClient;
import com.ibl.apps.Network.ApiService;
import com.ibl.apps.util.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.util.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentProgressBinding;

import java.util.ArrayList;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;


/**
 * A simple {@link Fragment} subclass.
 */
public class ProgressFragment extends BaseFragment {


    private FragmentProgressBinding binding;
    CompositeDisposable disposable = new CompositeDisposable();
    private ApiService apiService;
    int pagenumber = 1;
    int perpagerecord = 10;
    private RecyclerViewLoadMoreScroll scrollListener;
    private boolean isLoad = true;
    private List<FeebBackTask.Data> feebBackTaskdata = new ArrayList<>();
    private FeedbackRepository feedbackRepository;

    public ProgressFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        binding = DataBindingUtil.inflate(inflater, R.layout.fragment_progress, container, false);
        return binding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        feedbackRepository = new FeedbackRepository();
        if (feebBackTaskdata != null && feebBackTaskdata.size() != 0) {
            feebBackTaskdata.clear();
            pagenumber = 1;
        }
        callApiForprogressList();

        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(NoonApplication.getContext());
        linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        binding.rvInprogress.rcVertical.setLayoutManager(linearLayoutManager);
        scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);
        scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
            @Override
            public void onLoadMore() {
                if (isLoad) {
                    binding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    callApiForprogressList();
                } else {
                    binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            }
        });

        binding.rvInprogress.rcVertical.addOnScrollListener(scrollListener);
    }

    private void callApiForprogressList() {
        disposable.add(feedbackRepository.getProcessListApp(pagenumber, perpagerecord)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<FeebBackTask>() {
                    @Override
                    public void onSuccess(FeebBackTask feebBackTask) {
                        if (feebBackTask != null) {
                            if (feebBackTask.getData() != null && !feebBackTask.getData().isEmpty()) {
                                binding.rvInprogress.rcVertical.setVisibility(View.VISIBLE);
                                binding.txtNofeedback.setVisibility(View.GONE);
//                                pagenumber = 1;
                                pagenumber++;
                                feebBackTaskdata.addAll(feebBackTask.getData());
                                QueueListAdapter adapter = new QueueListAdapter(getActivity(), feebBackTaskdata);
                                binding.rvInprogress.rcVertical.setAdapter(adapter);
                                scrollListener.setLoaded();
                                hideDialog();
                                if (feebBackTask.getData().size() == 0) {
                                    isLoad = false;
                                }
                                binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);

                            } else {
                                hideDialog();
                                binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                                if (feebBackTaskdata.size() == 0) {
                                    binding.rvInprogress.rcVertical.setVisibility(View.GONE);
                                    binding.txtNofeedback.setVisibility(View.VISIBLE);
                                }
                            }
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        e.printStackTrace();
                        Log.e("FeedBack||onError", "onError: " + e.getMessage());
                    }
                }));

    }
}

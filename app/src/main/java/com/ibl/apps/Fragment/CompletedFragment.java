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
import com.ibl.apps.FeedbackManagement.FeedbackRepository;
import com.ibl.apps.Model.feedback.FeebBackTask;
import com.ibl.apps.util.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.util.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.FragmentCompletedBinding;

import java.util.ArrayList;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;


/**
 * A simple {@link Fragment} subclass.
 */
public class CompletedFragment extends BaseFragment {


    private FragmentCompletedBinding binding;
    CompositeDisposable disposable = new CompositeDisposable();
    int pagenumber = 1;
    int perpagerecord = 10;
    private RecyclerViewLoadMoreScroll scrollListener;
    private boolean isLoad = true;
    private List<FeebBackTask.Data> feebBackTaskdata = new ArrayList<>();
    private FeedbackRepository feedbackRepository;

    public CompletedFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        binding = DataBindingUtil.inflate(inflater, R.layout.fragment_completed, container, false);
        return binding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        feedbackRepository = new FeedbackRepository();

        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(NoonApplication.getContext());
        linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        binding.rvCompleted.rcVertical.setLayoutManager(linearLayoutManager);
        scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);

        if (feebBackTaskdata != null && feebBackTaskdata.size() != 0) {
            feebBackTaskdata.clear();
            pagenumber = 1;
        }
        callApiForCompletedList();
        scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
            @Override
            public void onLoadMore() {
                if (isLoad) {
                    binding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    callApiForCompletedList();
                } else {
                    binding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            }
        });

        binding.rvCompleted.rcVertical.addOnScrollListener(scrollListener);
    }

    private void callApiForCompletedList() {
        disposable.add(feedbackRepository.getCompletedListApp(pagenumber, perpagerecord)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<FeebBackTask>() {
                    @Override
                    public void onSuccess(FeebBackTask feebBackTask) {
                        if (feebBackTask != null) {
                            if (feebBackTask.getData() != null && !feebBackTask.getData().isEmpty()) {
                                binding.rvCompleted.rcVertical.setVisibility(View.VISIBLE);
                                binding.txtNofeedback.setVisibility(View.GONE);
                                pagenumber++;
                                feebBackTaskdata.addAll(feebBackTask.getData());
                                QueueListAdapter adapter = new QueueListAdapter(getActivity(), feebBackTaskdata);
                                binding.rvCompleted.rcVertical.setAdapter(adapter);
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
                                    binding.rvCompleted.rcVertical.setVisibility(View.GONE);
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

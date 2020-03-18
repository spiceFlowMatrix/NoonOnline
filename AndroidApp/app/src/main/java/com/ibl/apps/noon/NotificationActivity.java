package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.view.MenuItem;
import android.view.View;
import android.widget.Toast;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.NotificationListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.NotificationObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.LoadMoreData.OnLoadMoreListener;
import com.ibl.apps.Utils.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.noon.databinding.NotificationLayoutBinding;

import java.util.ArrayList;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;


public class NotificationActivity extends BaseActivity implements View.OnClickListener {

    NotificationLayoutBinding notificationLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    int pageNumber = 1;
    String perpagerecord = "10";
    private RecyclerViewLoadMoreScroll scrollListener;
    List<NotificationObject.Data> notificationList = new ArrayList<>();
    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    boolean isLoad = true;
    NotificationListAdapter notificationListAdapter;

    @Override
    protected int getContentView() {
        return R.layout.notification_layout;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        notificationLayoutBinding = (NotificationLayoutBinding) getBindObj();
        setToolbar(notificationLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.notification_label));

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();
                    showDialog(getString(R.string.loading));
                    callApiNotificationList();
                }
                return null;
            }

        }).execute();

        notificationLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
        LinearLayoutManager linearLayoutManager = new LinearLayoutManager(this);
        linearLayoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        notificationLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(linearLayoutManager);
        notificationListAdapter = new NotificationListAdapter(this, notificationList);
        notificationLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(notificationListAdapter);

        scrollListener = new RecyclerViewLoadMoreScroll(linearLayoutManager);
        scrollListener.setOnLoadMoreListener(new OnLoadMoreListener() {
            @Override
            public void onLoadMore() {
                if (isLoad) {
                    notificationLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    callApiNotificationList();
                } else {
                    notificationLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            }
        });
        notificationLayoutBinding.rcVerticalLayout.rcVertical.addOnScrollListener(scrollListener);

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

    public void callApiNotificationList() {

        disposable.add(apiService.fetchNotification(String.valueOf(pageNumber), perpagerecord)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<NotificationObject>() {

                    @Override
                    public void onSuccess(NotificationObject notificationObject) {

                        if (notificationObject.getData() != null) {
                            if (notificationObject.getData().size() != 0) {
                                pageNumber++;
                                notificationList.addAll(notificationObject.getData());
                                notificationListAdapter.notifyDataSetChanged();
                                scrollListener.setLoaded();
                            } else {
                                isLoad = false;
                            }
                        }
                        hideDialog();
                        notificationLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            NotificationObject notificationObject = new Gson().fromJson(error.response().errorBody().string(), NotificationObject.class);
                            Toast.makeText(getApplicationContext(), notificationObject.getMessage(), Toast.LENGTH_LONG).show();
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        disposable.dispose();
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                onBackPressed();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    public void onBackPressed() {
        super.onBackPressed();
        finish();
    }


}

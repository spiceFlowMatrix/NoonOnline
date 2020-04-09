package com.ibl.apps.Adapter;

import android.content.Context;
import android.os.AsyncTask;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.ibl.apps.Model.QuizMainObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.QuizAnswerSelect;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AnswerItemLayoutBinding;
import com.ibl.apps.noon.databinding.DeviceLoginDetailListBinding;

import java.util.ArrayList;
import java.util.Collections;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AllDeviceListAdapter extends RecyclerView.Adapter<AllDeviceListAdapter.MyViewHolder> {

    Context context;

    public AllDeviceListAdapter(Context context) {
        this.context = context;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DeviceLoginDetailListBinding binding;

        public MyViewHolder(DeviceLoginDetailListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }


    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        DeviceLoginDetailListBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.device_login_detail_list, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {

    }

    @Override
    public int getItemCount() {
        return 5;
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }
}
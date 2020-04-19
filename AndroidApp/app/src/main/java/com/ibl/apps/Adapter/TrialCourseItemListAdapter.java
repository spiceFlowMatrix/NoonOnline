package com.ibl.apps.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.TrialCourseItemLayoutBinding;
import com.ibl.apps.util.WrapContentLinearLayoutManager;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class TrialCourseItemListAdapter extends RecyclerView.Adapter<TrialCourseItemListAdapter.MyViewHolder> {
    Context context;

    public TrialCourseItemListAdapter(Context context) {
        this.context = context;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        public TrialCourseItemLayoutBinding courseItemLayoutBinding;

        public MyViewHolder(TrialCourseItemLayoutBinding itemhistorylayoutBinding) {
            super(itemhistorylayoutBinding.getRoot());
            this.courseItemLayoutBinding = itemhistorylayoutBinding;
            courseItemLayoutBinding.executePendingBindings();
        }
    }


    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        TrialCourseItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.trial_course_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
        holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new WrapContentLinearLayoutManager(NoonApplication.getContext(), LinearLayoutManager.VERTICAL, false));
        TrialCourseItemInnerListAdapter adp = new TrialCourseItemInnerListAdapter(context);
        holder.courseItemLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(adp);
    }


    @Override
    public int getItemCount() {
        return 3;
    }


    @Override
    public int getItemViewType(int position) {
        return position;
    }

}
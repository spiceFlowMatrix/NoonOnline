package com.ibl.apps.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.TrialCourseInnerItemLayoutBinding;
import com.ibl.apps.noon.databinding.TrialCourseItemLayoutBinding;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class TrialCourseItemInnerListAdapter extends RecyclerView.Adapter<TrialCourseItemInnerListAdapter.MyViewHolder> {
    Context context;

    public TrialCourseItemInnerListAdapter(Context context) {
        this.context = context;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        public TrialCourseInnerItemLayoutBinding courseItemLayoutBinding;

        public MyViewHolder(TrialCourseInnerItemLayoutBinding itemhistorylayoutBinding) {
            super(itemhistorylayoutBinding.getRoot());
            this.courseItemLayoutBinding = itemhistorylayoutBinding;
            courseItemLayoutBinding.executePendingBindings();
        }
    }


    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        TrialCourseInnerItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.trial_course_inner_item_layout, parent, false);
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
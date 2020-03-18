package com.ibl.apps.Adapter;

import android.content.Context;
import android.databinding.DataBindingUtil;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.ibl.apps.Model.APICourseObject;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourseAllocationLayoutBinding;

import java.util.ArrayList;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class CourseAllocateListAdapter extends RecyclerView.Adapter<CourseAllocateListAdapter.MyViewHolder> {
    ArrayList<APICourseObject.Data> list;
    Context ctx;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        CourseAllocationLayoutBinding courseAllocationLayoutBinding;

        public MyViewHolder(CourseAllocationLayoutBinding courseAllocationLayoutBinding) {
            super(courseAllocationLayoutBinding.getRoot());
            this.courseAllocationLayoutBinding = courseAllocationLayoutBinding;
        }
    }

    public CourseAllocateListAdapter(Context ctx, ArrayList<APICourseObject.Data> list) {
        this.list = list;
        this.ctx = ctx;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        CourseAllocationLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.course_allocation_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        APICourseObject.Data model = list.get(position);

        holder.courseAllocationLayoutBinding.txtCourseName.setText(model.getName());
        holder.courseAllocationLayoutBinding.txtCourseDescription.setText(model.getDescription());

        GlideApp.with(ctx)
                .load(model.getImage())
                .error(R.drawable.ic_no_image_found)
                .into(holder.courseAllocationLayoutBinding.imgCourse);

    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }
}
package com.ibl.apps.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.ibl.apps.Model.CourseObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CourseLayoutBinding;
import com.ibl.apps.noon.databinding.TrialCourseLayoutBinding;

import java.util.ArrayList;
import java.util.List;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class TrialCourseListAdapter extends RecyclerView.Adapter<TrialCourseListAdapter.MyViewHolder> {
    List<CourseObject.Data> list;
    Context ctx;
    UserDetails userDetailsObject;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        TrialCourseLayoutBinding courseLayoutBinding;

        public MyViewHolder(TrialCourseLayoutBinding itemhistorylayoutBinding) {
            super(itemhistorylayoutBinding.getRoot());
            this.courseLayoutBinding = itemhistorylayoutBinding;
        }
    }

    /* public TrialCourseListAdapter(Context ctx, List<CourseObject.Data> list, UserDetails userDetailsObject) {
         this.list = list;
         this.ctx = ctx;
         this.userDetailsObject = userDetailsObject;
     }*/
    public TrialCourseListAdapter(Context ctx) {
        this.ctx = ctx;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        TrialCourseLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.trial_course_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
       /* CourseObject.Data model = list.get(position);
        holder.courseLayoutBinding.txtgradeName.setText(model.getName());

        if (model.getCourses().size() <= 4) {
            holder.courseLayoutBinding.expandCourse.setVisibility(View.GONE);
        } else {
            holder.courseLayoutBinding.expandCourse.setVisibility(View.VISIBLE);
        }

        if (position == getItemCount() - 1) {
            holder.courseLayoutBinding.coursedivider.setVisibility(View.VISIBLE);
        } else {
            holder.courseLayoutBinding.coursedivider.setVisibility(View.VISIBLE);
        }

        holder.courseLayoutBinding.expandCourse.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                holder.courseLayoutBinding.expandCourse.setVisibility(View.GONE);
                loadData(holder, model, 1);
            }
        });
        loadData(holder, model, 0);*/
        holder.courseLayoutBinding.rcHorizontalLayout.rcVertical.setLayoutManager(new GridLayoutManager(ctx, 2));
        holder.courseLayoutBinding.rcHorizontalLayout.rcVertical.setHasFixedSize(true);
        TrialCourseInnerListAdapter adp = new TrialCourseInnerListAdapter(ctx);
        holder.courseLayoutBinding.rcHorizontalLayout.rcVertical.setAdapter(adp);
    }

    private void loadData(MyViewHolder holder, CourseObject.Data model, int expandFlag) {
        List<CourseObject.Courses> oldCourseInnerList = new ArrayList<>();
        oldCourseInnerList = model.getCourses();

        List<CourseObject.Courses> CourseInnerList = new ArrayList<>();

        if (expandFlag == 0) {
            for (int i = 0; i < oldCourseInnerList.size(); i++) {
                if (i <= 3) {
                    CourseInnerList.add(oldCourseInnerList.get(i));
                }
            }
        } else {
            CourseInnerList = oldCourseInnerList;
        }

        holder.courseLayoutBinding.rcHorizontalLayout.rcVertical.setLayoutManager(new GridLayoutManager(ctx, 2));
        holder.courseLayoutBinding.rcHorizontalLayout.rcVertical.setHasFixedSize(true);
        CourseInnerListAdapter adp = new CourseInnerListAdapter(ctx, CourseInnerList, userDetailsObject);
        holder.courseLayoutBinding.rcHorizontalLayout.rcVertical.setAdapter(adp);
    }

    @Override
    public int getItemCount() {
       /* if (list != null && list.size() != 0) {
            return list.size();
        }*/
        return 1;
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }
}

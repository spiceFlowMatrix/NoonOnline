package com.ibl.apps.Adapter;

import android.content.Context;
import android.content.DialogInterface;
import androidx.databinding.DataBindingUtil;
import android.graphics.Typeface;
import androidx.core.content.res.ResourcesCompat;
import androidx.recyclerview.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.assignment.StudentDetailData;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AssignmentItemLayoutBinding;

import java.util.ArrayList;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class StudentListAdapter extends RecyclerView.Adapter<StudentListAdapter.MyViewHolder> implements DroidListener {

    ArrayList<StudentDetailData> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;

    private OnItemClicked onClick;

    public interface OnItemClicked {
        void onItemClick(int position);
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        AssignmentItemLayoutBinding assignmentItemLayoutBinding;

        public MyViewHolder(AssignmentItemLayoutBinding assignmentItemLayoutBinding) {
            super(assignmentItemLayoutBinding.getRoot());
            this.assignmentItemLayoutBinding = assignmentItemLayoutBinding;
        }
    }

    public StudentListAdapter(Context ctx, ArrayList<StudentDetailData> list, UserDetails userDetailsObject, OnItemClicked onClick) {
        this.list = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
        this.onClick = onClick;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        AssignmentItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.assignment_item_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        StudentDetailData model = list.get(position);
        if (!TextUtils.isEmpty(model.getName())) {
            holder.assignmentItemLayoutBinding.txtAssigignmentName.setText(model.getName());
        } else {
            holder.assignmentItemLayoutBinding.txtAssigignmentName.setText(ctx.getResources().getString(R.string.user));
        }

        holder.assignmentItemLayoutBinding.txtSubmitionCount.setText(model.getSubmissions() + " " + ctx.getString(R.string.submitionE));

        if (model.getStatus() == 0) {
            holder.assignmentItemLayoutBinding.imgassignmentComplite.setVisibility(View.VISIBLE);
            holder.assignmentItemLayoutBinding.imgassignmentInComplite.setVisibility(View.GONE);
            holder.assignmentItemLayoutBinding.imgassignmentNoStatus.setVisibility(View.GONE);
        } else if (model.getStatus() == 1) {
            holder.assignmentItemLayoutBinding.imgassignmentComplite.setVisibility(View.GONE);
            holder.assignmentItemLayoutBinding.imgassignmentInComplite.setVisibility(View.VISIBLE);
            holder.assignmentItemLayoutBinding.imgassignmentNoStatus.setVisibility(View.GONE);
        } else if (model.getStatus() == 2) {
            holder.assignmentItemLayoutBinding.imgassignmentComplite.setVisibility(View.GONE);
            holder.assignmentItemLayoutBinding.imgassignmentInComplite.setVisibility(View.GONE);
            holder.assignmentItemLayoutBinding.imgassignmentNoStatus.setVisibility(View.VISIBLE);

        } else if (model.getStatus() == 3) {
            holder.assignmentItemLayoutBinding.imgassignmentComplite.setVisibility(View.GONE);
            holder.assignmentItemLayoutBinding.imgassignmentInComplite.setVisibility(View.GONE);
            holder.assignmentItemLayoutBinding.imgassignmentNoStatus.setVisibility(View.GONE);
        }
        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onClick.onItemClick(position);
            }
        });
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    public static boolean isNetworkAvailable(Context context) {
        return isNetworkConnected;
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        if (isConnected) {
            isNetworkConnected = true;
        } else {
            isNetworkConnected = false;
        }
    }

    public static SpannableStringBuilder setTypeface(Context context, String message) {
        Typeface typeface = ResourcesCompat.getFont(context, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }

    public static void showNetworkAlert(Context activity) {
        try {
            SpannableStringBuilder message = setTypeface(activity, activity.getResources().getString(R.string.validation_Connect_internet));
            androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                        }
                    });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
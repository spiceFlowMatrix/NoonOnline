package com.ibl.apps.Adapter;

import android.content.Context;
import android.databinding.DataBindingUtil;
import android.graphics.Typeface;
import android.support.annotation.NonNull;
import android.support.v4.content.res.ResourcesCompat;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Interface.ViewFiles;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.Model.assignment.SubmissionFiles;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Utils.CustomTypefaceSpan;
import com.ibl.apps.Utils.GlideApp;
import com.ibl.apps.Utils.TimeAgoClass;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.AssignmentCommentLayoutBinding;

import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AssignmentCommentListAdapter extends RecyclerView.Adapter<AssignmentCommentListAdapter.MyViewHolder> implements DroidListener, ViewFiles {

    ArrayList<AssignmentData> list;
    Context ctx;
    UserDetails userDetailsObject;
    DroidNet mDroidNet;
    public static boolean isNetworkConnected = false;
    private ViewFiles viewFilesinterface;

    @Override
    public void openDialogView(SubmissionFiles submissionFiles, int filetypeid) {
        viewFilesinterface.openDialogView(submissionFiles, filetypeid);
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        AssignmentCommentLayoutBinding assignmentCommentLayoutBinding;

        public MyViewHolder(AssignmentCommentLayoutBinding assignmentCommentLayoutBinding) {
            super(assignmentCommentLayoutBinding.getRoot());
            this.assignmentCommentLayoutBinding = assignmentCommentLayoutBinding;
        }
    }

    public AssignmentCommentListAdapter(Context ctx, UserDetails userDetailsObject, ViewFiles viewFilesinterface) {
        this.viewFilesinterface = viewFilesinterface;
        this.list = new ArrayList<>();
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        AssignmentCommentLayoutBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.assignment_comment_layout, parent, false);
        return new MyViewHolder(itemViewBinding);

    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        AssignmentData model = list.get(position);

        if (model.getIssubmission()) {
            holder.assignmentCommentLayoutBinding.txtcommentstatic.setText(ctx.getResources().getString(R.string.added_submission));
        } else if (model.getIsapproved()) {
            holder.assignmentCommentLayoutBinding.txtcommentstatic.setText(ctx.getResources().getString(R.string.paas_assignment));
        } else {
            holder.assignmentCommentLayoutBinding.txtcommentstatic.setText(ctx.getResources().getString(R.string.commentedE));
        }


        if (model.getUser() != null) {
            if (!TextUtils.isEmpty(model.getUser().getProfilepicurl())) {
                GlideApp.with(NoonApplication.getContext())
                        .load(model.getUser().getProfilepicurl())
                        .placeholder(R.drawable.personprofile)
                        .error(R.drawable.personprofile)
                        .into(holder.assignmentCommentLayoutBinding.commentprofileImage);
            }
            if (!TextUtils.isEmpty(model.getUser().getFullname())) {
                holder.assignmentCommentLayoutBinding.txtcommentUserName.setText(model.getUser().getFullname());
            } else if (!TextUtils.isEmpty(model.getUser().getUsername())) {
                holder.assignmentCommentLayoutBinding.txtcommentUserName.setText(model.getUser().getUsername());
            } else {
                holder.assignmentCommentLayoutBinding.txtcommentUserName.setText(ctx.getResources().getString(R.string.user));
            }
        }


        if (!TextUtils.isEmpty(model.getComment())) {
            holder.assignmentCommentLayoutBinding.rcVerticalLay.rcVertical.setVisibility(View.GONE);
            holder.assignmentCommentLayoutBinding.txtcomment.setText(model.getComment());
            holder.assignmentCommentLayoutBinding.txtcomment.setVisibility(View.VISIBLE);
        } else if (!TextUtils.isEmpty(model.getRemark())) {
            holder.assignmentCommentLayoutBinding.txtcomment.setVisibility(View.VISIBLE);
            holder.assignmentCommentLayoutBinding.txtcomment.setText(model.getRemark());
        } else if (model.getSubmissionfiles() != null) {
            holder.assignmentCommentLayoutBinding.txtcomment.setVisibility(View.GONE);
            holder.assignmentCommentLayoutBinding.rcVerticalLay.rcVertical.setVisibility(View.VISIBLE);

            if (model.getSubmissionfiles().size() != 0) {
                holder.assignmentCommentLayoutBinding.rcVerticalLay.rcVertical.setLayoutManager(new LinearLayoutManager(ctx, LinearLayoutManager.HORIZONTAL, false));
                AssignmentAddCommentFileListAdapter adp = new AssignmentAddCommentFileListAdapter(ctx, model.getSubmissionfiles(), userDetailsObject, this);
                holder.assignmentCommentLayoutBinding.rcVerticalLay.rcVertical.setAdapter(adp);
                adp.notifyDataSetChanged();
            }
        }

        if (!TextUtils.isEmpty(model.getDatecreated())) {
            TimeAgoClass timeAgo = new TimeAgoClass();
            String getDate = getDate(model.getDatecreated());
            String timeagotxt = timeAgo.covertTimeToText(getDate);
            holder.assignmentCommentLayoutBinding.txtcommenttime.setText(timeagotxt);
        }

    }


    public void setCards(ArrayList<AssignmentData> data) {
        if (list == null) {
            list = new ArrayList<>();
        } else {
            list.clear();
        }
        list.addAll(data);
        notifyDataSetChanged();
    }

    private String getDate(String ourDate) {
        try {

            SimpleDateFormat formatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss aaa", Locale.ENGLISH);
            formatter.setTimeZone(TimeZone.getTimeZone("UTC"));
            Date value = formatter.parse(ourDate);

            SimpleDateFormat dateFormatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH); //this format changeable
            dateFormatter.setTimeZone(TimeZone.getDefault());
            ourDate = dateFormatter.format(value);

            //Log.d("ourDate", ourDate);
        } catch (Exception e) {
            ourDate = "00-00-0000 00:00";
        }
        return ourDate;
    }

    @Override
    public int getItemCount() {
        return list.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    public static boolean isNetworkAvailable() {
        return isNetworkConnected;
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        isNetworkConnected = isConnected;
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
            android.support.v7.app.AlertDialog.Builder builder = new android.support.v7.app.AlertDialog.Builder(activity);
            builder.setTitle(activity.getResources().getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(activity.getResources().getString(R.string.button_ok), (dialog, which) -> {
                        // Yes button clicked, do something
                        dialog.dismiss();
                    });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
package com.ibl.apps.Adapter;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.databinding.DataBindingUtil;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Model.feedback.FeebBackTask;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.TimeAgoClass;
import com.ibl.apps.noon.FilesFeedbackActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.ReportProblemActivity;
import com.ibl.apps.noon.databinding.QueueListItemBinding;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

public class QueueListAdapter extends RecyclerView.Adapter<QueueListAdapter.QueueViewHolder> {
    Context context;
    private List<FeebBackTask.Data> feebBackTask;

    public QueueListAdapter(Context context, List<FeebBackTask.Data> feebBackTask) {
        this.context = context;
        this.feebBackTask = feebBackTask;
    }

    @NonNull
    @Override
    public QueueListAdapter.QueueViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        QueueListItemBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.queue_list_item, parent, false);

        context = parent.getContext();
        return new QueueListAdapter.QueueViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull QueueViewHolder holder, int i) {
        FeebBackTask.Data model = feebBackTask.get(i);
        if (!TextUtils.isEmpty(model.getTitle())) {
            holder.binding.tvTitle.setText(model.getTitle());
        }

        if (!TextUtils.isEmpty(model.getDescription())) {
            holder.binding.tvDiscription.setText(model.getDescription());
        }

        if (!TextUtils.isEmpty(model.getSubmitteddate())) {
//            10/18/2019 10:00:24 AM
//            10/19/2019 05:31:51
            TimeAgoClass timeAgo = new TimeAgoClass();
            String getDate = getDate(model.getSubmitteddate());
            String timeagotxt = timeAgo.covertTimeToText(getDate);
            holder.binding.tvTime.setText(timeagotxt);
        }


        holder.itemView.setOnClickListener(view -> {
            if (BaseFragment.isNetworkAvailable(context)) {
                Intent reportIntent = null;

                if (model.getCategory().getId() != null) {
                    if (model.getCategory().getId() == 1 || model.getCategory().getId() == 7) {
                        reportIntent = new Intent(context, ReportProblemActivity.class);
                    } else if (model.getCategory().getId() == 2 || model.getCategory().getId() == 3 || model.getCategory().getId() == 4 || model.getCategory().getId() == 5 || model.getCategory().getId() == 6) {
                        reportIntent = new Intent(context, FilesFeedbackActivity.class);
                    }
                }
                if (reportIntent != null) {
                    reportIntent.putExtra(Const.Flag, Integer.parseInt(String.valueOf(model.getCategory().getId())));
//                reportIntent.putExtra("description", holder.binding.tvDiscription.getText().toString().trim());
                    reportIntent.putExtra("id", model.getId());
                    context.startActivity(reportIntent);
                    ((Activity) context).finish();
                }
            } else {
                BaseFragment.showNetworkAlert(context);
            }
        });

//        holder.itemView.setOnClickListener(view -> {
//            Intent reportIntent = null;
//
//            if (model.getCategory().getId() != null) {
//                if (model.getCategory().getId() == 1 || model.getCategory().getId() == 7) {
//                    reportIntent = new Intent(context, ReportProblemActivity.class);
//                } else if (model.getCategory().getId() == 2 || model.getCategory().getId() == 3 || model.getCategory().getId() == 4 || model.getCategory().getId() == 5 || model.getCategory().getId() == 6) {
//                    reportIntent = new Intent(context, FilesFeedbackActivity.class);
//                }
//            }
//            if (reportIntent != null) {
//                reportIntent.putExtra(Const.Flag, Integer.parseInt(String.valueOf(model.getCategory().getId())));
//                reportIntent.putExtra("description", holder.binding.tvDiscription.getText().toString().trim());
//                context.startActivity(reportIntent);
//                ((Activity) context).finish();
//            }
//        });

    }

    private String getDate(String ourDate) {
        try {
            SimpleDateFormat formatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss aaa", Locale.ENGLISH);
            formatter.setTimeZone(TimeZone.getTimeZone("UTC"));
            Date value = formatter.parse(ourDate);

            SimpleDateFormat dateFormatter = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH); //this format changeable
            dateFormatter.setTimeZone(TimeZone.getDefault());
            ourDate = dateFormatter.format(value);

        } catch (Exception e) {
            ourDate = "00-00-0000 00:00";
        }
        return ourDate;
    }

    @Override
    public int getItemCount() {
        return feebBackTask.size();
    }

    class QueueViewHolder extends RecyclerView.ViewHolder {
        QueueListItemBinding binding;

        QueueViewHolder(QueueListItemBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}

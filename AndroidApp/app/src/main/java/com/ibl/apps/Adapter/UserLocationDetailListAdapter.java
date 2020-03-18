package com.ibl.apps.Adapter;

import android.content.Context;
import android.databinding.DataBindingUtil;
import android.support.annotation.NonNull;
import android.support.v7.widget.RecyclerView;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.ibl.apps.Model.parent.ParentGraphPoints;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.UserLocationDetailBinding;

import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Locale;
import java.util.TimeZone;

public class UserLocationDetailListAdapter extends RecyclerView.Adapter<UserLocationDetailListAdapter.ViewHolder> {
    Context context;
    private List<ParentGraphPoints.Data> parentDataPointsList;

    public UserLocationDetailListAdapter(Context context, List<ParentGraphPoints.Data> parentDataPointsList) {
        this.context = context;
        this.parentDataPointsList = parentDataPointsList;
    }

    @NonNull
    @Override
    public UserLocationDetailListAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        UserLocationDetailBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.user_location_detail, parent, false);

        //context = parent.getContext();
        return new UserLocationDetailListAdapter.ViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int i) {
        ParentGraphPoints.Data model = parentDataPointsList.get(i);

        if (model.getUserDetails() != null) {
            if (!TextUtils.isEmpty(model.getUserDetails().getUsername())) {
                holder.binding.tvgetNamet.setText(context.getResources().getString(R.string.user_name) + model.getUserDetails().getUsername());
            } else if (!TextUtils.isEmpty(model.getUserDetails().getFullName())) {
                holder.binding.tvgetNamet.setText(context.getResources().getString(R.string.user_name) + model.getUserDetails().getFullName());
            } else if (!TextUtils.isEmpty(model.getUserDetails().getEmail())) {
                String[] separated = model.getUserDetails().getEmail().split("@");
                holder.binding.tvgetNamet.setText(context.getResources().getString(R.string.user_name) + separated[0]);
            } else {
                holder.binding.tvgetNamet.setText(context.getResources().getString(R.string.user_name) + context.getResources().getString(R.string.user));
            }
        }

//        if (model.getHours() != null) {
//            NumberFormat nm = NumberFormat.getNumberInstance();
//            holder.binding.tvDate.setText(nm.format(model.getHours()));
//        }
        if (model.getHours() != null) {
            DecimalFormat REAL_FORMATTER = new DecimalFormat("0.###");
            holder.binding.tvDate.setText(context.getResources().getString(R.string.spent_hour) + REAL_FORMATTER.format(model.getHours()));

//            String stringdouble = Double.toString(model.getHours());
//            holder.binding.tvDate.setText(context.getResources().getString(R.string.spent_hour) + stringdouble);
        }


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
        return parentDataPointsList.size();
    }

    class ViewHolder extends RecyclerView.ViewHolder {
        UserLocationDetailBinding binding;

        ViewHolder(UserLocationDetailBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}

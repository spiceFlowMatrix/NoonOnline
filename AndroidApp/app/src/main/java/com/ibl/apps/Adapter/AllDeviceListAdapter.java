package com.ibl.apps.Adapter;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.ibl.apps.Model.deviceManagement.DevicesModel;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DeviceLoginDetailListBinding;

import java.util.List;

import static android.content.Context.MODE_PRIVATE;


public class AllDeviceListAdapter extends RecyclerView.Adapter<AllDeviceListAdapter.MyViewHolder> {

    private Context context;
    private List<DevicesModel> devicesModel;
    private BottomSheetDialog mBottomSheetDialog;
    private ProgressDialog mProgressDialog = null;
    private ShowBottomInterface showBottomInterface;

    public AllDeviceListAdapter(Context context, List<DevicesModel> devicesModel, ShowBottomInterface showBottomInterface) {
        this.context = context;
        this.devicesModel = devicesModel;
        this.showBottomInterface = showBottomInterface;
    }

    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        DeviceLoginDetailListBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.device_login_detail_list, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        DevicesModel model = devicesModel.get(position);
        SharedPreferences sharedPreferences = context.getSharedPreferences("deviceManagement", MODE_PRIVATE);
        String deviceToken = sharedPreferences.getString("deviceToken", "");

        if (deviceToken.contains(model.getDeviceToken())) {
            holder.binding.txtDeviceName.setText(context.getResources().getString(R.string.this_device));
            holder.binding.txtDeviceName.setTextColor(context.getResources().getColor(R.color.colorDarkBlue));
            holder.binding.txtDeviceStatus.setTextColor(context.getResources().getColor(R.color.colorDarkBlue));
            holder.binding.dash.setBackgroundColor(context.getResources().getColor(R.color.colorDarkBlue));
        } else {
            holder.binding.txtDeviceName.setText(model.getModelName());
            holder.binding.txtDeviceName.setTextColor(context.getResources().getColor(R.color.colorBlack));
            holder.binding.txtDeviceStatus.setTextColor(context.getResources().getColor(R.color.colorBlack));
            holder.binding.dash.setBackgroundColor(context.getResources().getColor(R.color.colorBlack));
        }

        if (model.getIsActive()) {
            holder.binding.txtDeviceStatus.setText(context.getResources().getString(R.string.device_active));
        } else {
            holder.binding.txtDeviceStatus.setText(context.getResources().getString(R.string.device_deactivated));
        }
        holder.binding.imgMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showBottomInterface.showBottomDialog(model.getId());
            }
        });
    }

    @Override
    public int getItemCount() {
        return devicesModel.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }

    public void showDialog(String message) {
        if (mProgressDialog == null) {
            mProgressDialog = new ProgressDialog(context);
            mProgressDialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            mProgressDialog.setCancelable(true);
            mProgressDialog.setCanceledOnTouchOutside(false);
        }
        mProgressDialog.setMessage(message);
        mProgressDialog.show();

    }

    protected void hideDialog() {
        if (mProgressDialog != null && mProgressDialog.isShowing()) {
            mProgressDialog.dismiss();
        }
    }

    public interface ShowBottomInterface {
        void showBottomDialog(Long id);
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DeviceLoginDetailListBinding binding;

        public MyViewHolder(DeviceLoginDetailListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }


    }
}
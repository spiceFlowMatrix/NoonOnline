package com.ibl.apps.Adapter;

import android.app.ProgressDialog;
import android.content.Context;
import android.content.SharedPreferences;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.ibl.apps.DeviceManagement.DeviceManagementRepository;
import com.ibl.apps.Model.deviceManagement.DeviceListModel;
import com.ibl.apps.Model.deviceManagement.DevicesModel;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.ActivateDeavtivateBottomSheetBinding;
import com.ibl.apps.noon.databinding.DeviceLoginDetailListBinding;

import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;

import static android.content.Context.MODE_PRIVATE;


public class AllDeviceListAdapter extends RecyclerView.Adapter<AllDeviceListAdapter.MyViewHolder> {

    private Context context;
    private List<DevicesModel> devicesModel;
    private BottomSheetDialog mBottomSheetDialog;
    private ProgressDialog mProgressDialog = null;

    public AllDeviceListAdapter(Context context, List<DevicesModel> devicesModel) {
        this.context = context;
        this.devicesModel = devicesModel;
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

        if (deviceToken.equals(model.getDeviceToken())) {
            holder.binding.txtDeviceName.setText(context.getResources().getString(R.string.this_device));
        } else if (!TextUtils.isEmpty(model.getModelName())) {
            holder.binding.txtDeviceName.setText(model.getModelName());
        }
        if (!TextUtils.isEmpty(model.getModelName()))
            holder.binding.txtDeviceName.setText(model.getModelName());

        if (model.getIsActive()) {
            holder.binding.txtDeviceStatus.setText(context.getResources().getString(R.string.device_active));
        } else {
            holder.binding.txtDeviceStatus.setText(context.getResources().getString(R.string.device_deactivated));
        }
        holder.binding.imgMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showBottom(model.getId());
            }
        });
    }

    private void showBottom(Long DeviceId) {
        mBottomSheetDialog = new BottomSheetDialog(context);
        ActivateDeavtivateBottomSheetBinding bottomSheetBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.activate_deavtivate_bottom_sheet, null, false);

        mBottomSheetDialog.setContentView(bottomSheetBinding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) bottomSheetBinding.getRoot().getParent());
        CompositeDisposable disposable = new CompositeDisposable();
        DeviceManagementRepository deviceManagementRepository = new DeviceManagementRepository();

        bottomSheetBinding.cardViewActivateDeactivate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showDialog(context.getResources().getString(R.string.loading));
                disposable.add(deviceManagementRepository.chaneDeviceStatus(String.valueOf(DeviceId))
                        .subscribeOn(Schedulers.io())
                        .observeOn(AndroidSchedulers.mainThread())
                        .subscribeWith(new DisposableSingleObserver<DeviceListModel>() {
                            @Override
                            public void onSuccess(DeviceListModel deviceListModel) {
                                if (deviceListModel != null && deviceListModel.getMessage() != null)
                                    Toast.makeText(context, deviceListModel.getMessage(), Toast.LENGTH_LONG).show();
                                mBottomSheetDialog.dismiss();
                                hideDialog();
                            }

                            @Override
                            public void onError(Throwable e) {
                                hideDialog();
                            }
                        }));
            }
        });

        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(bottomSheetBinding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
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

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DeviceLoginDetailListBinding binding;

        public MyViewHolder(DeviceLoginDetailListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}
package com.ibl.apps.Adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.ActivateDeavtivateBottomSheetBinding;
import com.ibl.apps.noon.databinding.DeviceLoginDetailListBinding;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class AllDeviceListAdapter extends RecyclerView.Adapter<AllDeviceListAdapter.MyViewHolder> {

    Context context;
    private BottomSheetDialog mBottomSheetDialog;

    public AllDeviceListAdapter(Context context) {
        this.context = context;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DeviceLoginDetailListBinding binding;

        public MyViewHolder(DeviceLoginDetailListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }


    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        DeviceLoginDetailListBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()),
                R.layout.device_login_detail_list, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        holder.binding.imgMenu.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                showBottom();
            }
        });
    }

    private void showBottom() {
        mBottomSheetDialog = new BottomSheetDialog(context);
        ActivateDeavtivateBottomSheetBinding bottomSheetBinding = DataBindingUtil.inflate(LayoutInflater.from(context), R.layout.activate_deavtivate_bottom_sheet, null, false);

        mBottomSheetDialog.setContentView(bottomSheetBinding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) bottomSheetBinding.getRoot().getParent());
        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(bottomSheetBinding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
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
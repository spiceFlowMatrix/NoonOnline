package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;

import com.ibl.apps.Model.AllGradeObject;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.SpinnerLayoutBinding;

import java.util.ArrayList;

/**
 * Created by iblinfotech on 12/06/18.
 */

public class SpinnerItemAdapter extends BaseAdapter {

    Context c;
    ArrayList<AllGradeObject.Data> spinnerObjectArrayList;

    public SpinnerItemAdapter(Context context, ArrayList<AllGradeObject.Data> spinnerObjectArrayList) {
        super();
        this.c = context;
        this.spinnerObjectArrayList = spinnerObjectArrayList;
    }

    @Override
    public int getCount() {
        return spinnerObjectArrayList.size();
    }

    @Override
    public Object getItem(int position) {
        return spinnerObjectArrayList.get(position);
    }

    @Override
    public long getItemId(int position) {
        return 0;
    }

    public View getView(int position, View convertView, ViewGroup parent) {

        SpinnerViewHolder holder;
        if (convertView == null) {
            SpinnerLayoutBinding itemBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.spinner_layout, parent, false);
            holder = new SpinnerViewHolder(itemBinding);
            holder.view = itemBinding.getRoot();
            holder.view.setTag(holder);
        } else {
            holder = (SpinnerViewHolder) convertView.getTag();
        }
        holder.spinnerLayoutBinding.txtSpinnerGradeName.setText(spinnerObjectArrayList.get(position).getName());
        return holder.view;
    }

    private class SpinnerViewHolder {
        private View view;
        private SpinnerLayoutBinding spinnerLayoutBinding;

        SpinnerViewHolder(SpinnerLayoutBinding spinnerLayoutBinding) {
            this.view = spinnerLayoutBinding.getRoot();
            this.spinnerLayoutBinding = spinnerLayoutBinding;
        }
    }
}
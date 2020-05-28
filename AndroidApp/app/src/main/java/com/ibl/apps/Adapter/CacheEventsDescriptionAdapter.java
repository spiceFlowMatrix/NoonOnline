package com.ibl.apps.Adapter;

import android.view.LayoutInflater;
import android.view.ViewGroup;

import androidx.annotation.NonNull;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CacheEventsDescriptionListBinding;

public class CacheEventsDescriptionAdapter extends RecyclerView.Adapter<CacheEventsDescriptionAdapter.ViewHolder> {
    @NonNull
    @Override
    public CacheEventsDescriptionAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        CacheEventsDescriptionListBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.cache_events_description_list, parent, false);

        //context = parent.getContext();
        return new CacheEventsDescriptionAdapter.ViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull CacheEventsDescriptionAdapter.ViewHolder holder, int position) {

    }

    @Override
    public int getItemCount() {
        return 1;
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        CacheEventsDescriptionListBinding binding;

        ViewHolder(CacheEventsDescriptionListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}

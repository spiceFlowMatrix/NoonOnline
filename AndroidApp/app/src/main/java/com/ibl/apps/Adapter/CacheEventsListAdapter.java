package com.ibl.apps.Adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;

import com.google.gson.Gson;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.noon.CacheEventsDescriptionActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.CacheEventsListBinding;

import java.util.ArrayList;
import java.util.List;

public class CacheEventsListAdapter extends RecyclerView.Adapter<CacheEventsListAdapter.ViewHolder> {
    private Context context;
    private List<SyncAPITable> syncAPITables = new ArrayList<>();


    public CacheEventsListAdapter(Context context, List<SyncAPITable> syncAPITableList) {
        this.context = context;
        this.syncAPITables = syncAPITableList;
    }

    @NonNull
    @Override
    public CacheEventsListAdapter.ViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        CacheEventsListBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.cache_events_list, parent, false);

        //context = parent.getContext();
        return new CacheEventsListAdapter.ViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull CacheEventsListAdapter.ViewHolder holder, int position) {
        SyncAPITable model = syncAPITables.get(position);
        if (model.getApi_name() != null)
            holder.binding.txtCacheEventsName.setText(model.getApi_name());

        if (model.getCourseName() != null && model.getGradeName() != null)
            holder.binding.txtCourseName.setText(model.getCourseName() + " " + model.getGradeName() + ", " + context.getResources().getString(R.string.sync_id) + " " + model.getId());

        if (model.getEndpoint_url().equals("ProgessSync/AppTimeTrack") || model.getEndpoint_url().equals("ProgessSync/ProgessSyncAdd")) {
//            holder.binding.txtCourseName.setVisibility(View.INVISIBLE);
            holder.binding.txtCourseName.setText(context.getResources().getString(R.string.sync_id) + " " + model.getId());
        }

        if (model.getStatus() != null)
            holder.binding.txtStatus.setText(model.getStatus());

        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                if (model.getStatus().equals(context.getResources().getString(R.string.errored_status))) {
                    Gson gson = new Gson();
                    String syncAPIData = gson.toJson(model);
                    Intent intent = new Intent(context, CacheEventsDescriptionActivity.class);
                    intent.putExtra("CacheEventsDetail", syncAPIData);
                    context.startActivity(intent);
                } else {
                    Toast.makeText(context, context.getResources().getString(R.string.no_description), Toast.LENGTH_SHORT).show();
                }
            }
        });
    }

    public void updateData(List<SyncAPITable> syncAPITables) {
        this.syncAPITables = syncAPITables;
        notifyDataSetChanged();
    }

    @Override
    public int getItemCount() {
        return syncAPITables.size();
    }

    public class ViewHolder extends RecyclerView.ViewHolder {
        CacheEventsListBinding binding;

        ViewHolder(CacheEventsListBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}

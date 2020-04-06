package com.ibl.apps.Adapter;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import androidx.databinding.DataBindingUtil;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.ibl.apps.util.Const;
import com.ibl.apps.noon.FilesFeedbackActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.QueueListItemBinding;

public class CompletedListAdapter extends RecyclerView.Adapter<CompletedListAdapter.QueueViewHolder> {
    Context context;

    @NonNull
    @Override
    public CompletedListAdapter.QueueViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        QueueListItemBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.queue_list_item, parent, false);
        context = parent.getContext();
        return new CompletedListAdapter.QueueViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull QueueViewHolder holder, int i) {

        holder.binding.tvTitle.setText(context.getResources().getString(R.string.lesson_assignment_issue));
        holder.binding.tvDiscription.setText("When I click on the Lesson app gets crashed");

        holder.itemView.setOnClickListener(view -> {
            Intent reportIntent = new Intent(context, FilesFeedbackActivity.class);
            reportIntent.putExtra(Const.Flag, 4);
            reportIntent.putExtra("description", holder.binding.tvDiscription.getText().toString().trim());
            context.startActivity(reportIntent);
            ((Activity) context).finish();
        });

    }

    @Override
    public int getItemCount() {
        return 6;
    }

    class QueueViewHolder extends RecyclerView.ViewHolder {
        QueueListItemBinding binding;

        QueueViewHolder(QueueListItemBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}

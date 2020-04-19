package com.ibl.apps.Adapter;

import android.content.Context;

import androidx.databinding.DataBindingUtil;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.QueueListItemBinding;

public class InprogressListAdapter extends RecyclerView.Adapter<InprogressListAdapter.QueueViewHolder> {
    Context context;

    @NonNull
    @Override
    public QueueViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        QueueListItemBinding itemViewBinding = DataBindingUtil.inflate(
                LayoutInflater.from(parent.getContext()),
                R.layout.queue_list_item, parent, false);
        context = parent.getContext();
        return new InprogressListAdapter.QueueViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull QueueViewHolder holder, int i) {

//        holder.binding.tvTitle.setText(context.getResources().getString(R.string.video_issue));
//        holder.binding.tvDiscription.setText("When I click on the create discription");

//        holder.itemView.setOnClickListener(view -> {
//            Intent reportIntent = new Intent(context, FilesFeedbackActivity.class);
//            reportIntent.putExtra(Const.Flag, 2);
//            reportIntent.putExtra("description", holder.binding.tvDiscription.getText().toString().trim());
//            context.startActivity(reportIntent);
//            ((Activity) context).finish();
//        });

    }

    @Override
    public int getItemCount() {
        return 3;
    }

    class QueueViewHolder extends RecyclerView.ViewHolder {
        QueueListItemBinding binding;

        QueueViewHolder(QueueListItemBinding binding) {
            super(binding.getRoot());
            this.binding = binding;
        }
    }
}

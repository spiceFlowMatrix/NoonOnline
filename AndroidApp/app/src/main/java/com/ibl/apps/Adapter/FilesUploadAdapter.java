package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.annotation.NonNull;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;

import com.droidnet.DroidListener;
import com.droidnet.DroidNet;
import com.ibl.apps.Model.feedback.FillesData;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.DiscussionsFilePreviewItemLayoutBinding;

import java.util.ArrayList;

public class FilesUploadAdapter extends RecyclerView.Adapter<FilesUploadAdapter.MyViewHolder> implements DroidListener {
    public static boolean isNetworkConnected = false;
    private final DroidNet mDroidNet;
    private ArrayList<FillesData> images;
    private int flagid;
    private Context context;


    public FilesUploadAdapter(ArrayList<FillesData> images) {
        this.images = images;
        mDroidNet = DroidNet.getInstance();
        mDroidNet.addInternetConnectivityListener(this);
    }

    public static boolean isNetworkAvailable(Context context) {
        return isNetworkConnected;
    }

    @NonNull
    @Override
    public MyViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int i) {
        DiscussionsFilePreviewItemLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.discussions_file_preview_item_layout, parent, false);
        context = parent.getContext();
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(@NonNull MyViewHolder holder, int position) {
        FillesData model = images.get(position);
    }

    @Override
    public int getItemCount() {
        return images.size();
    }

    @Override
    public void onInternetConnectivityChanged(boolean isConnected) {
        isNetworkConnected = isConnected;
    }

    public class MyViewHolder extends RecyclerView.ViewHolder {
        DiscussionsFilePreviewItemLayoutBinding binding;

        public MyViewHolder(DiscussionsFilePreviewItemLayoutBinding imageUploadListBinding) {
            super(imageUploadListBinding.getRoot());
            this.binding = imageUploadListBinding;
        }
    }
}

package com.ibl.apps.Model;

import com.ibl.apps.Adapter.CourseItemInnerListAdapter;

/**
 * Created by iblinfotech on 07/12/18.
 */

public class EncryptDecryptObject {

    private String selectedVideoPath;

    private String filename;

    private  CourseItemInnerListAdapter.MyViewHolder holder;


    public String getSelectedVideoPath() {
        return selectedVideoPath;
    }

    public void setSelectedVideoPath(String selectedVideoPath) {
        this.selectedVideoPath = selectedVideoPath;
    }

    public String getFilename() {
        return filename;
    }

    public void setFilename(String filename) {
        this.filename = filename;
    }

    public CourseItemInnerListAdapter.MyViewHolder getHolder() {
        return holder;
    }

    public void setHolder(CourseItemInnerListAdapter.MyViewHolder holder) {
        this.holder = holder;
    }


    @Override
    public String toString() {
        return "EncryptDecryptObject{" +
                "selectedVideoPath='" + selectedVideoPath + '\'' +
                ", filename='" + filename + '\'' +
                ", holder=" + holder +
                '}';
    }
}

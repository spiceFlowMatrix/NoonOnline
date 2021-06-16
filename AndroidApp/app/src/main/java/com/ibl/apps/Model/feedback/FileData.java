
package com.ibl.apps.Model.feedback;

import com.google.gson.annotations.SerializedName;

public class FileData {

    @SerializedName("files")
    private Files mFiles;
    @SerializedName("id")
    private Long mId;

    public Files getFiles() {
        return mFiles;
    }

    public void setFiles(Files files) {
        mFiles = files;
    }

    public Long getId() {
        return mId;
    }

    public void setId(Long id) {
        mId = id;
    }

}

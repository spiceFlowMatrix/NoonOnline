package com.ibl.apps.Model.feedback;

import android.net.Uri;

public class FillesData {
    private Uri uri;
    private int filetype;

    public Uri getUri() {
        return uri;
    }

    public void setUri(Uri uri) {
        this.uri = uri;
    }

    public int getFiletype() {
        return filetype;
    }

    public void setFiletype(int filetype) {
        this.filetype = filetype;
    }

    @Override
    public String toString() {
        return "FillesData{" +
                "uri='" + uri + '\'' +
                ", filetype=" + filetype +
                '}';
    }
}

package com.ibl.apps.Model;

public class DownloadProgress {

    private long downloadId = 0;
    private long progress = 0;
    private long total = 0;

    public DownloadProgress(long downloadId) {
        this.downloadId = downloadId;
    }

    public long getDownloadId() {
        return downloadId;
    }

    public void setDownloadId(long downloadId) {
        this.downloadId = downloadId;
    }

    public long getProgress() {
        return progress;
    }

    public void setProgress(long progress) {
        this.progress = progress;
    }

    public long getTotal() {
        return total;
    }

    public void setTotal(long total) {
        this.total = total;
    }
}

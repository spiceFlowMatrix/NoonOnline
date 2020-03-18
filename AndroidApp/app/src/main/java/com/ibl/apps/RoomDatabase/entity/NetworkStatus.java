package com.ibl.apps.RoomDatabase.entity;

import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

/**
 * Created by iblinfotech on 26/11/18.
 */

@Entity
public class NetworkStatus {

    @PrimaryKey(autoGenerate = true)
    private int networkstatusid;

    private boolean networkStatus;

    public int getNetworkstatusid() {
        return networkstatusid;
    }

    public void setNetworkstatusid(int networkstatusid) {
        this.networkstatusid = networkstatusid;
    }

    public boolean isNetworkStatus() {
        return networkStatus;
    }

    public void setNetworkStatus(boolean networkStatus) {
        this.networkStatus = networkStatus;
    }

    @Override
    public String toString() {
        return "NetworkStatus{" +
                "networkstatusid=" + networkstatusid +
                ", networkStatus=" + networkStatus +
                '}';
    }
}

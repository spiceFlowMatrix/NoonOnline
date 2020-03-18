package com.ibl.apps.Model;

import android.arch.persistence.room.Entity;
import android.arch.persistence.room.PrimaryKey;

@Entity
public class IntervalTableObject {

    @PrimaryKey(autoGenerate = true)
    private int IntervalTableID;

    private String interval;

    private String localinterval;

    public String getInterval() {
        return interval;
    }

    public void setInterval(String interval) {
        this.interval = interval;
    }

    public int getIntervalTableID() {
        return IntervalTableID;
    }

    public void setIntervalTableID(int intervalTableID) {
        IntervalTableID = intervalTableID;
    }

    public String getLocalinterval() {
        return localinterval;
    }

    public void setLocalinterval(String localinterval) {
        this.localinterval = localinterval;
    }

    @Override
    public String toString() {
        return "IntervalTableObject{" +
                "IntervalTableID=" + IntervalTableID +
                ", interval='" + interval + '\'' +
                ", localinterval='" + localinterval + '\'' +
                '}';
    }
}

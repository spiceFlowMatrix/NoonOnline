
package com.ibl.apps.devicemanagement;


import androidx.room.ColumnInfo;
import androidx.room.Embedded;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.ArrayList;

@SuppressWarnings("unused")
@Entity
public class UserDeviceModel {

    @PrimaryKey(autoGenerate = true)
    private int id;

    @ColumnInfo(name = "macAddress")
    private String macAddress;

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    private String userId = getUser().getId();

    @ColumnInfo(name = "ipAddress")
    private String ipAddress;


    @Embedded
     public User user;

    @Embedded
    private OperatingSystem operatingSystem;

    @ColumnInfo(name = "tags")
    @TypeConverters(DataTypeConverter.class)
    private ArrayList<Tags> tags;

    public void setId(int id) {
        this.id = id;
    }

    public String getMacAddress() {
        return macAddress;
    }

    public void setMacAddress(String macAddress) {
        this.macAddress = macAddress;
    }

    public String getIpAddress() {
        return ipAddress;
    }

    public void setIpAddress(String ipAddress) {
        this.ipAddress = ipAddress;
    }

    public int getId() {
        return id;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public OperatingSystem getOperatingSystem() {
        return operatingSystem;
    }

    public void setOperatingSystem(OperatingSystem operatingSystem) {
        this.operatingSystem = operatingSystem;
    }

    public ArrayList<Tags> getTags() {
        return tags;
    }

    public void setTags(ArrayList<Tags> tags) {
        this.tags = tags;
    }

    @Override
    public String toString() {
        return "ClassPojo [macAddress = " + macAddress + ", ipAddress = " + ipAddress + ", id = " + id + ", user = " + user + ", operatingSystem = " + operatingSystem + ", tags = " + tags + "]";
    }
}

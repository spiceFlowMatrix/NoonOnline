package com.ibl.apps.devicemanagement;

import androidx.room.ColumnInfo;
import androidx.room.Embedded;
import androidx.room.Entity;
import androidx.room.PrimaryKey;
import androidx.room.TypeConverters;

import com.ibl.apps.RoomDatabase.database.DataTypeConverter;

import java.util.ArrayList;

@Entity
public class UserDeviceProfileModel {

    @PrimaryKey(autoGenerate = true)
    private int id;

    @ColumnInfo(name = "currentDevices")
    @TypeConverters(DataTypeConverter.class)
    private ArrayList<CurrentDevices> currentDevices;

    String userId ;
    String deviceId;

    @Embedded
    private User user;

    @Embedded
    private QuotaStatus quotaStatus;


    public ArrayList<CurrentDevices> getCurrentDevices() {
        return currentDevices;
    }

    public void setCurrentDevices(ArrayList<CurrentDevices> currentDevices) {
        this.currentDevices = currentDevices;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getDeviceId() {
        return deviceId;
    }

    public void setDeviceId(String deviceId) {
        this.deviceId = deviceId;
    }

    public User getUser() {
        return user;
    }

    public void setUser(User user) {
        this.user = user;
    }

    public QuotaStatus getQuotaStatus() {
        return quotaStatus;
    }

    public void setQuotaStatus(QuotaStatus quotaStatus) {
        this.quotaStatus = quotaStatus;
    }

    @Override
    public String toString() {
        return "ClassPojo [currentDevices = " + currentDevices + ", user = " + user + ", quotaStatus = " + quotaStatus + "]";
    }

    public class CurrentDevices {
        private String macAddress;

        private String ipAddress;

        private String id;

        private User user;

        private OperatingSystem operatingSystem;

        private ArrayList<Tags> tags;

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

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
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


}

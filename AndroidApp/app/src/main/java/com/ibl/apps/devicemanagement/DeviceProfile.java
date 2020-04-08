package com.ibl.apps.devicemanagement;

public class DeviceProfile {
    private CurrentDevices[] currentDevices;

    private User user;

    private QuotaStatus quotaStatus;

    public CurrentDevices[] getCurrentDevices() {
        return currentDevices;
    }

    public void setCurrentDevices(CurrentDevices[] currentDevices) {
        this.currentDevices = currentDevices;
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

        private Tags[] tags;

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

        public Tags[] getTags() {
            return tags;
        }

        public void setTags(Tags[] tags) {
            this.tags = tags;
        }

        @Override
        public String toString() {
            return "ClassPojo [macAddress = " + macAddress + ", ipAddress = " + ipAddress + ", id = " + id + ", user = " + user + ", operatingSystem = " + operatingSystem + ", tags = " + tags + "]";
        }
    }

    public class Tags {
        private String name;

        private String id;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        @Override
        public String toString() {
            return "ClassPojo [name = " + name + ", id = " + id + "]";
        }
    }

    public class OperatingSystem {
        private String name;

        private String version;

        public String getName() {
            return name;
        }

        public void setName(String name) {
            this.name = name;
        }

        public String getVersion() {
            return version;
        }

        public void setVersion(String version) {
            this.version = version;
        }

        @Override
        public String toString() {
            return "ClassPojo [name = " + name + ", version = " + version + "]";
        }
    }

    public class User {
        private String firstName;

        private String lastName;

        private String password;

        private String userStatus;

        private String phone;

        private String id;

        private String email;

        private String username;

        public String getFirstName() {
            return firstName;
        }

        public void setFirstName(String firstName) {
            this.firstName = firstName;
        }

        public String getLastName() {
            return lastName;
        }

        public void setLastName(String lastName) {
            this.lastName = lastName;
        }

        public String getPassword() {
            return password;
        }

        public void setPassword(String password) {
            this.password = password;
        }

        public String getUserStatus() {
            return userStatus;
        }

        public void setUserStatus(String userStatus) {
            this.userStatus = userStatus;
        }

        public String getPhone() {
            return phone;
        }

        public void setPhone(String phone) {
            this.phone = phone;
        }

        public String getId() {
            return id;
        }

        public void setId(String id) {
            this.id = id;
        }

        public String getEmail() {
            return email;
        }

        public void setEmail(String email) {
            this.email = email;
        }

        public String getUsername() {
            return username;
        }

        public void setUsername(String username) {
            this.username = username;
        }

        @Override
        public String toString() {
            return "ClassPojo [firstName = " + firstName + ", lastName = " + lastName + ", password = " + password + ", userStatus = " + userStatus + ", phone = " + phone + ", id = " + id + ", email = " + email + ", username = " + username + "]";
        }
    }

    public class QuotaStatus {
        private String currentConsumption;

        private CurrentLimit currentLimit;

        public String getCurrentConsumption() {
            return currentConsumption;
        }

        public void setCurrentConsumption(String currentConsumption) {
            this.currentConsumption = currentConsumption;
        }

        public CurrentLimit getCurrentLimit() {
            return currentLimit;
        }

        public void setCurrentLimit(CurrentLimit currentLimit) {
            this.currentLimit = currentLimit;
        }

        @Override
        public String toString() {
            return "ClassPojo [currentConsumption = " + currentConsumption + ", currentLimit = " + currentLimit + "]";
        }
    }

    public class CurrentLimit {
        private String id;

        private User user;

        private String deviceLimit;

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

        public String getDeviceLimit() {
            return deviceLimit;
        }

        public void setDeviceLimit(String deviceLimit) {
            this.deviceLimit = deviceLimit;
        }

        @Override
        public String toString() {
            return "ClassPojo [id = " + id + ", user = " + user + ", deviceLimit = " + deviceLimit + "]";
        }
    }


}

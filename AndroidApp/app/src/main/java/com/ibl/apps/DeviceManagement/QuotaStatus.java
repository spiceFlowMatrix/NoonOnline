package com.ibl.apps.DeviceManagement;

public class QuotaStatus {
        private int currentConsumption;

        private int currentLimit;

        public int getCurrentConsumption() {
            return currentConsumption;
        }

        public void setCurrentConsumption(int currentConsumption) {
            this.currentConsumption = currentConsumption;
        }

        public int getCurrentLimit() {
            return currentLimit;
        }

        public void setCurrentLimit(int currentLimit) {
            this.currentLimit = currentLimit;
        }

        @Override
        public String toString() {
            return "ClassPojo [currentConsumption = " + currentConsumption + ", currentLimit = " + currentLimit + "]";
        }
    }
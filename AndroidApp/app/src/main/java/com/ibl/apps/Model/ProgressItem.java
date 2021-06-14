package com.ibl.apps.Model;

/**
 * Created by iblinfotech on 26/10/18.
 */

public class ProgressItem {

    public int color;
    public float progressItemPercentage;

    public int getColor() {
        return color;
    }

    public void setColor(int color) {
        this.color = color;
    }

    public float getProgressItemPercentage() {
        return progressItemPercentage;
    }

    public void setProgressItemPercentage(float progressItemPercentage) {
        this.progressItemPercentage = progressItemPercentage;
    }

    @Override
    public String toString() {
        return "ProgressItem{" +
                "color=" + color +
                ", progressItemPercentage=" + progressItemPercentage +
                '}';
    }
}

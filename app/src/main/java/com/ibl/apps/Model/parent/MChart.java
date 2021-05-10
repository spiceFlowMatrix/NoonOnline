
package com.ibl.apps.Model.parent;

import java.util.List;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

@SuppressWarnings("unused")
public class MChart {

    @Expose
    private List<Data> data;
    @Expose
    private String message;
    @SerializedName("response_code")
    private Long responseCode;
    @Expose
    private String status;

    public List<Data> getData() {
        return data;
    }

    public void setData(List<Data> data) {
        this.data = data;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Long getResponseCode() {
        return responseCode;
    }

    public void setResponseCode(Long responseCode) {
        this.responseCode = responseCode;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    @SuppressWarnings("unused")
    public static class Data {

        @Expose
        private Long x;
        @Expose
        private float y;

        public Long getX() {
            return x;
        }

        public void setX(Long x) {
            this.x = x;
        }

        public float getY() {
            return y;
        }

        public void setY(float y) {
            this.y = y;
        }

    }

    @SuppressWarnings("unused")
    public static class Da {

        @Expose
        private String x;
        @Expose
        private float y;

        public String getX() {
            return x;
        }

        public void setX(String x) {
            this.x = x;
        }

        public float getY() {
            return y;
        }

        public void setY(float y) {
            this.y = y;
        }

    }

    @SuppressWarnings("unused")
    public static class Mo {

        @Expose
        private Long x;
        @Expose
        private float y;

        public Long getX() {
            return x;
        }

        public void setX(Long x) {
            this.x = x;
        }

        public float getY() {
            return y;
        }

        public void setY(float y) {
            this.y = y;
        }

    }
}

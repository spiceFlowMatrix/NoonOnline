
package com.ibl.apps.Model.parent;

import com.google.gson.annotations.Expose;
import com.google.gson.annotations.SerializedName;

import java.util.List;

@SuppressWarnings("unused")
public class Chart {

    @Expose
    private Data data;
    @Expose
    private String message;
    @SerializedName("response_code")
    private Long responseCode;
    @Expose
    private String status;

    public Data getData() {
        return data;
    }

    public void setData(Data data) {
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
        private List<Da> da;
        @Expose
        private List<Mo> mo;

        public List<Da> getDa() {
            return da;
        }

        public void setDa(List<Da> da) {
            this.da = da;
        }

        public List<Mo> getMo() {
            return mo;
        }

        public void setMo(List<Mo> mo) {
            this.mo = mo;
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

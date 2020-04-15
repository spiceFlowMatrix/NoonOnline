package com.ibl.apps.util;

/**
 * Created by iblinfotech on 11/09/18.
 */

public class Constants {

    private static final Constants instance = new Constants();

    private RUN_TYPE type;

    public static String BASE_URL;

    public void setRunType(RUN_TYPE type) {
        this.type = type;
        if (type == RUN_TYPE.RELEASE) {
            BASE_URL = "http://release.api/endpoints";
        } else if (type == RUN_TYPE.TEST) {
            BASE_URL = "http://test.api/endpoints";
        }
    }

    public static Constants getInstance() {
        return instance;
    }

    // More getters here

    public enum RUN_TYPE {
        TEST,
        RELEASE;
    }

}
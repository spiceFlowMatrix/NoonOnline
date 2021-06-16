package com.ibl.apps.util;

import android.util.Base64;

import java.io.UnsupportedEncodingException;

/**
 * Created by iblinfotech on 11/09/18.
 */

public class JWTUtils {

    public static String[] decoded(String JWTEncoded) throws Exception {
        String[] split = JWTEncoded.split("\\.");
        return split;
    }

    public static String getJson(String strEncoded) throws UnsupportedEncodingException {
        byte[] decodedBytes = Base64.decode(strEncoded, Base64.URL_SAFE);
        return new String(decodedBytes, "UTF-8");
    }
}
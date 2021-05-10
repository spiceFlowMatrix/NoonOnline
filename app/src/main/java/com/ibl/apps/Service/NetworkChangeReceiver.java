package com.ibl.apps.Service;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.util.Log;

public class NetworkChangeReceiver extends BroadcastReceiver {

    static boolean isnetworkvar;

    @Override
    public void onReceive(Context context, Intent intent) {
        try {

            if (intent.getAction().equals(ConnectivityManager.CONNECTIVITY_ACTION)) {

                boolean noConnectivity = intent.getBooleanExtra(ConnectivityManager.EXTRA_NO_CONNECTIVITY, false);

                if (!noConnectivity) {
                    Log.e("NetworkCheckReceiver", "connected");
                    isnetworkvar = true;
                } else {
                    isnetworkvar = false;
                    Log.e("NetworkCheckReceiver", "disconnected");
                }
            }

        } catch (NullPointerException e) {
            e.printStackTrace();
        }
    }

    public static boolean isnetwork() {
        return isnetworkvar;
    }
}
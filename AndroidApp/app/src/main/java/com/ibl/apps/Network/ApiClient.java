package com.ibl.apps.Network;

import android.content.Context;
import android.os.Build;
import android.text.TextUtils;
import android.util.Log;

import com.ibl.apps.Model.AuthTokenObject;
import com.ibl.apps.RoomDatabase.dao.userManagementDatabse.UserDatabaseRepository;
import com.ibl.apps.noon.R;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;

import java.io.IOException;
import java.io.InputStream;
import java.security.KeyManagementException;
import java.security.KeyStore;
import java.security.KeyStoreException;
import java.security.NoSuchAlgorithmException;
import java.security.cert.Certificate;
import java.security.cert.CertificateException;
import java.security.cert.CertificateFactory;
import java.security.cert.X509Certificate;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.TimeUnit;

import javax.net.ssl.SSLContext;
import javax.net.ssl.SSLSocketFactory;
import javax.net.ssl.TrustManager;
import javax.net.ssl.TrustManagerFactory;
import javax.net.ssl.X509TrustManager;

import okhttp3.ConnectionSpec;
import okhttp3.Interceptor;
import okhttp3.OkHttpClient;
import okhttp3.Request;
import okhttp3.Response;
import okhttp3.TlsVersion;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.adapter.rxjava2.RxJava2CallAdapterFactory;
import retrofit2.converter.gson.GsonConverterFactory;

/**
 * Created by ravi on 20/02/18.
 */

public class ApiClient {
    private static Retrofit retrofit = null;
    private static int REQUEST_TIMEOUT = 120;
    private static OkHttpClient okHttpClient;

    public static Retrofit getClient(Context context) {

        if (okHttpClient == null)
            initOkHttp(context);

        if (retrofit == null) {
            retrofit = new Retrofit.Builder()
                    .baseUrl(Const.BASE_URL)
                    .client(okHttpClient)
                    .addCallAdapterFactory(RxJava2CallAdapterFactory.create())
                    .addConverterFactory(GsonConverterFactory.create())
                    .build();
        }
        return retrofit;
    }

    private static void initOkHttp(final Context context) {

        //Log.e(Const.LOG_NOON_TAG, "==SSL==");

        //OkHttpClient.Builder httpClient = enableTls12OnPreLollipop(new OkHttpClient.Builder())

        OkHttpClient.Builder httpClient = new OkHttpClient.Builder()
                .connectTimeout(REQUEST_TIMEOUT, TimeUnit.SECONDS)
                .readTimeout(REQUEST_TIMEOUT, TimeUnit.SECONDS)
                .writeTimeout(REQUEST_TIMEOUT, TimeUnit.SECONDS);

        HttpLoggingInterceptor interceptor = new HttpLoggingInterceptor();
        interceptor.setLevel(HttpLoggingInterceptor.Level.BODY);
        httpClient.addInterceptor(interceptor);
        httpClient.addInterceptor(new Interceptor() {
            @Override
            public Response intercept(Chain chain) throws IOException {

                Request original = chain.request();
                Request.Builder requestBuilder = original.newBuilder()
                        .addHeader("Accept", "application/json")
                        .addHeader("Content-Type", "application/json");

                // Adding Authorization token (API Key)
                // Requests will be denied without API key

                String authid = PrefUtils.getAuthid(context);

                //Log.e(Const.LOG_NOON_TAG, "===authid=" + authid);

                UserDatabaseRepository userDatabaseRepository = new UserDatabaseRepository();
                if (!TextUtils.isEmpty(authid)) {
                    AuthTokenObject authTokenObject = userDatabaseRepository.getAuthTokenData(authid);
                    if (authTokenObject != null) {

                        String accessToken = "";
                        String idToken = "";

                        if (authTokenObject.getAccessToken() != null) {
                            accessToken = authTokenObject.getAccessToken();

                        }
                        if (authTokenObject.getIdToken() != null) {
                            idToken = authTokenObject.getIdToken();
                        }

                        if (!TextUtils.isEmpty(accessToken) && !TextUtils.isEmpty(idToken)) {
                            requestBuilder.addHeader("Authorization", "Bearer" + " " + accessToken);
                            requestBuilder.addHeader("id_token", "Bearer" + " " + idToken);

                            Log.e(Const.LOG_NOON_TAG, "==getExpiresAt==" + authTokenObject.getExpiresAt());
                            Log.e(Const.LOG_NOON_TAG, "==Authorization==" + "Bearer" + " " + accessToken);
                            Log.e(Const.LOG_NOON_TAG, "==id_token==" + "Bearer" + " " + idToken);
                        }
                    }
                }

                Request request = requestBuilder.build();
                return chain.proceed(request);
            }
        });

        try {
            // Tell the URLConnection to use a SocketFactory from our SSLContext
            httpClient.sslSocketFactory(getSSLSocketFactory(context));
        } catch (CertificateException e) {
            e.printStackTrace();
        } catch (KeyStoreException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        } catch (NoSuchAlgorithmException e) {
            e.printStackTrace();
        } catch (KeyManagementException e) {
            e.printStackTrace();
        }

        okHttpClient = httpClient.build();
    }

    private static OkHttpClient.Builder enableTls12OnPreLollipop(OkHttpClient.Builder client) {

        if (Build.VERSION.SDK_INT > 16 && Build.VERSION.SDK_INT < 22) {
            try {
                SSLContext sc = SSLContext.getInstance("TLSv1.2");
                sc.init(null, null, null);
                client.sslSocketFactory(new Tls12SocketFactory(sc.getSocketFactory()));

                ConnectionSpec cs = new ConnectionSpec.Builder(ConnectionSpec.MODERN_TLS)
                        .tlsVersions(TlsVersion.TLS_1_2)
                        .build();

                List<ConnectionSpec> specs = new ArrayList<>();
                specs.add(cs);
                specs.add(ConnectionSpec.COMPATIBLE_TLS);
                specs.add(ConnectionSpec.CLEARTEXT);

                client.connectionSpecs(specs);
            } catch (Exception exc) {
                Log.e("OkHttpTLSCompat", "Error while setting TLS 1.2", exc);
            }
        }

        return client;
    }

    private static SSLSocketFactory getSSLSocketFactory(Context context)
            throws CertificateException, KeyStoreException, IOException,
            NoSuchAlgorithmException, KeyManagementException {
        // Load CAs from an InputStream
        CertificateFactory cf = CertificateFactory.getInstance("X.509");
        InputStream caInput = context.getResources().openRawResource(R.raw.noonssl); // your certificate file
        Certificate ca = cf.generateCertificate(caInput);
        caInput.close();
        // Create a KeyStore containing our trusted CAs
        KeyStore keyStore = KeyStore.getInstance("BKS");
        keyStore.load(null, null);
        keyStore.setCertificateEntry("ca", ca);

        String tmfAlgorithm = TrustManagerFactory.getDefaultAlgorithm();
        TrustManagerFactory tmf = TrustManagerFactory.getInstance(tmfAlgorithm);
        tmf.init(keyStore);
        TrustManager[] wrappedTrustManagers = getWrappedTrustManagers(tmf.getTrustManagers());
        // Create an SSLContext that uses our TrustManager
        SSLContext sslContext = SSLContext.getInstance("TLS");
        sslContext.init(null, wrappedTrustManagers, null);
        return sslContext.getSocketFactory();
    }

    private static TrustManager[] getWrappedTrustManagers(TrustManager[] trustManagers) {
        final X509TrustManager originalTrustManager = (X509TrustManager) trustManagers[0];
        return new TrustManager[]{

                new X509TrustManager() {
                    public X509Certificate[] getAcceptedIssuers() {
                        return originalTrustManager.getAcceptedIssuers();
                    }

                    public void checkClientTrusted(X509Certificate[] certs, String authType) {
                        try {
                            originalTrustManager.checkClientTrusted(certs, authType);
                        } catch (CertificateException ignored) {
                        }
                    }

                    public void checkServerTrusted(X509Certificate[] certs, String authType) {
                        try {
                            originalTrustManager.checkServerTrusted(certs, authType);
                        } catch (CertificateException ignored) {
                        }
                    }
                }
        };
    }
}

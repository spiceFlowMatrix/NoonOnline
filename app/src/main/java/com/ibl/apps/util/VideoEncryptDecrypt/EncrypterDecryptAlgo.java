package com.ibl.apps.util.VideoEncryptDecrypt;

import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Typeface;
import androidx.core.content.res.ResourcesCompat;
import android.text.SpannableStringBuilder;
import android.text.Spanned;
import android.util.Log;

import com.ibl.apps.util.CustomTypefaceSpan;
import com.ibl.apps.noon.R;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
import java.security.spec.AlgorithmParameterSpec;

import javax.crypto.Cipher;
import javax.crypto.CipherOutputStream;
import javax.crypto.NoSuchPaddingException;
import javax.crypto.SecretKey;

import fr.maxcom.http.LocalSingleHttpServer;


/**
 * Created by akash on 21/5/16.
 */

public class EncrypterDecryptAlgo {
    public static Context mcontext;
    private final static int DEFAULT_READ_WRITE_BLOCK_BUFFER_SIZE = 1024;
    private final static String ALGORITHM_VIDEO_ENCRYPTOR = "AES/CBC/PKCS5Padding";

//    public EncrypterDecryptAlgo(Context context) {
//        this.mcontext = context;
//    }

    @SuppressWarnings("resource")
    public static void encrypt(SecretKey key, AlgorithmParameterSpec paramSpec, InputStream in, OutputStream out)
            throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException,
            InvalidAlgorithmParameterException, IOException {
        try {
            Cipher c = Cipher.getInstance(ALGORITHM_VIDEO_ENCRYPTOR);
            c.init(Cipher.ENCRYPT_MODE, key, paramSpec);
            out = new CipherOutputStream(out, c);
            int count;
            byte[] buffer = new byte[DEFAULT_READ_WRITE_BLOCK_BUFFER_SIZE];
            while ((count = in.read(buffer)) >= 0) {
                out.write(buffer, 0, count);
            }
        } finally {
            out.close();
        }
    }

    @SuppressWarnings("resource")
    public static String decrypt(SecretKey key, AlgorithmParameterSpec paramSpec, InputStream in, String encrypted_path)
            throws NoSuchAlgorithmException, NoSuchPaddingException, InvalidKeyException,
            InvalidAlgorithmParameterException, IOException, SecurityException {
        LocalSingleHttpServer mserver;
        String path = "";
        try {
            mserver = new LocalSingleHttpServer();
            Cipher c = Cipher.getInstance(ALGORITHM_VIDEO_ENCRYPTOR);
            c.init(Cipher.DECRYPT_MODE, key, paramSpec);
            //  out = new CipherOutputStream(out, c);
            int count;

            mserver.setCipher(c);
            mserver.start();
            path = mserver.getURL(encrypted_path);

//            byte[] buffer = new byte[DEFAULT_READ_WRITE_BLOCK_BUFFER_SIZE];
//            while ((count = in.read(buffer)) >= 0) {
//                out.write(buffer, 0, count);
//            }
        } finally {
//            out.close();
        }
        return path;
    }

    public static void showAlertDialog(Context context) {
        try {
            Log.e("showLogoutAlert", "---try--");
            SpannableStringBuilder message = setTypeface(context.getString(R.string.Expired_Key));
            androidx.appcompat.app.AlertDialog.Builder builder = new androidx.appcompat.app.AlertDialog.Builder(context);
            Log.e("showLogoutAlert", "-builder--");

            builder.setTitle(context.getString(R.string.validation_warning));
            builder.setMessage(message)
                    .setPositiveButton(context.getString(R.string.button_ok), new DialogInterface.OnClickListener() {
                        public void onClick(DialogInterface dialog, int which) {
                            // Yes button clicked, do something
                            dialog.dismiss();
                        }
                    });
            builder.setNegativeButton(context.getString(R.string.button_cancel), new DialogInterface.OnClickListener() {
                public void onClick(DialogInterface dialog, int which) {
                    dialog.cancel();
                }
            });

            builder.show();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    public static SpannableStringBuilder setTypeface(String message) {
        Typeface typeface = ResourcesCompat.getFont(mcontext, R.font.bahij_helvetica_neue_bold);
        SpannableStringBuilder spannableStringBuilder = new SpannableStringBuilder(message);
        spannableStringBuilder.setSpan(new CustomTypefaceSpan("", typeface), 0, message.length(), Spanned.SPAN_EXCLUSIVE_INCLUSIVE);
        return spannableStringBuilder;
    }


}
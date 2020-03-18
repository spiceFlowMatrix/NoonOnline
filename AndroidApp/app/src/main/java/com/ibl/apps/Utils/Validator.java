package com.ibl.apps.Utils;

import android.util.Log;
import android.util.Patterns;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.Calendar;
import java.util.GregorianCalendar;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class Validator {

    public static boolean checkEmpty(EditText edt) {
        if (edt.getText().toString().trim().isEmpty()) {
            edt.setFocusable(true);
            edt.requestFocus();

            return false;
        }
        return true;
    }

    public static boolean checkEmptyTextview(TextView edt) {
        if (edt.getText().toString().trim().isEmpty() || edt.getText().toString().trim().equals("00")) {
            edt.setFocusable(true);
            edt.requestFocus();

            return false;
        }
        return true;
    }

    public static boolean checkLength(EditText edt) {
        if (edt.getText().toString().length() < 6) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean comparePWD(EditText edt1, EditText edt2) {
        if ((edt1.getText().toString().isEmpty() || edt2.getText().toString().isEmpty()) ||
                !edt1.getText().toString().trim().equals(edt2.getText().toString().trim())) {
            edt2.setFocusable(true);
            edt2.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkDescLimit(EditText edt) {
        if (edt.getText().toString().length() > 150) {
//            Log.e("" + edt.getId(), "empty");
//            edt.setError("field required");
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkPhnno(EditText edt) {

        String PHONE_PATTERN = "\\d{4}([- ]*)\\d{6}";
        Pattern pattern = Pattern.compile(PHONE_PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());
        boolean flg = matcher.matches();
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            edt.setError("Mobile Number is not valid");
            return false;
        }
        return true;

    }

    public static boolean checkEmail(EditText edt) {
//        emailPattern [A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\\.[A-Za-z]{2,4}

        String EMAIL_PATTERN = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@"
                + "[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";

        Pattern pattern = Pattern.compile(EMAIL_PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());

        boolean flg = matcher.matches();
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkUser(EditText edt) {
        String USER_PATTERN = "[a-zA-Z0-9]{1,250}";

        Pattern pattern = Pattern.compile(USER_PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());

        boolean flg = matcher.matches();
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkAlphaNumeric(EditText edt) {
        String PATTERN = "[-\\p{Alnum}]+";
//        String PATTERN = "[/^[a-zA-Z0-9_.-s]{6,15}$/]";
        Pattern pattern = Pattern.compile(PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());
        boolean flg = matcher.matches();
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }

        return true;
    }

    public static boolean checkOnlyAlpha(EditText edt) {
        String PATTERN = "[a-zA-Z]+";
        Pattern pattern = Pattern.compile(PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());
        boolean flg = matcher.matches();
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkPhn(EditText edt) {
        String PATTERN = "^\\(?([0-9]{3})\\)?[-.\\\\s]?([0-9]{3})[-.\\s]?([0-9]{4})$";
        Pattern pattern = Pattern.compile(PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());
        boolean flg = matcher.matches();
        Log.e("---", "-" + matcher.matches());
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkMobileLength(EditText reg_password) {
        String value = reg_password.getText().toString();
        int password_length = value.length();
        if (password_length < 10) {
            reg_password.setFocusable(true);
            reg_password.requestFocus();
            return false;
        }
        if (password_length > 10) {
            reg_password.setFocusable(true);
            reg_password.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkPassword(EditText edt) {
//       String PATTERN = "((?=.*\\\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,})";
        String PATTERN = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^\\w\\s]).{8,}$";
        Pattern pattern = Pattern.compile(PATTERN);
        Matcher matcher = pattern.matcher(edt.getText().toString());
        boolean flg = matcher.matches();
        if (!flg) {
            edt.setFocusable(true);
            edt.requestFocus();
            return false;
        }
        return true;
    }

    public static boolean checkBitmapNullOrNot(ImageView imageView) {
        if (imageView.getDrawable() == null) {
            return false;
        }

//        Bitmap bitmap = ((BitmapDrawable) imageView.getDrawable()).getBitmap();
//        ByteArrayOutputStream stream = new ByteArrayOutputStream();
//        bitmap.compress(Bitmap.CompressFormat.PNG, 100, stream);
//        byte[] bytes = stream.toByteArray();
//        Log.e("bitmap to byte", Base64.encodeToString(bytes, 0));
//        String encode_value = Base64.encodeToString(bytes, 0);
//        if (encode_value.isEmpty()) {
//            return false;
//        }
        return true;
    }

    public static boolean checkAdult(int year, int month, int day) {
        Calendar userAge = new GregorianCalendar(year, month, day);
        Calendar minAdultAge = new GregorianCalendar();
        minAdultAge.add(Calendar.YEAR, -18);
        if (minAdultAge.before(userAge)) {
            return false;
        }
        return true;
    }

    public static boolean isValidUrl(EditText editText) {
        Pattern p = Patterns.WEB_URL;
        Matcher m = p.matcher(editText.getText().toString().trim().toLowerCase());
        return m.matches();
    }

    public static boolean isValidStringUrl(String strUrl) {
        Pattern p = Patterns.WEB_URL;
        Matcher m = p.matcher(strUrl);
        return m.matches();
    }
}

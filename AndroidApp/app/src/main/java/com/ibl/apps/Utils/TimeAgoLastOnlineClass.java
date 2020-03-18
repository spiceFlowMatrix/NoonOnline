package com.ibl.apps.Utils;

/**
 * Created by IBL InfoTech on 9/7/2017.
 */

import android.text.format.DateFormat;
import android.util.Log;

import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;
import java.util.concurrent.TimeUnit;

public class TimeAgoLastOnlineClass {

    public static String getTimeAgo(long timestamp) {

        Calendar cal = Calendar.getInstance();
        TimeZone tz = cal.getTimeZone();//get your local time zone.
        SimpleDateFormat sdf = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss a", Locale.ENGLISH);
        sdf.setTimeZone(tz);//set time zone.
        String localTime = sdf.format(new Date(timestamp * 1000));
        Date date = new Date();
        try {
            date = sdf.parse(localTime);//get local date
        } catch (ParseException e) {
            e.printStackTrace();
        }

        if (date == null) {
            return null;
        }

        long time = date.getTime();

        Date curDate = currentDate();
        long now = curDate.getTime();
        if (time > now || time <= 0) {
            return null;
        }

        int timeDIM = getTimeDistanceInMinutes(time);

        String timeAgo = null;

        if (timeDIM == 0) {
            return NoonApplication.getContext().getString(R.string.just_now);
        } else if (timeDIM == 1) {
            return NoonApplication.getContext().getString(R.string.one_minute);
        } else if (timeDIM >= 2 && timeDIM <= 44) {
            timeAgo = timeDIM + " " + NoonApplication.getContext().getString(R.string.minutes);
        } else if (timeDIM >= 45 && timeDIM <= 89) {
            timeAgo = NoonApplication.getContext().getString(R.string.about_an_hour);
        } else if (timeDIM >= 90 && timeDIM <= 1439) {
            timeAgo = (Math.round(timeDIM / 60)) + " " + NoonApplication.getContext().getString(R.string.hours);
        } else if (timeDIM >= 1440 && timeDIM <= 2519) {
            timeAgo = NoonApplication.getContext().getString(R.string.one_day);
        } else if (timeDIM >= 2520 && timeDIM <= 43199) {
            timeAgo = (Math.round(timeDIM / 1440)) + " " + NoonApplication.getContext().getString(R.string.days);
        } else if (timeDIM >= 43200 && timeDIM <= 86399) {
            timeAgo = NoonApplication.getContext().getString(R.string.about_a_month);
        } else if (timeDIM >= 86400 && timeDIM <= 525599) {
            timeAgo = (Math.round(timeDIM / 43200)) + " " + NoonApplication.getContext().getString(R.string.months);
        } else if (timeDIM >= 525600 && timeDIM <= 655199) {
            timeAgo = NoonApplication.getContext().getString(R.string.about_a_year);
        } else if (timeDIM >= 655200 && timeDIM <= 914399) {
            timeAgo = NoonApplication.getContext().getString(R.string.over_a_year);
        } else if (timeDIM >= 914400 && timeDIM <= 1051199) {
            timeAgo = NoonApplication.getContext().getString(R.string.almost_two_years);
        } else {
            timeAgo = (Math.round(timeDIM / 525600)) + " " + NoonApplication.getContext().getString(R.string.years);
        }

        return timeAgo + " " + NoonApplication.getContext().getString(R.string.ago);
    }

    public static Date currentDate() {
        Calendar calendar = Calendar.getInstance();
        return calendar.getTime();
    }

    private static int getTimeDistanceInMinutes(long time) {
        long timeDistance = currentDate().getTime() - time;
        return Math.round((Math.abs(timeDistance) / 1000) / 60);
    }

    public static String convertDate(String dateInMilliseconds, String dateFormat) {
        return DateFormat.format(dateFormat, Long.parseLong(dateInMilliseconds)).toString();
    }

    public static String getDateCurrentTimeZone(long timestamp) {
        try {
            Calendar calendar = Calendar.getInstance();
            TimeZone tz = TimeZone.getDefault();
            calendar.setTimeInMillis(timestamp * 1000);
            calendar.add(Calendar.MILLISECOND, tz.getOffset(calendar.getTimeInMillis()));
            SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.ENGLISH);
            Date currenTimeZone = (Date) calendar.getTime();
            return sdf.format(currenTimeZone);
        } catch (Exception e) {
        }
        return "";
    }

    public static String getcurrentDate() {
        Calendar calendar = Calendar.getInstance();
        SimpleDateFormat sdf = new SimpleDateFormat("dd/MM/yyyy", Locale.ENGLISH);
        Date currenTimeZone = (Date) calendar.getTime();
        return sdf.format(currenTimeZone);
    }

    public static String getDayDifference(String expierDate) {
        try {
            //Dates to compare
            Date date1;
            Date date2;

            SimpleDateFormat dates = new SimpleDateFormat("dd/MM/yyyy", Locale.ENGLISH);

            //Setting dates
            date1 = dates.parse(getcurrentDate());
            date2 = dates.parse(expierDate);

            //Comparing dates
            long difference = Math.abs(date1.getTime() - date2.getTime());
            long differenceDates = difference / (24 * 60 * 60 * 1000);

            //Convert long to String
            String dayDifference = Long.toString(differenceDates);
            //Log.e("HERE", "HERE: " + dayDifference);

            return dayDifference;

        } catch (Exception exception) {
            //Log.e("DIDN'T WORK", "exception " + exception);
        }
        return null;
    }

    public static boolean getDayAfter(String expierDate) {
        try {
            //Dates to compare
            Date date1;
            Date date2;

            SimpleDateFormat dates = new SimpleDateFormat("dd/MM/yyyy", Locale.ENGLISH);

            //Setting dates
            date1 = dates.parse(getcurrentDate());
            date2 = dates.parse(expierDate);

            if (date1.after(date2)) {
                return true;
            }

        } catch (Exception exception) {
            //Log.e("DIDN'T WORK", "exception " + exception);
        }
        return false;
    }

    public String covertTimeToText(String dataDate) {

        String convTime = null;

        String prefix = "";
        String suffix = " " + NoonApplication.getContext().getString(R.string.ago);

        try {
//            SimpleDateFormat dateFormat = new SimpleDateFormat("MM/dd/yyyy hh:mm:ss aa", Locale.ENGLISH);
            SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss",Locale.ENGLISH);
            Date pasTime = dateFormat.parse(dataDate);
            Date nowTime = new Date();

            long dateDiff = nowTime.getTime() - pasTime.getTime();

            long second = TimeUnit.MILLISECONDS.toSeconds(dateDiff);
            long minute = TimeUnit.MILLISECONDS.toMinutes(dateDiff);
            long hour = TimeUnit.MILLISECONDS.toHours(dateDiff);
            long day = TimeUnit.MILLISECONDS.toDays(dateDiff);

            if (second < 60) {
                convTime = NoonApplication.getContext().getString(R.string.just_now);
            } else if (minute < 60) {
                convTime = minute + " " + NoonApplication.getContext().getString(R.string.minutes) + suffix;
            } else if (hour < 24) {
                convTime = hour + " " + NoonApplication.getContext().getString(R.string.hours) + suffix;
            } else if (day >= 7) {
                if (day > 30) {
                    convTime = (day / 30) + " " + NoonApplication.getContext().getString(R.string.months) + suffix;
                } else if (day > 360) {
                    convTime = (day / 360) + " " + NoonApplication.getContext().getString(R.string.years) + suffix;
                } else {
                    convTime = (day / 7) + " " + NoonApplication.getContext().getString(R.string.week) + suffix;
                }
            } else if (day < 7) {
                convTime = day + " " + " " + NoonApplication.getContext().getString(R.string.days) + suffix;
            }
        } catch (ParseException e) {
            e.printStackTrace();
            Log.e("ConvTimeE", e.getMessage());
        }
        return convTime;
    }


    public String getNewDate(String getOldDate) {
        if (getOldDate == null) {
            return "";
        }
        SimpleDateFormat oldFormatter = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss'UTC'", Locale.ENGLISH);
        oldFormatter.setTimeZone(TimeZone.getTimeZone("UTC"));
        Date value = null;
        String dueDateAsNormal = "";
        try {
            value = oldFormatter.parse(getOldDate);
            SimpleDateFormat newFormatter = new SimpleDateFormat("MM/dd/yyyy - hh:mm a", Locale.ENGLISH);
            newFormatter.setTimeZone(TimeZone.getDefault());
            dueDateAsNormal = newFormatter.format(value);
        } catch (ParseException e) {
            e.printStackTrace();
        }

        return dueDateAsNormal;
    }
}
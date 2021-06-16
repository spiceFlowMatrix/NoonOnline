package com.ibl.apps.Model.parent;

import android.util.Log;

import com.github.mikephil.charting.components.AxisBase;
import com.github.mikephil.charting.data.Entry;
import com.github.mikephil.charting.formatter.IAxisValueFormatter;

import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;
import java.util.List;
import java.util.Locale;


public class GraphXAxisValueFormatter implements IAxisValueFormatter {

    private static int MINUTES_INTERVAL = 5;
    private String[] mValues;
    private int mInterval;
    private float value;

    public GraphXAxisValueFormatter(Entry range, int interval) {
        mValues = new String[1];
        value = range.getX();
        mInterval = 1;


        Calendar calendar = Calendar.getInstance();
        if (interval == 1) {

            SimpleDateFormat sdf = null;
            sdf = new SimpleDateFormat("MM", Locale.ENGLISH);
            try {
                Date mDate = sdf.parse(String.valueOf(range.getX()));
                long timeInMilliseconds = mDate.getTime();
                //System.out.println("Date in milli :: " + timeInMilliseconds);
                calendar.setTimeInMillis(timeInMilliseconds);
            } catch (ParseException e) {
                e.printStackTrace();
            }


            int unroundedMinutes = calendar.get(Calendar.MINUTE);
            int mod = unroundedMinutes % MINUTES_INTERVAL;
            calendar.add(Calendar.MINUTE, mod < 8 ? -mod : (MINUTES_INTERVAL - mod));


            String s = "";
            s = getMonthFromTimestamp(calendar.getTimeInMillis());


            /*if (slot.equals(Interval.HOUR) || slot.equals(Interval.DAY))
                s = getTimeFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.WEEK))
                s = getDayFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.MONTH))
                s = getMonthFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.YEAR))
                s = getYearFromTimestamp(calendar.getTimeInMillis());
*/

            //setLog("Time : " + s);
            mValues[0] = s;
        } else {

            SimpleDateFormat sdf = null;

            sdf = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.ENGLISH);
            try {
                Date mDate = sdf.parse(String.valueOf(range.getX()));
                long timeInMilliseconds = mDate.getTime();
                //System.out.println("Date in milli :: " + timeInMilliseconds);
                calendar.setTimeInMillis(timeInMilliseconds);
            } catch (ParseException e) {
                e.printStackTrace();
            }

            int unroundedMinutes = calendar.get(Calendar.MINUTE);
            int mod = unroundedMinutes % MINUTES_INTERVAL;
            calendar.add(Calendar.MINUTE, mod < 8 ? -mod : (MINUTES_INTERVAL - mod));

            String s = "";
            s = getTimeFromTimestamp(calendar.getTimeInMillis());


            /*if (slot.equals(Interval.HOUR) || slot.equals(Interval.DAY))
                s = getTimeFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.WEEK))
                s = getDayFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.MONTH))
                s = getMonthFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.YEAR))
                s = getYearFromTimestamp(calendar.getTimeInMillis());
*/

            //setLog("Time : " + s);
            mValues[0] = s;

        }
//        return mValues[0];
    }

    public String getDate() {
        if (value % mInterval == 0 && value >= 0) {
            return mValues[(int) value % mValues.length];
        } else
            return "";
    }

    public GraphXAxisValueFormatter(List<Chart.Mo> range, int interval, List<Chart.Da> date) {

        if (interval == 1) {
            mValues = new String[range.size()];
        } else {
            mValues = new String[date.size()];
        }
        mInterval = 1;


        Calendar calendar = Calendar.getInstance();
        if (interval == 1) {
            for (int i = 0; i < range.size(); i++) {
                SimpleDateFormat sdf = null;
                sdf = new SimpleDateFormat("MM", Locale.ENGLISH);
                try {
                    Date mDate = sdf.parse(String.valueOf(range.get(i).getX()));
                    long timeInMilliseconds = mDate.getTime();
                    //System.out.println("Date in milli :: " + timeInMilliseconds);
                    calendar.setTimeInMillis(timeInMilliseconds);
                } catch (ParseException e) {
                    e.printStackTrace();
                }


                int unroundedMinutes = calendar.get(Calendar.MINUTE);
                int mod = unroundedMinutes % MINUTES_INTERVAL;
                calendar.add(Calendar.MINUTE, mod < 8 ? -mod : (MINUTES_INTERVAL - mod));


                String s = "";
                s = getMonthFromTimestamp(calendar.getTimeInMillis());


            /*if (slot.equals(Interval.HOUR) || slot.equals(Interval.DAY))
                s = getTimeFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.WEEK))
                s = getDayFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.MONTH))
                s = getMonthFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.YEAR))
                s = getYearFromTimestamp(calendar.getTimeInMillis());
*/

                //setLog("Time : " + s);
                mValues[i] = s;
            }
        } else {
            for (int i = 0; i < date.size(); i++) {
                SimpleDateFormat sdf = null;

                sdf = new SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss", Locale.ENGLISH);
                try {
                    Date mDate = sdf.parse(String.valueOf(date.get(i).getX()));
                    long timeInMilliseconds = mDate.getTime();
                    //System.out.println("Date in milli :: " + timeInMilliseconds);
                    calendar.setTimeInMillis(timeInMilliseconds);
                } catch (ParseException e) {
                    e.printStackTrace();
                }

                int unroundedMinutes = calendar.get(Calendar.MINUTE);
                int mod = unroundedMinutes % MINUTES_INTERVAL;
                calendar.add(Calendar.MINUTE, mod < 8 ? -mod : (MINUTES_INTERVAL - mod));

                String s = "";
                s = getTimeFromTimestamp(calendar.getTimeInMillis());


            /*if (slot.equals(Interval.HOUR) || slot.equals(Interval.DAY))
                s = getTimeFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.WEEK))
                s = getDayFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.MONTH))
                s = getMonthFromTimestamp(calendar.getTimeInMillis());
            else if (slot.equals(Interval.YEAR))
                s = getYearFromTimestamp(calendar.getTimeInMillis());
*/

                //setLog("Time : " + s);
                mValues[i] = s;
            }
        }


    }

    private void setLog(String s) {
        Log.d("time", "setLog: " + s);
    }

    private String getYearFromTimestamp(long timeInMillis) {
        SimpleDateFormat formatter = new SimpleDateFormat("yyyy");

        return formatter.format(new Date(timeInMillis));
    }

    private String getMonthFromTimestamp(long timeInMillis) {
        SimpleDateFormat formatter = new SimpleDateFormat("MMM");

        return formatter.format(new Date(timeInMillis));
    }

    private String getDayFromTimestamp(long timeInMillis) {
        SimpleDateFormat formatter = new SimpleDateFormat("dd");

        return formatter.format(new Date(timeInMillis));
    }

    private String getTimeFromTimestamp(long timeInMillis) {
        SimpleDateFormat formatter = new SimpleDateFormat("dd/MM");

        return formatter.format(new Date(timeInMillis));
    }

    @Override
    public String getFormattedValue(float value, AxisBase axis) {
        float xrange = axis.mAxisRange;

        //setLog("Value : " + value);
        if (value % mInterval == 0 && value >= 0) {
            return mValues[(int) value % mValues.length];
        } else
            return "";

    }
}
package com.ibl.apps.util;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.AttributeSet;

import com.ibl.apps.Model.ProgressItem;

import java.util.ArrayList;

/**
 * Created by iblinfotech on 26/10/18.
 */

public class CustomProgressBar extends androidx.appcompat.widget.AppCompatSeekBar {

    private ArrayList<ProgressItem> mProgressItemsList;

    public CustomProgressBar(Context context) {
        super(context);
        mProgressItemsList = new ArrayList<>();
    }

    public CustomProgressBar(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public CustomProgressBar(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }

    public void initData(ArrayList<ProgressItem> progressItemsList) {
        this.mProgressItemsList = progressItemsList;
    }

    @Override
    protected synchronized void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {
        super.onMeasure(widthMeasureSpec, heightMeasureSpec);
    }

    protected void onDraw(Canvas canvas) {

        if (mProgressItemsList != null) {
            if (mProgressItemsList.size() > 0) {

                int progressBarWidth = getWidth();
                int progressBarHeight = getHeight();
                int thumboffset = getThumbOffset();
                int lastProgressX = 0;
                int progressItemWidth, progressItemRight;
                for (int i = 0; i < mProgressItemsList.size(); i++) {
                    ProgressItem progressItem = mProgressItemsList.get(i);
                    Paint progressPaint = new Paint();
                    progressPaint.setColor(getResources().getColor(progressItem.color));

                    progressItemWidth = (int) (progressItem.progressItemPercentage * progressBarWidth / 100);
                    progressItemRight = lastProgressX + progressItemWidth;

                    // for last item give right to progress item to the width
                    if (i == mProgressItemsList.size() - 1 && progressItemRight != progressBarWidth) {
                        progressItemRight = progressBarWidth;
                    }

                    Rect progressRect = new Rect();
                    progressRect.set(lastProgressX, thumboffset / 2, progressItemRight, progressBarHeight - thumboffset / 2);
                    canvas.drawRect(progressRect, progressPaint);
                    lastProgressX = progressItemRight;

                }
                super.onDraw(canvas);
            }
        }
    }
}

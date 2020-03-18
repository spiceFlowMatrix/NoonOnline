package com.ibl.apps.Utils.CustomView;

import android.content.Context;
import android.widget.TextView;

import com.github.mikephil.charting.components.MarkerView;
import com.ibl.apps.Model.parent.Chart;
import com.ibl.apps.noon.R;

public class CustomMarkerView extends MarkerView {

    private TextView tvContent;


    public CustomMarkerView(Context context, int layoutResource) {
        super(context, layoutResource);
        tvContent = findViewById(R.id.tvContent);
    }

    public void setmodel(Chart.Da da) {
        tvContent.setText("Date :" + da.getX());
    }

    public void setmodel(Chart.Mo mo) {
        tvContent.setText("Date :" + mo.getX());
    }
}
package com.ibl.apps.noon;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.ibl.apps.Adapter.CacheEventsDescriptionAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.noon.databinding.ActivityCacheEventsDescrptionBinding;

import java.lang.reflect.Type;

public class CacheEventsDescriptionActivity extends BaseActivity {
    ActivityCacheEventsDescrptionBinding binding;
    CacheEventsDescriptionAdapter cacheEventsDescriptionAdapter;
    private String getSyncAPIData;
    private SyncAPITable syncAPIArray = new SyncAPITable();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    protected int getContentView() {
        return R.layout.activity_cache_events_descrption;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityCacheEventsDescrptionBinding) getBindObj();
        setToolbar(binding.toolBar);
        showBackArrow("Pending Cache Events");

        if (getIntent() != null)
            getSyncAPIData = getIntent().getStringExtra("CacheEventsDetail");
        Gson gson = new Gson();
        Type listOfCourseType = new TypeToken<SyncAPITable>() {
        }.getType();
        syncAPIArray = gson.fromJson(getSyncAPIData, listOfCourseType);
        Log.e("syncAPIArray", "onViewReady: " + syncAPIArray.toString());

        binding.toolBar.setNavigationOnClickListener(view -> {
            finish();
        });

        if (syncAPIArray != null) {
            if (syncAPIArray.getApi_name() != null) {
                binding.txtCacheEventsName.setText(syncAPIArray.getApi_name());
            }

            if (syncAPIArray.getDescription() != null) {
                binding.txtCacheDescription.setText(syncAPIArray.getDescription());
            }

            if (syncAPIArray.getStatus() != null) {
                binding.txtStatus.setText(syncAPIArray.getStatus());
            }
        }


//        cacheEventsDescriptionAdapter = new CacheEventsDescriptionAdapter();
//        binding.rcVerticalLayout.rcVertical.setAdapter(cacheEventsDescriptionAdapter);
    }
}

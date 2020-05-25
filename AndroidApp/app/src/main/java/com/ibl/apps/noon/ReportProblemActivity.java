package com.ibl.apps.noon;

import android.content.Intent;
import android.content.SharedPreferences;
import android.database.Cursor;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Toast;

import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.GridLayoutManager;

import com.google.android.material.bottomsheet.BottomSheetBehavior;
import com.google.android.material.bottomsheet.BottomSheetDialog;
import com.google.gson.JsonArray;
import com.google.gson.JsonObject;
import com.ibl.apps.Adapter.ImageUploadAdapter;
import com.ibl.apps.Adapter.ImageUploadAdapterList;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.FeedbackManagement.FeedbackRepository;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.Model.feedback.FeedBack;
import com.ibl.apps.Model.feedback.FeedBackTaskDetail;
import com.ibl.apps.Model.feedback.FileData;
import com.ibl.apps.Model.feedback.FillesData;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.noon.databinding.FileSelectItemBinding;
import com.ibl.apps.noon.databinding.PickVideoFileBinding;
import com.ibl.apps.noon.databinding.ReportProblemActivityBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.util.Validator;
import com.theartofdev.edmodo.cropper.CropImage;

import java.io.File;
import java.util.ArrayList;
import java.util.Objects;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.ibl.apps.noon.AssignmentAddActivity.getMimeType;

public class ReportProblemActivity extends BaseActivity implements View.OnClickListener {
    ReportProblemActivityBinding binding;
    private ImageUploadAdapter imageUploadAdapter;
    private ArrayList<FillesData> images = new ArrayList<>();
    private String description = "";
    private BottomSheetDialog mBottomSheetDialog;
    int flagid;
    CompositeDisposable disposable = new CompositeDisposable();
    public static ArrayList<String> fileIdList = new ArrayList<>();
    private Long id;
    private ArrayList<FileData> filesArrayList = new ArrayList<>();
    private FeedbackRepository feedbackRepository;
    private String ErrorSync;


    @Override
    protected int getContentView() {
        return R.layout.report_problem_activity;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ReportProblemActivityBinding) getBindObj();
        feedbackRepository = new FeedbackRepository();
        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
        String userId = sharedPreferencesuser.getString("uid", "");
        if (fileIdList != null) {
            fileIdList.clear();
        }
        if (getIntent() != null) {
            flagid = getIntent().getIntExtra(Const.Flag, 0);
            //description = getIntent().getStringExtra(Const.description);
            id = getIntent().getLongExtra("id", 0);
        }
        if (id != null && id != 0) {
            callApiForFeedbackDeatil();
        }

//        if (description != null && !description.isEmpty()) {
//            binding.DescriptionLay.setVisibility(View.VISIBLE);
//            binding.txtDescription.setText(description);
//            binding.bottomLay.setVisibility(View.GONE);
//            binding.descrptionWrapperApp.setVisibility(View.GONE);
//            binding.generaldescrptionWrapper.setVisibility(View.GONE);
//            setButtonEnable(false);
//        } else {
//            setButtonEnable(true);
//            binding.DescriptionLay.setVisibility(View.GONE);
//            binding.bottomLay.setVisibility(View.VISIBLE);
//            binding.descrptionWrapperApp.setVisibility(View.VISIBLE);
//            binding.generaldescrptionWrapper.setVisibility(View.VISIBLE);
//        }

        setToolbar(binding.toolbarLayout.toolBar);
        showBackArrow(getResources().getString(R.string.submit_feedback));
        binding.toolbarLayout.cacheEventsStatusBtn.setVisibility(View.VISIBLE);

        SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
        SharedPreferences sharedPreferenceCache = getSharedPreferences("cacheStatus", MODE_PRIVATE);
        String flagStatus = sharedPreferenceCache.getString("FlagStatus", "");
        switch (flagStatus) {
            case "1":
                binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_pending);
                break;
            case "2":
                binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_error);
                break;
            case "3":
                binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_syncing);
                break;
            case "4":
                GlideApp.with(ReportProblemActivity.this)
                        .load(R.drawable.ic_cache_empty)
                        .error(R.drawable.ic_cache_empty)
                        .into(binding.toolbarLayout.cacheEventsStatusBtn);
                break;
        }

        binding.toolbarLayout.cacheEventsStatusBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(ReportProblemActivity.this, CacheEventsListActivity.class));
            }
        });


        if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() >= 50) {
            showHitLimitDialog(ReportProblemActivity.this);
        }
        flagPassDeails();
        setUpRecyclerView();
        setOnClickListner();

        binding.toolbarLayout.toolBar.setNavigationOnClickListener(view -> {
            if (isNetworkAvailable(ReportProblemActivity.this)) {
                finish();
                Intent intent1 = new Intent(ReportProblemActivity.this, FeedBackActivity.class);
                startActivity(intent1);
            } else {
                showNetworkAlert(ReportProblemActivity.this);
            }
        });
    }

    public void callApiForFeedbackDeatil() {
        disposable.add(feedbackRepository.getTaskFeedBackDetailsById(id)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<FeedBackTaskDetail>() {
                    @Override
                    public void onSuccess(FeedBackTaskDetail feedBackTaskDetail) {
                        if (feedBackTaskDetail != null) {
                            if (feedBackTaskDetail.getData() != null) {
                                binding.DescriptionLay.setVisibility(View.VISIBLE);
                                binding.bottomLay.setVisibility(View.GONE);
                                binding.rcVerticalFiles.rcVertical.setVisibility(View.VISIBLE);
                                binding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                                binding.descrptionWrapperApp.setVisibility(View.GONE);
                                binding.generaldescrptionWrapper.setVisibility(View.GONE);

                                if (!TextUtils.isEmpty(feedBackTaskDetail.getData().getDescription())) {
                                    binding.txtDescription.setText(feedBackTaskDetail.getData().getDescription());
                                }
                                if (feedBackTaskDetail.getData().getFiles() != null && feedBackTaskDetail.getData().getFiles().size() != 0) {
                                    for (int i = 0; i < feedBackTaskDetail.getData().getFiles().size(); i++) {
                                        filesArrayList.addAll(feedBackTaskDetail.getData().getFiles());

                                        GridLayoutManager gridLayoutManager = new GridLayoutManager(ReportProblemActivity.this, 4);
                                        binding.rcVerticalFiles.rcVertical.setLayoutManager(gridLayoutManager);
                                        ImageUploadAdapterList imageUploadAdapterList = new ImageUploadAdapterList(feedBackTaskDetail.getData().getFiles(), flagid);
                                        binding.rcVerticalFiles.rcVertical.setAdapter(imageUploadAdapterList);
                                    }
                                }
                                setButtonEnable(false);
                            }
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        e.printStackTrace();
                        Log.e("FeedBack||onError", "onError: " + e.getMessage());
                    }
                }));
    }

    private void setButtonEnable(boolean isEnable) {
        binding.cardCameraroll.setEnabled(isEnable);
        binding.cardCameraroll.setAlpha(isEnable ? 1f : 0.5f);
    }

    private void setOnClickListner() {
        binding.cardCameraroll.setOnClickListener(this::onClick);
        binding.txtSendFeedback.setOnClickListener(this);
    }

    private void setUpRecyclerView() {
        GridLayoutManager gridLayoutManager = new GridLayoutManager(ReportProblemActivity.this, 4);
        binding.rcVerticalLayout.rcVertical.setLayoutManager(gridLayoutManager);
        imageUploadAdapter = new ImageUploadAdapter(images, flagid);
        binding.rcVerticalLayout.rcVertical.setAdapter(imageUploadAdapter);
    }

    private void flagPassDeails() {
        if (flagid == 1) {
            String firstHint = getString(R.string.explain_what_happened);
//            binding.descrptionWrapperApp.setHint(firstHint + "\n" + secondHint);
//            binding.descrptionApp.setHint(firstHint + "\n" + secondHint);

            binding.tvTitle.setText(getResources().getString(R.string.something_isn_t_working_app));
            binding.generaldescrptionWrapper.setVisibility(View.GONE);
            binding.descrptionWrapperApp.setVisibility(View.VISIBLE);

        } else if (flagid == 7) {
            binding.tvTitle.setText(getResources().getString(R.string.general_feedback));
            binding.descrptionWrapperApp.setVisibility(View.GONE);
            binding.generaldescrptionWrapper.setVisibility(View.VISIBLE);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        Uri resultUri = null;
        if (resultCode == RESULT_CANCELED || data == null) {
            setSendButtonEnable(true);
            return;
        }
        FillesData fillesData1 = new FillesData();
        if (requestCode == CropImage.CROP_IMAGE_ACTIVITY_REQUEST_CODE) {
            CropImage.ActivityResult result = CropImage.getActivityResult(data);
            if (resultCode == RESULT_OK) {
                if (result != null) {
                    resultUri = result.getUri();
                    fillesData1.setFiletype(1);
                    fillesData1.setUri(resultUri);
                }

            } else if (resultCode == CropImage.CROP_IMAGE_ACTIVITY_RESULT_ERROR_CODE) {
                assert result != null;
                Exception error = result.getError();
                error.printStackTrace();
            }

        } else if (requestCode == Const.pickvideo) {
            Uri selectedImageUri = data.getData();
            resultUri = selectedImageUri;
            fillesData1.setFiletype(2);
            fillesData1.setUri(resultUri);
            // OI FILE Manager
            assert selectedImageUri != null;
            String selectedImagePath = getPath(selectedImageUri);

            // MEDIA GALLERY
            if (selectedImagePath != null) {
                resultUri = Uri.parse(selectedImagePath);

            }
        } else if (requestCode == Const.capturevideo) {
            String selectedVideoPath = getPath(data.getData());
            resultUri = Uri.parse(selectedVideoPath);
            fillesData1.setFiletype(2);
            fillesData1.setUri(resultUri);
        }

        images.add(fillesData1);
        if (imageUploadAdapter != null) {
            imageUploadAdapter.notifyDataSetChanged();
        } else {
            imageUploadAdapter = new ImageUploadAdapter(images, flagid);
            binding.rcVerticalLayout.rcVertical.setAdapter(imageUploadAdapter);
        }


        if (resultUri != null) {
            File file = new File(resultUri.getEncodedPath());
            RequestBody requestFile;
            int type = getMimeType(file, resultUri.getEncodedPath());
            switch (type) {
                case 1:
                    requestFile = RequestBody.create(MediaType.parse("application/pdf"), file);
                    break;
                case 2:
                    requestFile = RequestBody.create(MediaType.parse("video/*"), file);
                    break;
                case 3:
                    requestFile = RequestBody.create(MediaType.parse("image/*"), file);
                    break;
                case 6:
                case 7:
                case 8:
                    requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                    break;
                default:
                    // Error
                    Toast.makeText(this, "No file chooser", Toast.LENGTH_SHORT).show();
                    return;
            }
            callApiForFilesUploading(file, requestFile, type);
        }

    }


    private void callApiForFilesUploading(File file, RequestBody requestFile, int type) {
        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
        RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), String.valueOf(type));
        RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
        RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");
        RequestBody totalpages = RequestBody.create(MediaType.parse("text/plain"), "0");

        Call<RestResponse<FileUploadResponse>> call = feedbackRepository.uploadAssignmentFile(body, fileTypeId, duration, filesize, totalpages);
        call.enqueue(new Callback<RestResponse<FileUploadResponse>>() {
            @Override
            public void onResponse(Call<RestResponse<FileUploadResponse>> call, Response<RestResponse<FileUploadResponse>> response) {
                setSendButtonEnable(true);
                if (response.body() != null && response.body().getResponse_code().equalsIgnoreCase("0")) {
                    RestResponse<FileUploadResponse> fileUploadResponse = response.body();
                    if (fileUploadResponse.getData() != null && fileUploadResponse.getData().getId() != 0) {
                        fileIdList.add(String.valueOf(fileUploadResponse.getData().getId()));
                    }
                }
            }

            @Override
            public void onFailure(Call<RestResponse<FileUploadResponse>> call, Throwable t) {
                setSendButtonEnable(true);
                Log.e("FileUploadResponse", "onFailure: " + t.getMessage());
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (imageUploadAdapter != null) {
            imageUploadAdapter.notifyDataSetChanged();
        }
    }

    private void setSendButtonEnable(boolean isEnable) {
        binding.txtSendFeedback.setEnabled(isEnable);
        binding.txtSendFeedback.setAlpha(isEnable ? 1f : 0.5f);
    }

    public String getPath(Uri uri) {
        String[] projection = {MediaStore.Video.Media.DATA};
        Cursor cursor = getContentResolver().query(uri, projection, null, null, null);
        if (cursor != null) {
            // HERE YOU WILL GET A NULLPOINTER IF CURSOR IS NULL
            // THIS CAN BE, IF YOU USED OI FILE MANAGER FOR PICKING THE MEDIA
            int column_index = cursor.getColumnIndexOrThrow(MediaStore.Video.Media.DATA);
            cursor.moveToFirst();
            return cursor.getString(column_index);
        } else
            return null;
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.card_cameraroll:
                showFileSelect();
                //CropImage.activity().start(this);
                break;
            case R.id.select_video:
                mBottomSheetDialog.dismiss();
                setSendButtonEnable(false);
                showVideoSelect();
                break;
            case R.id.select_image:
                mBottomSheetDialog.dismiss();
                setSendButtonEnable(false);
                CropImage.activity().start(this);
                break;
            case R.id.select_video_camera:
                mBottomSheetDialog.dismiss();
                setSendButtonEnable(false);
                takeVideoFromCamera();
                break;
            case R.id.select_video_gallary:
                mBottomSheetDialog.dismiss();
                setSendButtonEnable(false);
                chooseVideoFromGallary();
                break;

            case R.id.txtSendFeedback:
                if (validateFields()) {
                    if (isNetworkAvailable(ReportProblemActivity.this)) {
                        String somethingnotWorkinApp = Objects.requireNonNull(binding.descrptionApp.getText()).toString().trim();
                        String generalFeedbackDescrption = Objects.requireNonNull(binding.generalDescrption.getText()).toString().trim();

                        if (flagid == 1) {
                            callApiForAddFeedBack(somethingnotWorkinApp);
                        } else if (flagid == 7) {
                            callApiForAddFeedBack(generalFeedbackDescrption);
                        }
                    } else {
                        showNetworkAlert(ReportProblemActivity.this);
                    }
                }
                break;
        }
    }

    private void takeVideoFromCamera() {
        Intent takeVideoIntent = new Intent(MediaStore.ACTION_VIDEO_CAPTURE);
        if (takeVideoIntent.resolveActivity(getPackageManager()) != null) {
            startActivityForResult(takeVideoIntent, Const.capturevideo);
        }
    }

    public void chooseVideoFromGallary() {
        Intent intent = new Intent(Intent.ACTION_PICK);
        intent.setType("video/*");
        startActivityForResult(Intent.createChooser(intent, "Select Video"), Const.pickvideo);
    }

    private void showFileSelect() {
        mBottomSheetDialog = new BottomSheetDialog(this);
        FileSelectItemBinding fileSelectItemBinding = DataBindingUtil.inflate(LayoutInflater.from(this), R.layout.file_select_item, null, false);
        fileSelectItemBinding.selectVideo.setOnClickListener(this);
        fileSelectItemBinding.selectImage.setOnClickListener(this);
        mBottomSheetDialog.setContentView(fileSelectItemBinding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) fileSelectItemBinding.getRoot().getParent());
        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(fileSelectItemBinding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
    }

    private void showVideoSelect() {
        mBottomSheetDialog = new BottomSheetDialog(this);
        PickVideoFileBinding pickVideoFileBinding = DataBindingUtil.inflate(LayoutInflater.from(this), R.layout.pick_video_file, null, false);
        pickVideoFileBinding.selectVideoCamera.setOnClickListener(this);
        pickVideoFileBinding.selectVideoGallary.setOnClickListener(this);
        mBottomSheetDialog.setContentView(pickVideoFileBinding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) pickVideoFileBinding.getRoot().getParent());
        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(pickVideoFileBinding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
    }

    private boolean validateFields() {
        //For Something not working Descrption Validation
        if (binding.descrptionWrapperApp.getVisibility() == View.VISIBLE) {
            if (!Validator.checkEmpty(binding.descrptionApp)) {
                hideKeyBoard(binding.descrptionApp);
                binding.descrptionWrapperApp.setError(getString(R.string.validation_enterDecription));
                return false;
            } else {
                hideKeyBoard(binding.descrptionApp);
                binding.descrptionWrapperApp.setErrorEnabled(false);
            }

//            if (!Validator.checkDescLimit(binding.descrptionApp)) {
//                hideKeyBoard(binding.descrptionApp);
//                binding.descrptionWrapperApp.setError(getString(R.string.validation_limitDecription));
//                return false;
//            } else {
//                hideKeyBoard(binding.descrptionApp);
//                binding.descrptionWrapperApp.setErrorEnabled(false);
//            }
        }

        //For general feedback Descrption Validation
        if (binding.generaldescrptionWrapper.getVisibility() == View.VISIBLE) {
            if (!Validator.checkEmpty(binding.generalDescrption)) {
                hideKeyBoard(binding.generalDescrption);
                binding.generaldescrptionWrapper.setError(getString(R.string.validation_enterDecription));
                return false;
            } else {
                hideKeyBoard(binding.generalDescrption);
                binding.generaldescrptionWrapper.setErrorEnabled(false);
            }


//            if (!Validator.checkDescLimit(binding.generalDescrption)) {
//                hideKeyBoard(binding.generalDescrption);
//                binding.generaldescrptionWrapper.setError(getString(R.string.validation_limitDecription));
//                return false;
//            } else {
//                hideKeyBoard(binding.generalDescrption);
//                binding.generaldescrptionWrapper.setErrorEnabled(false);
//            }
        }
        return true;
    }

    private void callApiForAddFeedBack(String descrption) {
        JsonArray jsonArray = new JsonArray();
        for (int i = 0; i < fileIdList.size(); i++) {
            jsonArray.add(Integer.valueOf(fileIdList.get(i)));
        }
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty(Const.title, binding.tvTitle.getText().toString().trim());
        jsonObject.addProperty(Const.description, descrption);
        jsonObject.addProperty(Const.categoryid, flagid);
        jsonObject.addProperty(Const.gradeid, 0);
        jsonObject.addProperty(Const.courseid, 0);
        jsonObject.addProperty(Const.lessonid, 0);
        jsonObject.addProperty(Const.chapterid, 0);
        jsonObject.addProperty(Const.time, "00:00");
        jsonObject.addProperty(Const.device, Build.MODEL);
        jsonObject.addProperty(Const.version, Const.var_androidOSversion);
        jsonObject.addProperty(Const.appversion, BuildConfig.VERSION_NAME);
        jsonObject.addProperty(Const.operatingsystem, Const.var_deviceType);
        jsonObject.add(Const.filesids, jsonArray);

        disposable.add(feedbackRepository.getAddFeedbackApp(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<FeedBack>() {
                    @Override
                    public void onSuccess(FeedBack feedBack) {
                        if (feedBack != null) {
                            if (feedBack.getResponseCode() != null && feedBack.getResponseCode().equals(0L)) {
                                Toast.makeText(ReportProblemActivity.this, feedBack.getMessage(), Toast.LENGTH_LONG).show();
                                finish();
                                startActivity(new Intent(ReportProblemActivity.this, FeedBackActivity.class));
                            }
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        e.printStackTrace();
                        Log.e("FeedBack||onError", "onError: " + e.getMessage());
                    }
                }));
    }

    @Override
    public void onBackPressed() {
        if (isNetworkAvailable(ReportProblemActivity.this)) {
            super.onBackPressed();
            finish();
            Intent intent1 = new Intent(ReportProblemActivity.this, FeedBackActivity.class);
            startActivity(intent1);
        } else {
            showNetworkAlert(ReportProblemActivity.this);
        }
    }
}

package com.ibl.apps.noon;

import android.Manifest;
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
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Toast;

import androidx.annotation.NonNull;
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
import com.ibl.apps.Model.feedback.ChapterData;
import com.ibl.apps.Model.feedback.CourseSpinner;
import com.ibl.apps.Model.feedback.FeedBack;
import com.ibl.apps.Model.feedback.FeedBackTaskDetail;
import com.ibl.apps.Model.feedback.FileData;
import com.ibl.apps.Model.feedback.FillesData;
import com.ibl.apps.Model.feedback.LessonData;
import com.ibl.apps.RoomDatabase.dao.syncAPIManagementDatabase.SyncAPIDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.SyncAPITable;
import com.ibl.apps.noon.databinding.ActivityFilesFeedbackBinding;
import com.ibl.apps.noon.databinding.FileSelectItemBinding;
import com.ibl.apps.noon.databinding.PickVideoFileBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.GlideApp;
import com.ibl.apps.util.Validator;
import com.theartofdev.edmodo.cropper.CropImage;

import java.io.File;
import java.util.ArrayList;
import java.util.List;
import java.util.Objects;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import pub.devrel.easypermissions.EasyPermissions;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.ibl.apps.noon.AssignmentAddActivity.getMimeType;
import static com.ibl.apps.noon.CacheEventsListActivity.isClick;

public class FilesFeedbackActivity extends BaseActivity implements View.OnClickListener {
    ActivityFilesFeedbackBinding binding;
    private ImageUploadAdapter imageUploadAdapter;
    private ArrayList<FillesData> images = new ArrayList<>();
    private List<FileData> filesArrayList = new ArrayList<>();
    private String description = "";
    private BottomSheetDialog mBottomSheetDialog;
    CompositeDisposable disposable = new CompositeDisposable();
    public static ArrayList<String> fileIdList = new ArrayList<>();
    int flagid;
    private int courseId;
    private int lessonid = 0;
    private int chpterId = 0;
    private Long id;
    private FeedbackRepository feedbackRepository;
    private String ErrorSync;

    @Override
    protected int getContentView() {
        return R.layout.activity_files_feedback;
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        binding = (ActivityFilesFeedbackBinding) getBindObj();
        feedbackRepository = new FeedbackRepository();
        SharedPreferences sharedPreferencesuser = getSharedPreferences("user", MODE_PRIVATE);
        String userId = sharedPreferencesuser.getString("uid", "");

        if (fileIdList != null) {
            fileIdList.clear();
        }

        if (getIntent() != null) {
            flagid = getIntent().getIntExtra(Const.Flag, 0);
//            description = getIntent().getStringExtra("description");
            id = getIntent().getLongExtra("id", 0);
        }
        if (id != null && id != 0) {
            callApiForFeedbackDeatil();
        }

//        if (description != null && !description.isEmpty()) {
//            binding.DescriptionLay.setVisibility(View.VISIBLE);
//            binding.bottomLay.setVisibility(View.GONE);
//            binding.descrptionWrapper.setVisibility(View.GONE);
//            binding.courseSpinner.setText(binding.courseSpinner.getText().toString().trim());
//            binding.lessonSpinner.setText(binding.lessonSpinner.getText().toString().trim());
//            binding.tvHour.setText(binding.tvHour.getText().toString().trim());
//            binding.tvMin.setText(binding.tvMin.getText().toString().trim());
//            binding.txtDescription.setText(description);
//
//            binding.courseSpinner.setEnabled(false);
//            binding.lessonSpinner.setEnabled(false);
//            binding.tvHour.setEnabled(false);
//            binding.tvMin.setEnabled(false);
//            setButtonEnable(false);
//        } else {
//            setButtonEnable(true);
//            binding.DescriptionLay.setVisibility(View.GONE);
//            binding.bottomLay.setVisibility(View.VISIBLE);
//            binding.descrptionWrapper.setVisibility(View.VISIBLE);
//        }

        setToolbar(binding.toolbarLayout.toolBar);
        showBackArrow(getResources().getString(R.string.submit_feedback));
        binding.toolbarLayout.cacheEventsStatusBtn.setVisibility(View.VISIBLE);
        SyncAPIDatabaseRepository syncAPIDatabaseRepository = new SyncAPIDatabaseRepository();
        List<SyncAPITable> syncAPITableList = syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId));

        for (int i = 0; i < syncAPITableList.size(); i++) {
            ErrorSync = syncAPITableList.get(i).getStatus();
        }
        if (syncAPITableList != null && syncAPITableList.size() != 0) {
            binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_pending);
        } else if (syncAPITableList == null && syncAPITableList.size() == 0) {
            GlideApp.with(FilesFeedbackActivity.this)
                    .load(R.drawable.ic_cache_empty)
                    .error(R.drawable.ic_cache_empty)
                    .into(binding.toolbarLayout.cacheEventsStatusBtn);
        } else if (isClick) {
            binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_syncing);
        } /*else if (ErrorSync.contains("Errored")) {
            binding.toolbarLayout.cacheEventsStatusBtn.setImageResource(R.drawable.ic_cache_error);
        }*/


        binding.toolbarLayout.cacheEventsStatusBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                startActivity(new Intent(FilesFeedbackActivity.this, CacheEventsListActivity.class));
            }
        });


        if (syncAPIDatabaseRepository.getSyncUserById(Integer.parseInt(userId)).size() >= 50) {
            showHitLimitDialog(FilesFeedbackActivity.this);
        }
        feedbackFlagPassDeails();
        setOnclickListner();
        setUpRecyclerView();
        binding.toolbarLayout.toolBar.setNavigationOnClickListener(v -> {
            if (isNetworkAvailable(FilesFeedbackActivity.this)) {
                finish();
                Intent intent1 = new Intent(FilesFeedbackActivity.this, FeedBackActivity.class);
                startActivity(intent1);
            } else {
                showNetworkAlert(FilesFeedbackActivity.this);
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
                                binding.descrptionWrapper.setVisibility(View.GONE);
                                binding.timeLay.setVisibility(View.GONE);
                                binding.txtTime.setVisibility(View.GONE);
                                binding.rcVerticalFiles.rcVertical.setVisibility(View.VISIBLE);
                                binding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);

                                if (!TextUtils.isEmpty(feedBackTaskDetail.getData().getDescription())) {
                                    binding.txtDescription.setText(feedBackTaskDetail.getData().getDescription());
                                }

                                if (feedBackTaskDetail.getData().getCourse() != null && feedBackTaskDetail.getData().getCourse().getmName() != null) {
                                    binding.courseSpinner.setText(feedBackTaskDetail.getData().getCourse().getmName());
                                    binding.courseSpinner.setClickable(false);
                                    binding.courseSpinner.setEnabled(false);
                                    binding.courseSpinner.setAdapter(null);
                                }

                                if (flagid == 2) {
                                    if (feedBackTaskDetail.getData().getLesson() != null && !TextUtils.isEmpty(feedBackTaskDetail.getData().getLesson().getmName())) {
                                        binding.lessonSpinner.setText(feedBackTaskDetail.getData().getLesson().getmName());
                                        binding.lessonSpinner.setClickable(false);
                                        binding.lessonSpinner.setEnabled(false);

                                        binding.hourArrowDown.setEnabled(false);
                                        binding.hourArrowDown.setClickable(false);

                                        binding.hourArrowUp.setEnabled(false);
                                        binding.hourArrowUp.setClickable(false);

                                        binding.minArrowDown.setEnabled(false);
                                        binding.minArrowDown.setClickable(false);

                                        binding.minArrowUp.setEnabled(false);
                                        binding.minArrowUp.setClickable(false);

                                        binding.lessonSpinner.setAdapter(null);
                                    }

                                    if (!TextUtils.isEmpty(feedBackTaskDetail.getData().getTime())) {
                                        binding.timeLay.setVisibility(View.VISIBLE);
                                        binding.txtTime.setVisibility(View.VISIBLE);
                                        String[] strings = feedBackTaskDetail.getData().getTime().split(":");
                                        binding.tvHour.setText(strings[0]);
                                        binding.tvMin.setText(strings[1]);
                                    }

                                } else if (flagid == 3) {
                                    if (feedBackTaskDetail.getData().getLesson() != null && !TextUtils.isEmpty(feedBackTaskDetail.getData().getLesson().getmName())) {
                                        binding.lessonSpinner.setText(feedBackTaskDetail.getData().getLesson().getmName());
                                        binding.lessonSpinner.setClickable(false);
                                        binding.lessonSpinner.setEnabled(false);
                                        binding.lessonSpinner.setAdapter(null);
                                    }

                                } else if (flagid == 4) {
                                    if (feedBackTaskDetail.getData().getLesson() != null && !TextUtils.isEmpty(feedBackTaskDetail.getData().getLesson().getmName())) {
                                        binding.lessonSpinner.setText(feedBackTaskDetail.getData().getLesson().getmName());
                                        binding.lessonSpinner.setClickable(false);
                                        binding.lessonSpinner.setEnabled(false);
                                        binding.lessonSpinner.setAdapter(null);
                                    }

                                } else if (flagid == 5) {
                                    if (feedBackTaskDetail.getData().getChapter() != null && !TextUtils.isEmpty(feedBackTaskDetail.getData().getChapter().getmName())) {
                                        binding.lessonSpinner.setText(feedBackTaskDetail.getData().getChapter().getmName());
                                        binding.lessonSpinner.setClickable(false);
                                        binding.lessonSpinner.setEnabled(false);
                                        binding.lessonSpinner.setAdapter(null);
                                    }

                                } else if (flagid == 6) {
                                    if (feedBackTaskDetail.getData().getChapter() != null && !TextUtils.isEmpty(feedBackTaskDetail.getData().getChapter().getmName())) {
                                        binding.lessonSpinner.setText(feedBackTaskDetail.getData().getChapter().getmName());
                                        binding.lessonSpinner.setClickable(false);
                                        binding.lessonSpinner.setEnabled(false);
                                        binding.lessonSpinner.setAdapter(null);
                                    }
                                }

                                if (feedBackTaskDetail.getData().getFiles() != null && feedBackTaskDetail.getData().getFiles().size() != 0) {
                                    for (int i = 0; i < feedBackTaskDetail.getData().getFiles().size(); i++) {
                                        filesArrayList.addAll(feedBackTaskDetail.getData().getFiles());

                                        GridLayoutManager gridLayoutManager = new GridLayoutManager(FilesFeedbackActivity.this, 4);
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

    private void callApiCourseSpinner() {
        List<String> courseCategories = new ArrayList<>();
        courseCategories.add(0, "Java");
        courseCategories.add(1, "Chemistry");
        courseCategories.add(2, "physics");
        courseCategories.add(3, "Maths");

        ArrayAdapter<String> courseDataAdapter = new ArrayAdapter<>(this, R.layout.include_spinner_item, courseCategories);
        courseDataAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
        binding.courseSpinner.setAdapter(courseDataAdapter);
    }

    private void setUpRecyclerView() {
        GridLayoutManager gridLayoutManager = new GridLayoutManager(FilesFeedbackActivity.this, 4);
        binding.rcVerticalLayout.rcVertical.setLayoutManager(gridLayoutManager);
        imageUploadAdapter = new ImageUploadAdapter(images, flagid);
        binding.rcVerticalLayout.rcVertical.setAdapter(imageUploadAdapter);
    }

    private void setOnclickListner() {
        binding.hourArrowUp.setOnClickListener(this::onClick);
        binding.hourArrowDown.setOnClickListener(this);
        binding.minArrowUp.setOnClickListener(this);
        binding.minArrowDown.setOnClickListener(this);
        binding.cardCameraroll.setOnClickListener(this);
        binding.txtSendClick.setOnClickListener(this);
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.hour_arrow_up:
                int current_hour_add = Integer.parseInt(binding.tvHour.getText().toString());
                if (current_hour_add < 60) {
                    current_hour_add++;
                }
                if (current_hour_add <= 9) {
                    binding.tvHour.setText(String.valueOf(0).concat(String.valueOf(current_hour_add)));
                } else {
                    binding.tvHour.setText(String.valueOf(current_hour_add));
                }
                break;

            case R.id.hour_arrow_down:
                int current_hour_sub = Integer.parseInt(binding.tvHour.getText().toString());
                if (current_hour_sub > 0) {
                    current_hour_sub--;
                }

                if (current_hour_sub <= 9) {
                    binding.tvHour.setText(String.valueOf(0).concat(String.valueOf(current_hour_sub)));
                } else {
                    binding.tvHour.setText(String.valueOf(current_hour_sub));
                }
                break;

            case R.id.min_arrow_up:
                int current_min_add = Integer.parseInt(binding.tvMin.getText().toString());
                if (current_min_add < 60) {
                    current_min_add++;
                }
                if (current_min_add <= 9) {
                    binding.tvMin.setText(String.valueOf(0).concat(String.valueOf(current_min_add)));
                } else {
                    binding.tvMin.setText(String.valueOf(current_min_add));
                }
                break;

            case R.id.min_arrow_down:
                int currentsub_min_sub = Integer.parseInt(binding.tvMin.getText().toString());
                if (currentsub_min_sub > 0) {
                    currentsub_min_sub--;
                }
                if (currentsub_min_sub <= 9) {
                    binding.tvMin.setText(String.valueOf(0).concat(String.valueOf(currentsub_min_sub)));
                } else {
                    binding.tvMin.setText(String.valueOf(currentsub_min_sub));
                }
                break;

            case R.id.card_cameraroll:
                showFileSelect();
//                CropImage.activity().start(this);
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
                if (!EasyPermissions.hasPermissions(FilesFeedbackActivity.this, Manifest.permission.CAMERA)) {
                    EasyPermissions.requestPermissions(this, NoonApplication.getContext().getResources().getString(R.string.validation_download_permission), 0x01, Manifest.permission.CAMERA);
                } else {
                    takeVideoFromCamera();
                }
                break;

            case R.id.select_video_gallary:
                mBottomSheetDialog.dismiss();
                setSendButtonEnable(false);
                chooseVideoFromGallary();
                break;

            case R.id.txtSendClick:
                if (validateFields()) {
                    if (isNetworkAvailable(FilesFeedbackActivity.this)) {
                        String descrption = Objects.requireNonNull(binding.descrption.getText()).toString().trim();
                        String hour = binding.tvHour.getText().toString().trim();
                        String minute = binding.tvMin.getText().toString().trim();

                        String time = hour + ":" + minute;
                        callApiForAddFeedBack(descrption, time);
                    } else {
                        showNetworkAlert(FilesFeedbackActivity.this);
                    }
                }
                break;
        }
    }

    private void callApiForCourseSpinner() {
        disposable.add(feedbackRepository.getAllCourseByUser()
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<CourseSpinner>() {
                    @Override
                    public void onSuccess(CourseSpinner courseSpinner) {
                        if (courseSpinner != null && courseSpinner.getData() != null) {
                            ArrayAdapter<CourseSpinner.Data> courseAdapter = new ArrayAdapter<>(NoonApplication.getContext(), R.layout.include_spinner_item, courseSpinner.getData());
                            courseAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
                            binding.courseSpinner.setAdapter(courseAdapter);
                            binding.courseSpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                                @Override
                                public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                                    courseId = Integer.parseInt(String.valueOf(courseSpinner.getData().get(i).getId()));
                                    if (flagid == 2 || flagid == 3 || flagid == 4) {
                                        callApiForLesson(courseId);
                                    } else if (flagid == 5 || flagid == 6) {
                                        callApiForChapter(courseId);
                                    }
                                }
                            });
                            hideDialog();
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }

    private void callApiForChapter(int courseId) {
        disposable.add(feedbackRepository.getChapterByCourse(courseId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<ChapterData>() {
                    @Override
                    public void onSuccess(ChapterData chapterData) {
                        if (chapterData != null && chapterData.getData() != null) {
                            ArrayAdapter<ChapterData.Data> courseAdapter = new ArrayAdapter<>(NoonApplication.getContext(), R.layout.include_spinner_item, chapterData.getData());
                            courseAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
                            binding.lessonSpinner.setAdapter(courseAdapter);
                            binding.lessonSpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                                @Override
                                public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                                    chpterId = Integer.parseInt(chapterData.getData().get(i).getId());
                                }
                            });
                            hideDialog();
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));
    }

    private void callApiForLesson(int courseId) {
        disposable.add(feedbackRepository.getLessonByCourse(courseId)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<LessonData>() {
                    @Override
                    public void onSuccess(LessonData lessonData) {
                        if (lessonData != null && lessonData.getData() != null) {
                            ArrayAdapter<LessonData.Data> courseAdapter = new ArrayAdapter<>(NoonApplication.getContext(), R.layout.include_spinner_item, lessonData.getData());
                            courseAdapter.setDropDownViewResource(R.layout.include_spinner_popup_item);
                            binding.lessonSpinner.setAdapter(courseAdapter);
                            binding.lessonSpinner.setOnItemClickListener(new AdapterView.OnItemClickListener() {
                                @Override
                                public void onItemClick(AdapterView<?> adapterView, View view, int i, long l) {
                                    lessonid = Integer.parseInt(lessonData.getData().get(i).getId());
                                }
                            });
                            hideDialog();
                        }
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                    }
                }));

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
        startActivityForResult(Intent.createChooser(intent, getResources().getString(R.string.select_video)), Const.pickvideo);
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
        pickVideoFileBinding.selectVideoGallary.setOnClickListener(this::onClick);

        mBottomSheetDialog.setContentView(pickVideoFileBinding.getRoot());
        BottomSheetBehavior mBehavior = BottomSheetBehavior.from((View) pickVideoFileBinding.getRoot().getParent());
        mBottomSheetDialog.setOnShowListener(dialogInterface -> {
            mBehavior.setPeekHeight(pickVideoFileBinding.getRoot().getHeight()); //get the height dynamically
        });
        mBottomSheetDialog.show();
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, @Nullable Intent data) {
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
            public void onResponse(@NonNull Call<RestResponse<FileUploadResponse>> call, @NonNull Response<RestResponse<FileUploadResponse>> response) {
                setSendButtonEnable(true);
                if (response.body() != null && response.body().getResponse_code().equalsIgnoreCase("0")) {
                    RestResponse<FileUploadResponse> fileUploadResponse = response.body();
                    if (fileUploadResponse.getData() != null && fileUploadResponse.getData().getId() != 0) {
                        fileIdList.add(String.valueOf(fileUploadResponse.getData().getId()));
                    }
                }
            }

            @Override
            public void onFailure(@NonNull Call<RestResponse<FileUploadResponse>> call, @NonNull Throwable t) {
                setSendButtonEnable(true);
                Log.e("onFailure", "onFailure: " + t.getMessage());
            }
        });
    }

    private void setSendButtonEnable(boolean isEnable) {
        binding.txtSendClick.setEnabled(isEnable);
        binding.txtSendClick.setAlpha(isEnable ? 1f : 0.5f);
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

    private void feedbackFlagPassDeails() {
        callApiForCourseSpinner();
        if (flagid == 2) {
            binding.txtTime.setVisibility(View.VISIBLE);
            binding.timeLay.setVisibility(View.VISIBLE);
            binding.wrraperSpinnerLesson.setHint(getResources().getString(R.string.select_lesson));
            binding.tvTitle.setText(getResources().getString(R.string.video_issue));

        } else if (flagid == 3) {
            binding.txtTime.setVisibility(View.GONE);
            binding.timeLay.setVisibility(View.GONE);
            binding.wrraperSpinnerLesson.setHint(getResources().getString(R.string.select_lesson));
            binding.tvTitle.setText(getResources().getString(R.string.text_issue));

        } else if (flagid == 4) {
            binding.txtTime.setVisibility(View.GONE);
            binding.timeLay.setVisibility(View.GONE);
            binding.wrraperSpinnerLesson.setHint(getResources().getString(R.string.select_lesson));
            binding.tvTitle.setText(getResources().getString(R.string.lesson_assignment_issue));

        } else if (flagid == 5) {
            binding.txtTime.setVisibility(View.GONE);
            binding.timeLay.setVisibility(View.GONE);
            binding.wrraperSpinnerLesson.setHint(getResources().getString(R.string.select_chapterHint));
            binding.tvTitle.setText(getResources().getString(R.string.chapter_assignment_issue));

        } else if (flagid == 6) {
            binding.txtTime.setVisibility(View.GONE);
            binding.timeLay.setVisibility(View.GONE);
            binding.wrraperSpinnerLesson.setHint(getResources().getString(R.string.select_chapterHint));
            binding.tvTitle.setText(getResources().getString(R.string.quiz_issue));
        }
    }

    private void callApiForAddFeedBack(String descrption, String time) {
        JsonArray jsonArray = new JsonArray();
        for (int i = 0; i < fileIdList.size(); i++) {
            jsonArray.add(Integer.valueOf(fileIdList.get(i)));
        }
        JsonObject jsonObject = new JsonObject();
        jsonObject.addProperty(Const.title, binding.tvTitle.getText().toString().trim());
        jsonObject.addProperty(Const.description, descrption);
        jsonObject.addProperty(Const.categoryid, flagid);
        jsonObject.addProperty(Const.gradeid, 0);
        jsonObject.addProperty(Const.chapterid, chpterId);
        jsonObject.addProperty(Const.courseid, courseId);
        jsonObject.addProperty(Const.lessonid, lessonid);
        jsonObject.addProperty(Const.time, time);
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
                                Toast.makeText(FilesFeedbackActivity.this, feedBack.getMessage(), Toast.LENGTH_LONG).show();
                                finish();
                                startActivity(new Intent(FilesFeedbackActivity.this, FeedBackActivity.class));
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


    private boolean validateFields() {
        if (binding.wrraperSpinnerCourse.getVisibility() == View.VISIBLE) {
            if (!Validator.checkEmpty(binding.courseSpinner)) {
                hideKeyBoard(binding.courseSpinner);
                binding.wrraperSpinnerCourse.setError(getString(R.string.validation_course_select));
                return false;
            } else {
                hideKeyBoard(binding.courseSpinner);
                binding.wrraperSpinnerCourse.setErrorEnabled(false);
            }
        }

        if (flagid == 2 || flagid == 3 || flagid == 4) {
            if (binding.wrraperSpinnerLesson.getVisibility() == View.VISIBLE) {
                if (!Validator.checkEmpty(binding.lessonSpinner)) {
                    hideKeyBoard(binding.lessonSpinner);
                    binding.wrraperSpinnerLesson.setError(getString(R.string.validation_lesson_select));
                    return false;
                } else {
                    hideKeyBoard(binding.lessonSpinner);
                    binding.wrraperSpinnerLesson.setErrorEnabled(false);
                }
            }
        } else if (flagid == 5 || flagid == 6) {
            if (binding.wrraperSpinnerLesson.getVisibility() == View.VISIBLE) {
                if (!Validator.checkEmpty(binding.lessonSpinner)) {
                    hideKeyBoard(binding.lessonSpinner);
                    binding.wrraperSpinnerLesson.setError(getString(R.string.validation_chapter_select));
                    return false;
                } else {
                    hideKeyBoard(binding.lessonSpinner);
                    binding.wrraperSpinnerLesson.setErrorEnabled(false);
                }
            }
        }

        if (binding.timeLay.getVisibility() == View.VISIBLE) {
            if (!Validator.checkEmptyTextview(binding.tvHour)) {
                hideKeyBoard(binding.tvHour);
                binding.viewHour.setBackgroundColor(getResources().getColor(R.color.colorRed));
                return false;
            } else {
                binding.viewHour.setBackgroundColor(getResources().getColor(R.color.colorPrimary));
                hideKeyBoard(binding.tvHour);
            }

            if (!Validator.checkEmptyTextview(binding.tvMin)) {
                hideKeyBoard(binding.tvMin);
                binding.viewMin.setBackgroundColor(getResources().getColor(R.color.colorRed));
                return false;
            } else {
                binding.viewMin.setBackgroundColor(getResources().getColor(R.color.colorPrimary));
                hideKeyBoard(binding.tvHour);
            }
        }

        //For Descrption Validation
        if (!Validator.checkEmpty(binding.descrption)) {
            hideKeyBoard(binding.descrption);
            binding.descrptionWrapper.setError(getString(R.string.validation_enterDecription));
            return false;
        } else {
            hideKeyBoard(binding.descrption);
            binding.descrptionWrapper.setErrorEnabled(false);
        }

//        if (!Validator.checkDescLimit(binding.descrption)) {
//            hideKeyBoard(binding.descrption);
//            binding.descrptionWrapper.setError(getString(R.string.validation_limitDecription));
//            return false;
//        } else {
//            hideKeyBoard(binding.descrption);
//            binding.descrptionWrapper.setErrorEnabled(false);
//        }
        return true;
    }

    @Override
    protected void onResume() {
        super.onResume();
        if (imageUploadAdapter != null) {
            imageUploadAdapter.notifyDataSetChanged();
        }
    }

    @Override
    public void onBackPressed() {
        if (isNetworkAvailable(FilesFeedbackActivity.this)) {
            super.onBackPressed();
            finish();
            Intent intent1 = new Intent(FilesFeedbackActivity.this, FeedBackActivity.class);
            startActivity(intent1);
        } else {
            showNetworkAlert(FilesFeedbackActivity.this);
        }
    }
}

package com.ibl.apps.noon;

import android.Manifest;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.os.AsyncTask;
import android.os.Bundle;
import androidx.annotation.NonNull;
import androidx.core.app.ActivityCompat;
import androidx.core.content.ContextCompat;
import android.text.TextUtils;
import android.util.AttributeSet;
import android.view.MenuItem;
import android.view.View;
import android.widget.CompoundButton;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.ibl.apps.Adapter.DiscussionFIleUploadListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.DiscussionManagement.DiscussionRepository;
import com.ibl.apps.Model.AddDiscussionTopic;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.Validator;
import com.ibl.apps.noon.databinding.DiscussionsAddLayoutBinding;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.util.ArrayList;
import java.util.Arrays;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import ir.sohreco.androidfilechooser.ExternalStorageNotAvailableException;
import ir.sohreco.androidfilechooser.FileChooser;
import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import okhttp3.ResponseBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.HttpException;

import static com.ibl.apps.util.Const.GradeID;


public class GeneralDiscussionsAddActivity extends BaseActivity implements View.OnClickListener {

    DiscussionsAddLayoutBinding discussionsAddLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();
    private final static int READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE = 13;
    ArrayList<String> fileuploadlist = new ArrayList<>();

    UserDetails userDetailsObject = new UserDetails();
    String userId = "0";
    FileChooser.Builder builder;
    boolean isprivate = false;
    DiscussionFIleUploadListAdapter discussionFIleUploadListAdapter;
    String GradeId, CourseName, ActivityFlag, LessonID, QuizID;
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";
    private DiscussionRepository discussionRepository;

    @Override
    protected int getContentView() {
        return R.layout.discussions_add_layout;
    }

    @Override
    public View onCreateView(String name, Context context, AttributeSet attrs) {
        return super.onCreateView(name, context, attrs);
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        discussionsAddLayoutBinding = (DiscussionsAddLayoutBinding) getBindObj();
        discussionRepository = new DiscussionRepository();

        if (getIntent() != null) {
            GradeId = getIntent().getStringExtra(GradeID);
            CourseName = getIntent().getStringExtra(Const.CourseName);
            ActivityFlag = getIntent().getStringExtra(Const.ActivityFlag);
            LessonID = getIntent().getStringExtra(Const.LessonID);
            QuizID = getIntent().getStringExtra(Const.QuizID);
            AddtionalLibrary = getIntent().getStringExtra(Const.AddtionalLibrary);
            AddtionalAssignment = getIntent().getStringExtra(Const.AddtionalAssignment);
            AddtionalDiscussions = getIntent().getStringExtra(Const.AddtionalDiscussions);
        }


        fileuploadlist.clear();

        discussionsAddLayoutBinding.mainDicussionsAddLay.setVisibility(View.VISIBLE);
        discussionsAddLayoutBinding.filechooserFragment.setVisibility(View.GONE);
        discussionsAddLayoutBinding.laySwitch.setVisibility(View.GONE);

        PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
            @Override
            public UserDetails getLocalUserDetails(UserDetails userDetails) {
                if (userDetails != null) {
                    userDetailsObject = userDetails;
                    userId = userDetailsObject.getId();

                }
                return null;
            }

        }).execute();
        setToolbar(discussionsAddLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.item_4));
        setOnClickListener();
    }

    public void setOnClickListener() {
        discussionsAddLayoutBinding.cardAddFilePicker.setOnClickListener(this);
        discussionsAddLayoutBinding.cardAddSubmit.setOnClickListener(this);

        discussionsAddLayoutBinding.shareSwitch.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
                isprivate = isChecked;
            }
        });
    }

    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.cardAddFilePicker:

                int permissionCheck = ContextCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE);
                if (permissionCheck != PackageManager.PERMISSION_GRANTED) {
                    ActivityCompat.requestPermissions(this,
                            new String[]{Manifest.permission.READ_EXTERNAL_STORAGE},
                            READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE);
                } else {
                    addFileChooserFragment();
                }
                break;
            case R.id.cardAddSubmit:

                String title = discussionsAddLayoutBinding.discussionsTopicName.getText().toString().trim();
                String description = discussionsAddLayoutBinding.discussionsTopicdes.getText().toString().trim();
                if (fileuploadlist != null && fileuploadlist.size() != 0) {

                    if (validateFields()) {
                        if (isNetworkAvailable(this)) {
                            new setFileWithDataTask(new asynDelegate() {
                                @Override
                                public void asynDelegateMethod(ArrayList<Integer> strings) {
                                    CallApiAddDiscussions(title, description, isprivate, strings);
                                }
                            }).execute();
                        }
                    }

                } else {
                    if (validateFields()) {
                        if (isNetworkAvailable(this)) {
                            CallApiAddDiscussions(title, description, isprivate, new ArrayList<>());
                        } else {
                            showNetworkAlert(GeneralDiscussionsAddActivity.this);
                        }
                    }
                }
                break;
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode,
                                           @NonNull String[] permissions,
                                           @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                addFileChooserFragment();
            }
        }
    }

    private void addFileChooserFragment() {

        discussionsAddLayoutBinding.filechooserFragment.setVisibility(View.VISIBLE);

        builder = new FileChooser.Builder(FileChooser.ChooserType.FILE_CHOOSER,
                new FileChooser.ChooserListener() {
                    @Override
                    public void onSelect(String path) {
                        discussionsAddLayoutBinding.filechooserFragment.setVisibility(View.GONE);
                        if (!TextUtils.isEmpty(path)) {
                            String[] selectedFilePaths = path.split(FileChooser.FILE_NAMES_SEPARATOR);

                            fileuploadlist.clear();
                            fileuploadlist.addAll(Arrays.asList(selectedFilePaths));
                            discussionFIleUploadListAdapter = new DiscussionFIleUploadListAdapter(getApplicationContext(), fileuploadlist, userDetailsObject);
                            discussionsAddLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(discussionFIleUploadListAdapter);
                            discussionFIleUploadListAdapter.notifyDataSetChanged();
                            //Log.e("selectedFilePaths", "==fileUploadList=00=" + fileUploadList);
                        } else {
                            Toast.makeText(GeneralDiscussionsAddActivity.this, R.string.no_file_chosen, Toast.LENGTH_SHORT).show();
                        }
                    }
                })

                //.setMultipleFileSelectionEnabled(true)
                // Word document,PDF file,Powerpoint file,Excel file,GIF file,JPG file ,PNG file,Text file ,Video files
                .setFileFormats(new String[]{".noon", ".jpg", ".png", ".jpeg", ".gif", ".3gp", ".mpg", ".mpeg", ".mpe", ".mp4", ".avi", ".wav", ".mp3", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".docx", ".doc"});

        try {
            getSupportFragmentManager().beginTransaction().replace(R.id.filechooserFragment, builder.build()).commit();
        } catch (ExternalStorageNotAvailableException e) {
            Toast.makeText(this, R.string.no_external_storage, Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }

    @Override
    public void onBackPressed() {
       /* Fragment f = getSupportFragmentManager().findFragmentById(R.id.filechooserFragment);
        if (f instanceof FileChooser) {
            discussionsAddLayoutBinding.filechooserFragment.setVisibility(View.GONE);
            getSupportFragmentManager().beginTransaction()
                    .remove(f)
                    .commit();
        } else {
            finish();
        }*/


        PrivousScreen();
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                onBackPressed();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    private void CallApiAddDiscussions(String title, String description, boolean isprivate, ArrayList<Integer> filesidArraylist) {

        JsonObject gsonObject = new JsonObject();
        JSONObject jsonObject = new JSONObject();
        try {
            //jsonObject.put(Const.id, "0");
            jsonObject.put(Const.title, title);
            jsonObject.put(Const.description, description);
            jsonObject.put(Const.isprivate, isprivate);
            jsonObject.put(Const.filesid, new JSONArray(filesidArraylist));
            jsonObject.put(Const.courseid, GradeId);
            jsonObject.put(Const.ispublic, true);

        } catch (JSONException e) {
            e.printStackTrace();
        }

        JsonParser jsonParser = new JsonParser();
        gsonObject = (JsonObject) jsonParser.parse(jsonObject.toString());

        showDialog(getString(R.string.loading));
        disposable.add(discussionRepository.AddDiscussionTopic(gsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<AddDiscussionTopic>() {
                    @Override
                    public void onSuccess(AddDiscussionTopic addDiscussionTopic) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                        PrivousScreen();
                    }

                    @Override
                    public void onError(Throwable e) {

                        hideDialog();
                        try {
                            HttpException error = (HttpException) e;
                            AddDiscussionTopic addDiscussionTopic = new Gson().fromJson(error.response().errorBody().string(), AddDiscussionTopic.class);
                            Toast.makeText(getApplicationContext(), addDiscussionTopic.getMessage(), Toast.LENGTH_SHORT).show();
                            //showSnackBar(fragmentCourseItemLayoutBinding.mainFragmentCourseLayout, quizMainObject.getMessage());
                        } catch (Exception e1) {
                            showError(e);
                        }
                    }
                }));
    }

    private boolean validateFields() {
        if (!Validator.checkEmpty(discussionsAddLayoutBinding.discussionsTopicName)) {
            hideKeyBoard(discussionsAddLayoutBinding.discussionsTopicName);
            discussionsAddLayoutBinding.discussionsTopicNameWrapper.setError(getString(R.string.validation_enter_title));
            return false;
        } else {
            hideKeyBoard(discussionsAddLayoutBinding.discussionsTopicName);
            discussionsAddLayoutBinding.discussionsTopicNameWrapper.setErrorEnabled(false);
        }
        return true;
    }

    public void PrivousScreen() {
        finish();
        Intent i = new Intent(getApplicationContext(), MainDashBoardActivity.class);
        i.putExtra(Const.isDiscussions, true);
        startActivity(i);

    }

    public class setFileWithDataTask extends AsyncTask<Void, Void, ArrayList<Integer>> {

        asynDelegate delegate;

        public setFileWithDataTask(asynDelegate delegate) {
            this.delegate = delegate;
        }

        @Override
        protected void onPreExecute() {
            showDialog(getString(R.string.loading));
            super.onPreExecute();
        }

        @Override
        protected ArrayList<Integer> doInBackground(Void... params) {
            ArrayList<Integer> filesidArraylist = new ArrayList<>();

            for (int i = 0; i < fileuploadlist.size(); i++) {
                File file = new File(fileuploadlist.get(i));
                byte[] bitmapImage = new byte[(int) file.length()];
                if (bitmapImage != null) {
                    if (isNetworkAvailable(GeneralDiscussionsAddActivity.this)) {
                        //Log.e("selectedFilePaths", "BITMAP" + bitmapImage);

                        RequestBody requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
                        RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), "1");
                        RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
                        RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");

                        Call<UploadTopicFile> call = discussionRepository.UploadTopicFile(body, fileTypeId, duration, filesize);
                        call.enqueue(new Callback<UploadTopicFile>() {
                            @Override
                            public void onResponse(Call<UploadTopicFile> call, retrofit2.Response<UploadTopicFile> response) {

                                if (response.isSuccessful()) {
                                    UploadTopicFile uploadImageObject = response.body();
                                    filesidArraylist.add(Integer.valueOf(uploadImageObject.getFile_id()));
                                    delegate.asynDelegateMethod(filesidArraylist);

                                } else {
                                    ResponseBody errorBody = response.errorBody();
                                    Gson gson = new Gson();
                                    try {
                                        UploadTopicFile uploadTopicFile = gson.fromJson(errorBody.string(), UploadTopicFile.class);
                                        //Toast.makeText(getApplicationContext(), uploadTopicFile.getMessage(), Toast.LENGTH_LONG).show();

                                    } catch (Exception e) {
                                        e.printStackTrace();
                                        showError(e);
                                    }
                                }
                            }

                            @Override
                            public void onFailure(Call<UploadTopicFile> call, Throwable t) {
                                hideDialog();
                            }
                        });

                    }
                }
            }
            return filesidArraylist;
        }

        @Override
        protected void onPostExecute(ArrayList<Integer> strings) {

        }
    }

    public interface asynDelegate {
        public void asynDelegateMethod(ArrayList<Integer> strings);
    }


}

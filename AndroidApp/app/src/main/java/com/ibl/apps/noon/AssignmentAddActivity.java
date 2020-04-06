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
import android.util.Log;
import android.view.MenuItem;
import android.view.View;
import android.webkit.MimeTypeMap;
import android.widget.Toast;

import com.google.gson.Gson;
import com.google.gson.JsonObject;
import com.google.gson.JsonParser;
import com.google.gson.reflect.TypeToken;
import com.ibl.apps.Adapter.AssignmentFileUploadListAdapter;
import com.ibl.apps.Base.BaseActivity;
import com.ibl.apps.Model.CoursePriviewObject;
import com.ibl.apps.Model.RestResponse;
import com.ibl.apps.Model.UploadTopicFile;
import com.ibl.apps.Model.assignment.AssignmentData;
import com.ibl.apps.Model.assignment.FileUploadResponse;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;
import com.ibl.apps.util.Validator;
import com.ibl.apps.noon.databinding.AssignmentAddLayoutBinding;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;

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
import retrofit2.Response;


public class AssignmentAddActivity extends BaseActivity implements View.OnClickListener {

    private final static int READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE = 13;
    AssignmentAddLayoutBinding assignmentAddLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    public static ArrayList<String> fileUploadList = new ArrayList<>();
    UserDetails userDetailsObject;
    String userId = "0";
    FileChooser.Builder builder;

    //    boolean isprivate = false;
    AssignmentFileUploadListAdapter fileUploadAdapter;
    CoursePriviewObject.Assignment assignment;
    int flag;

    @Override
    protected int getContentView() {
        return R.layout.assignment_add_layout;
    }


    @Override
    public View onCreateView(String name, Context context, AttributeSet attrs) {
        return super.onCreateView(name, context, attrs);
    }

    @Override
    protected void onViewReady(Bundle savedInstanceState, Intent intent) {
        super.onViewReady(savedInstanceState, intent);
        assignmentAddLayoutBinding = (AssignmentAddLayoutBinding) getBindObj();

        flag = getIntent().getIntExtra(Const.Flag, 0);
        assignment = new Gson().fromJson(getIntent().getStringExtra(Const.Assignment), new TypeToken<CoursePriviewObject.Assignment>() {
        }.getType());


        fileUploadList.clear();
        assignmentAddLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
        assignmentAddLayoutBinding.mainAssignmentAddLay.setVisibility(View.VISIBLE);
        assignmentAddLayoutBinding.filechooserFragment.setVisibility(View.GONE);

//        assignmentAddLayoutBinding.laySwitch.setVisibility(View.GONE);

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
        setToolbar(assignmentAddLayoutBinding.toolbarLayout.toolBar);
        showBackArrow(getString(R.string.item_2));
        fileUploadAdapter = new AssignmentFileUploadListAdapter();
        assignmentAddLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(fileUploadAdapter);
        setOnClickListener();
    }

    public void setOnClickListener() {
        assignmentAddLayoutBinding.cardAddFilePicker.setOnClickListener(this);
        assignmentAddLayoutBinding.cardAddSubmit.setOnClickListener(this);
//        assignmentAddLayoutBinding.shareSwitch.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
//            public void onCheckedChanged(CompoundButton buttonView, boolean isChecked) {
//                isprivate = isChecked;
//            }
//        });
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
                if (fileUploadList != null && fileUploadList.size() != 0) {
                    if (isNetworkAvailable(this)) {
                        createdAssigmentSubmission();
                    } else {
                        showNetworkAlert(AssignmentAddActivity.this);
                    }
                }
                break;
        }
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        if (requestCode == READ_EXTERNAL_STORAGE_PERMISSION_REQUEST_CODE) {
            if (grantResults.length > 0 && grantResults[0] == PackageManager.PERMISSION_GRANTED) {
                addFileChooserFragment();
            }
        }
    }

    private void addFileChooserFragment() {
        assignmentAddLayoutBinding.filechooserFragment.setVisibility(View.VISIBLE);
        builder = new FileChooser.Builder(FileChooser.ChooserType.FILE_CHOOSER,
                (FileChooser.ChooserListener) path -> {
                    assignmentAddLayoutBinding.filechooserFragment.setVisibility(View.GONE);
                    assignmentAddLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.VISIBLE);
                    if (!TextUtils.isEmpty(path)) {
                        String[] selectedFilePaths = path.split(FileChooser.FILE_NAMES_SEPARATOR);
                        fileUploadAdapter.addFile(selectedFilePaths[0]);
                        uploadFile(selectedFilePaths[0]);
                    } else {
                        Toast.makeText(AssignmentAddActivity.this, R.string.no_file_chosen, Toast.LENGTH_SHORT).show();
                    }
                })

                //.setMultipleFileSelectionEnabled(true)
                // Word document,PDF file,Powerpoint file,Excel file,GIF file,JPG file ,PNG file,Text file ,Video files
                .setFileFormats(new String[]{".jpg", ".png", ".jpeg", ".gif", ".3gp", ".mpg", ".mpeg", ".mpe", ".mp4", ".avi", ".txt", ".xls", ".xlsx", ".ppt", ".pptx", ".pdf", ".docx", ".doc"});

        try {
            getSupportFragmentManager().beginTransaction().replace(R.id.filechooserFragment, builder.build()).commit();
        } catch (ExternalStorageNotAvailableException e) {
            Toast.makeText(this, R.string.no_external_storage, Toast.LENGTH_SHORT).show();
            e.printStackTrace();
        }
    }

    private void uploadFile(String selectedFilePath) {
        setButtonEnable(false);
        File file = new File(selectedFilePath);
        if (isNetworkAvailable(AssignmentAddActivity.this)) {
            int type = getMimeType(file, selectedFilePath);
            RequestBody requestFile;
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

//            String typeid = String.valueOf(type);
//            int filetypeid = Integer.valueOf(typeid);
            MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
            RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), String.valueOf(type));
            RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
            RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");
            RequestBody totalpages = RequestBody.create(MediaType.parse("text/plain"), "0");

            Call<RestResponse<FileUploadResponse>> call = apiService.uploadAssignmentFile(body, fileTypeId, duration, filesize, totalpages);
            call.enqueue(new Callback<RestResponse<FileUploadResponse>>() {
                @Override
                public void onResponse(@NonNull Call<RestResponse<FileUploadResponse>> call, @NonNull Response<RestResponse<FileUploadResponse>> response) {
                    setButtonEnable(true);
                    assert response.body() != null;
                    if (response.body().getResponse_code().equalsIgnoreCase("0")) {
                        fileUploadList.add(String.valueOf(response.body().getData().getId()));
                        assignmentAddLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                    }
                }

                @Override
                public void onFailure(@NonNull Call<RestResponse<FileUploadResponse>> call, @NonNull Throwable t) {
                    setButtonEnable(true);
                    assignmentAddLayoutBinding.progressDialogLay.itemProgressbar.setVisibility(View.GONE);
                }
            });
        }

    }

    @NonNull
    static int getMimeType(@NonNull File file, String selectedFilePath) {
        String type = null;
        int fileType = -1;
        final String url = file.toString();
        final String extension = MimeTypeMap.getFileExtensionFromUrl(url);
        if (extension != null) {
            type = MimeTypeMap.getSingleton().getMimeTypeFromExtension(extension.toLowerCase());
            Log.e("type", "getMimeType: " + type);
        }
        if (type == null) {
            if (selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xls")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlsx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("csv")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dbf")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dif")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("ods")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("prn")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("slk")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xla")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlam")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlsb")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlsm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xltm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xltx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xlw")) {
                fileType = 6;
            } else if (selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("doc")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("docx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("docs")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("docm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dot")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dotm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("dotx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("odt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("rtf")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("txt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("wps")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xml")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("xps")) {
                fileType = 7;
            } else if (selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("pptx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("ppt")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("pptm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("potx")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("potm")
                    || selectedFilePath.substring(selectedFilePath.lastIndexOf(".") + 1).equalsIgnoreCase("odp")) {
                fileType = 8;
            }
//            } else {
//                fileType = 0;
//            }
        } else {
            if (type.contains("image")) {
                fileType = 3;
            } else if (type.contains("video")) {
                fileType = 2;
            } else if (type.contains("pdf")) {
                fileType = 1;
            } else if (type.contains("excel")) {
                fileType = 6;
            } else if (type.contains("msword")) {
                fileType = 7;
            } else if (type.contains("vnd.ms-powerpoint")) {
                fileType = 8;
            }
        }
        return fileType;
    }

    private void setButtonEnable(boolean isEnable) {
        assignmentAddLayoutBinding.cardAddSubmit.setEnabled(isEnable);
        assignmentAddLayoutBinding.cardAddSubmit.setAlpha(isEnable ? 1f : 0.5f);
    }

    @Override
    public void onBackPressed() {
       /* Fragment f = getSupportFragmentManager().findFragmentById(R.id.filechooserFragment);
        if (f instanceof FileChooser) {
            assignmentAddLayoutBinding.filechooserFragment.setVisibility(View.GONE);
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

    private void submitAssignmentLession() {
        Gson gson = new Gson();
        JsonParser parser = new JsonParser();
        HashMap<String, Object> hashMap = new HashMap<>();
        hashMap.put(Const.assignmentid, assignment.getId());
        hashMap.put(Const.issubmission, true);
        hashMap.put(Const.userid, userId);
        hashMap.put(Const.files, fileUploadList);

        showDialog(getString(R.string.loading));
        disposable.add(apiService.submitAssignmentLession((JsonObject) parser.parse(gson.toJson(hashMap)))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<AssignmentData>>() {
                    @Override
                    public void onSuccess(RestResponse<AssignmentData> assignmentDataRestResponse) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), assignmentDataRestResponse.getMessage(), Toast.LENGTH_SHORT).show();
                        PrivousScreen();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));
    }

    private void submitAssignment() {
        Gson gson = new Gson();
        JsonParser parser = new JsonParser();
        HashMap<String, Object> hashMap = new HashMap<>();
        hashMap.put(Const.assignmentid, assignment.getId());
        hashMap.put(Const.issubmission, true);
        hashMap.put(Const.userid, userId);
        hashMap.put(Const.files, fileUploadList);

        showDialog(getString(R.string.loading));
        disposable.add(apiService.submitAssignment((JsonObject) parser.parse(gson.toJson(hashMap)))
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<RestResponse<AssignmentData>>() {
                    @Override
                    public void onSuccess(RestResponse<AssignmentData> assignmentDataRestResponse) {
                        hideDialog();
                        Toast.makeText(getApplicationContext(), assignmentDataRestResponse.getMessage(), Toast.LENGTH_SHORT).show();
                        PrivousScreen();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        showError(e);
                    }
                }));
    }

    public void createdAssigmentSubmission() {
        if (flag == 1) {
            submitAssignmentLession();
        } else {
            submitAssignment();
        }
    }

    private boolean validateFields() {
        if (!Validator.checkEmpty(assignmentAddLayoutBinding.assignmentTopicName)) {
            hideKeyBoard(assignmentAddLayoutBinding.assignmentTopicName);
            assignmentAddLayoutBinding.assignmentTopicNameWrapper.setError(getString(R.string.validation_enter_assignment_title));
            return false;
        } else {
            hideKeyBoard(assignmentAddLayoutBinding.assignmentTopicName);
            assignmentAddLayoutBinding.assignmentTopicNameWrapper.setErrorEnabled(false);
        }
        return true;
    }

    public void PrivousScreen() {
        finish();
       /* Intent i = new Intent(getApplicationContext(), MainDashBoardActivity.class);
        i.putExtra(Const.isDiscussions, true);
        startActivity(i);
*/
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

            for (int i = 0; i < fileUploadList.size(); i++) {
                File file = new File(fileUploadList.get(i));
                byte[] bitmapImage = new byte[(int) file.length()];
                if (bitmapImage != null) {
                    if (isNetworkAvailable(AssignmentAddActivity.this)) {
                        //Log.e("selectedFilePaths", "BITMAP" + bitmapImage);

                        RequestBody requestFile = RequestBody.create(MediaType.parse("*/*"), file);
                        Log.e("requestFile  ", "doInBackground: " + requestFile.toString());
                        MultipartBody.Part body = MultipartBody.Part.createFormData(Const.uploadTopicFile, file.getName(), requestFile);
                        Log.e("body  ", "doInBackground: " + body.toString());
                        RequestBody fileTypeId = RequestBody.create(MediaType.parse("text/plain"), "1");
                        RequestBody duration = RequestBody.create(MediaType.parse("text/plain"), "");
                        RequestBody filesize = RequestBody.create(MediaType.parse("text/plain"), "");

                        Call<UploadTopicFile> call = apiService.UploadTopicFile(body, fileTypeId, duration, filesize);
                        call.enqueue(new Callback<UploadTopicFile>() {
                            @Override
                            public void onResponse(@NonNull Call<UploadTopicFile> call, @NonNull retrofit2.Response<UploadTopicFile> response) {

                                if (response.isSuccessful()) {
                                    UploadTopicFile uploadImageObject = response.body();
                                    assert uploadImageObject != null;
                                    filesidArraylist.add(Integer.valueOf(uploadImageObject.getFile_id()));
                                    delegate.asynDelegateMethod(filesidArraylist);

                                } else {
                                    ResponseBody errorBody = response.errorBody();
                                    Gson gson = new Gson();
                                    try {
                                        assert errorBody != null;
                                        UploadTopicFile uploadTopicFile = gson.fromJson(errorBody.string(), UploadTopicFile.class);
                                        //Toast.makeText(getApplicationContext(), uploadTopicFile.getMessage(), Toast.LENGTH_LONG).show();

                                    } catch (Exception e) {
                                        e.printStackTrace();
                                        showError(e);
                                    }
                                }
                            }

                            @Override
                            public void onFailure(@NonNull Call<UploadTopicFile> call, @NonNull Throwable t) {
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

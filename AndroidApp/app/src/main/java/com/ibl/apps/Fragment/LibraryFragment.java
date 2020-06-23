package com.ibl.apps.Fragment;

import android.annotation.SuppressLint;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.SearchView;
import android.widget.Toast;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.databinding.DataBindingUtil;
import androidx.lifecycle.Observer;
import androidx.recyclerview.widget.GridLayoutManager;
import androidx.recyclerview.widget.LinearLayoutManager;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.LibraryGradeListAdapter;
import com.ibl.apps.Adapter.LibraryListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Interface.BackInterface;
import com.ibl.apps.LibraryManagement.LibraryRepository;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.RoomDatabase.dao.libraryManagementDatabase.LibraryDatabaseRepository;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Service.BookImageManager;
import com.ibl.apps.noon.ChapterActivity;
import com.ibl.apps.noon.LoginDevicesActivity;
import com.ibl.apps.noon.MainDashBoardActivity;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.LibraryLayoutBinding;
import com.ibl.apps.util.Const;
import com.ibl.apps.util.PrefUtils;

import java.net.NetworkInterface;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

import static android.content.Context.MODE_PRIVATE;


public class LibraryFragment extends BaseFragment implements View.OnClickListener {

    private LibraryLayoutBinding libraryLayoutBinding;
    private CompositeDisposable disposable = new CompositeDisposable();

    private static final String ARG_PARAM1 = "param1";
    private static final String ARG_PARAM2 = "param2";

    // TODO: Rename and change types of parameters
    private String mParam1;
    private String mParam2;

    UserDetails userDetailsObject;
    String GradeId, CourseName;
    String userId = "0";
    String ActivityFlag = "";
    String LessonID = "";
    String QuizID = "";
    String userRoleName = "";
    boolean bookLayout = false;
    LibraryListAdapter libraryListAdapter;
    LibraryGradeListAdapter libraryGradeListAdapter;
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";
    boolean AddtionalLibraryBoolean = true;
    BackInterface backInterface;
    private LibraryRepository libraryRepository;
    private LibraryDatabaseRepository libraryDatabaseRepository;
    private String deviceStatusCode;
    private Observer<Integer> observer;
    private Observer<Integer> chapterObserver;

    public LibraryFragment() {
        // Required empty public constructor
    }

    public static LibraryFragment newInstance(String param1, String param2) {
        LibraryFragment fragment = new LibraryFragment();
        Bundle args = new Bundle();
        args.putString(ARG_PARAM1, param1);
        args.putString(ARG_PARAM2, param2);
        fragment.setArguments(args);
        return fragment;
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            mParam1 = getArguments().getString(ARG_PARAM1);
            mParam2 = getArguments().getString(ARG_PARAM2);
        }
    }

    @Override
    public View onCreateView(@NonNull LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        libraryLayoutBinding = DataBindingUtil.inflate(inflater, R.layout.library_layout, container, false);
        observer = new Observer<Integer>() {
            @Override
            public void onChanged(Integer integer) {
                if (getFragmentManager() != null && integer == 1) {
                    callAPIDeviceManagement();
                    MainDashBoardActivity.coursePageNoArray.setValue(-1);
                }
            }
        };
        MainDashBoardActivity.coursePageNoArray.observe(getViewLifecycleOwner(), observer);


        chapterObserver = new Observer<Integer>() {
            @Override
            public void onChanged(Integer integer) {
                if (getFragmentManager() != null && integer == 2) {
//                    callAPIDeviceManagement();
                    libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                    libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                    PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                        @Override
                        public UserDetails getLocalUserDetails(UserDetails userDetails) {
                            if (userDetails != null) {
                                userDetailsObject = userDetails;
                                userId = userDetailsObject.getId();
                                if (userDetails.getRoleName() != null) {
                                    userRoleName = userDetails.getRoleName().get(0);
                                }

                                AddtionalLibrary = userDetails.getIs_library_authorized();
                                AddtionalAssignment = userDetails.getIs_assignment_authorized();
                                AddtionalDiscussions = userDetails.getIs_discussion_authorized();

                                if (!TextUtils.isEmpty(AddtionalLibrary)) {
                                    AddtionalLibraryBoolean = Boolean.parseBoolean(AddtionalLibrary);
                                }

                                if (AddtionalLibraryBoolean) {
                                    setBookLayout(bookLayout);
                                    libraryLayoutBinding.additionalServicetxt.setVisibility(View.GONE);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                                } else {
                                    libraryLayoutBinding.additionalServicetxt.setVisibility(View.VISIBLE);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                                }
                            }
                            return null;
                        }

                    }).execute();

                    libraryLayoutBinding.booksearchview.setOnQueryTextListener(new SearchView.OnQueryTextListener() {

                        @Override
                        public boolean onQueryTextChange(String newText) {

                            try {
                                if (TextUtils.isEmpty(newText)) {
                                    CallApiLibrarySearch(newText, bookLayout);
                                }
                            } catch (Exception e) {
                                e.printStackTrace();
                            }
                            return true;
                        }

                        @Override
                        public boolean onQueryTextSubmit(String query) {
                            CallApiLibrarySearch(query, bookLayout);
                            return true;
                        }
                    });
                    ChapterActivity.pageNo1.setValue(-1);
                }
            }
        };
        ChapterActivity.pageNo1.observe(getViewLifecycleOwner(), chapterObserver);

        return libraryLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
        libraryRepository = new LibraryRepository();
        libraryDatabaseRepository = new LibraryDatabaseRepository();
        if (Build.VERSION.SDK_INT <= Build.VERSION_CODES.KITKAT) {
            libraryLayoutBinding.libraryText.setTextSize(35);
        }

        Bundle bundle = this.getArguments();
        if (bundle != null) {
            GradeId = bundle.getString(Const.GradeID, "");
            CourseName = bundle.getString(Const.CourseName, "");
            ActivityFlag = bundle.getString(Const.ActivityFlag, "");
            LessonID = bundle.getString(Const.LessonID, "");
            QuizID = bundle.getString(Const.QuizID, "");
        }
        setToolbar(libraryLayoutBinding.toolBar);
        //showBackArrow(getString(R.string.item_5));
        libraryLayoutBinding.libraryText.setText(getString(R.string.item_5));


        backInterface = (BackInterface) getActivity();
        setOnClickListener();
    }


    public String getWifiMacAddress() {
        try {
            String interfaceName = "wlan0";
            List<NetworkInterface> interfaces = Collections.list(NetworkInterface.getNetworkInterfaces());
            for (NetworkInterface intf : interfaces) {
                if (!intf.getName().equalsIgnoreCase(interfaceName)) {
                    continue;
                }

                byte[] mac = intf.getHardwareAddress();
                if (mac == null) {
                    return "";
                }

                StringBuilder buf = new StringBuilder();
                for (byte aMac : mac) {
                    buf.append(String.format("%02X:", aMac));
                }
                if (buf.length() > 0) {
                    buf.deleteCharAt(buf.length() - 1);
                }
                return buf.toString();
            }
        } catch (Exception ex) {
            ex.getMessage();
        } // for now eat exceptions
        return "";
    }

    private void callAPIDeviceManagement() {
        SharedPreferences deviceSharedPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
        deviceStatusCode = deviceSharedPreferences.getString("deviceStatusCode", "");
        Log.e("deviceStatusCode", "setUp: LIBRARYNEW" + deviceStatusCode);

        if (deviceStatusCode != null) {
            switch (deviceStatusCode) {
                case "0":
                    libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                    libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                    PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                        @Override
                        public UserDetails getLocalUserDetails(UserDetails userDetails) {
                            if (userDetails != null) {
                                userDetailsObject = userDetails;
                                userId = userDetailsObject.getId();
                                if (userDetails.getRoleName() != null) {
                                    userRoleName = userDetails.getRoleName().get(0);
                                }

                                AddtionalLibrary = userDetails.getIs_library_authorized();
                                AddtionalAssignment = userDetails.getIs_assignment_authorized();
                                AddtionalDiscussions = userDetails.getIs_discussion_authorized();

                                if (!TextUtils.isEmpty(AddtionalLibrary)) {
                                    AddtionalLibraryBoolean = Boolean.parseBoolean(AddtionalLibrary);
                                }

                                if (AddtionalLibraryBoolean) {
                                    setBookLayout(bookLayout);
                                    libraryLayoutBinding.additionalServicetxt.setVisibility(View.GONE);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                                } else {
                                    libraryLayoutBinding.additionalServicetxt.setVisibility(View.VISIBLE);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                                }
                            }
                            return null;
                        }

                    }).execute();

                    libraryLayoutBinding.booksearchview.setOnQueryTextListener(new SearchView.OnQueryTextListener() {

                        @Override
                        public boolean onQueryTextChange(String newText) {

                            try {
                                if (TextUtils.isEmpty(newText)) {
                                    CallApiLibrarySearch(newText, bookLayout);
                                }
                            } catch (Exception e) {
                                e.printStackTrace();
                            }
                            return true;
                        }

                        @Override
                        public boolean onQueryTextSubmit(String query) {
                            CallApiLibrarySearch(query, bookLayout);
                            return true;
                        }
                    });
                    hideDialog();
                    break;
                case "2":
                    libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.VISIBLE);
                    libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                    break;
                case "3":
                    libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                    libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.VISIBLE);
                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                    break;
                case "4":
                    Toast.makeText(getActivity(), getActivity().getResources().getString(R.string.invalid_mac_ip), Toast.LENGTH_LONG).show();
                    break;
            }
        }

         /*DeviceManagementRepository deviceManagementRepository = new DeviceManagementRepository();
        disposable.add(deviceManagementRepository.registerDeviceDetail(jsonObject)
                .subscribeOn(Schedulers.io())
                .observeOn(AndroidSchedulers.mainThread())
                .subscribeWith(new DisposableSingleObserver<Response<DeviceRegisterModel>>() {
                    private String deviceToken;

                    @Override
                    public void onSuccess(Response<DeviceRegisterModel> deviceListModel) {
                        try {
                            if ((deviceListModel.errorBody() != null)) {
                                errorCode = new Gson().fromJson(deviceListModel.errorBody().string(), DeviceRegisterModel.class).getResponseCode();

                                if (errorCode == 2) {
                                    deviceStatus = 2;
                                    SharedPreferences deviceStatusPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                    editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                    editor.apply();
                                    libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.VISIBLE);
                                    libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                                    //libraryLayoutBinding.advanceSearchLayout.mainAdvanceSearchLayout.setVisibility(View.GONE);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                                } else if (errorCode == 3) {
                                    deviceStatus = 3;
                                    SharedPreferences deviceStatusPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                    SharedPreferences.Editor editor = deviceStatusPreferences.edit();
                                    editor.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                    editor.apply();
                                    libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                                    libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.VISIBLE);
                                    //libraryLayoutBinding.advanceSearchLayout.mainAdvanceSearchLayout.setVisibility(View.GONE);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                                }
                            }

                            if (deviceListModel.body() != null && deviceListModel.body().getResponseCode() == 0) {
                                if (deviceListModel.body().getData().getDeviceToken() != null) {
                                    deviceToken = deviceListModel.body().getData().getDeviceToken();
                                }

                                SharedPreferences sharedPreferences = getActivity().getSharedPreferences("deviceManagement", MODE_PRIVATE);
                                SharedPreferences.Editor editor = sharedPreferences.edit();

                                if (editor != null) {
                                    editor.putString("deviceToken", deviceToken);
                                    Log.e("deviceToken", "onSuccess: " + deviceToken);
                                    editor.apply();
                                }
                                libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                                libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                                libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                                PrefUtils.MyAsyncTask asyncTask = (PrefUtils.MyAsyncTask) new PrefUtils.MyAsyncTask(new PrefUtils.MyAsyncTask.AsyncResponse() {
                                    @Override
                                    public UserDetails getLocalUserDetails(UserDetails userDetails) {
                                        if (userDetails != null) {
                                            userDetailsObject = userDetails;
                                            userId = userDetailsObject.getId();
                                            if (userDetails.getRoleName() != null) {
                                                userRoleName = userDetails.getRoleName().get(0);
                                            }

                                            AddtionalLibrary = userDetails.getIs_library_authorized();
                                            AddtionalAssignment = userDetails.getIs_assignment_authorized();
                                            AddtionalDiscussions = userDetails.getIs_discussion_authorized();

                                            if (!TextUtils.isEmpty(AddtionalLibrary)) {
                                                AddtionalLibraryBoolean = Boolean.parseBoolean(AddtionalLibrary);
                                            }

                                            if (AddtionalLibraryBoolean) {
                                                setBookLayout(bookLayout);
                                                libraryLayoutBinding.additionalServicetxt.setVisibility(View.GONE);
                                                libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                                            } else {
                                                libraryLayoutBinding.additionalServicetxt.setVisibility(View.VISIBLE);
                                                libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                                            }
                                        }
                                        return null;
                                    }

                                }).execute();

                                libraryLayoutBinding.booksearchview.setOnQueryTextListener(new SearchView.OnQueryTextListener() {

                                    @Override
                                    public boolean onQueryTextChange(String newText) {

                                        try {
                                            if (TextUtils.isEmpty(newText)) {
                                                CallApiLibrarySearch(newText, bookLayout);
                                            }
                                        } catch (Exception e) {
                                            e.printStackTrace();
                                        }
                                        return true;
                                    }

                                    @Override
                                    public boolean onQueryTextSubmit(String query) {
                                        CallApiLibrarySearch(query, bookLayout);
                                        return true;
                                    }
                                });
                                deviceStatus = 0;
                                SharedPreferences deviceStatusPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
                                SharedPreferences.Editor editor1 = deviceStatusPreferences.edit();
                                editor1.putString("deviceStatusCode", String.valueOf(deviceStatus));
                                editor1.apply();
                            }
                        } catch (IOException e) {
                            e.printStackTrace();
                        }
                        hideDialog();
                    }

                    @Override
                    public void onError(Throwable e) {
                        hideDialog();
                        SharedPreferences deviceSharedPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
                        String deviceStatusCode = deviceSharedPreferences.getString("deviceStatusCode", "");
                        if (deviceStatusCode.equals("0")) {
                            libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                            libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.VISIBLE);
                            new setLocalDataTask(new LibraryAsyncResponse() {
                                @Override
                                public List<LibraryObject> getLocalUserDetails(List<LibraryObject> libraryObjects) {

                                    List<LibraryObject.Data> libraryDataObjects = new ArrayList<>();

                                    for (int i = 0; i < libraryObjects.size(); i++) {
                                        libraryDataObjects = libraryObjects.get(i).getData();
                                    }

                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new GridLayoutManager(getActivity(), 2));
                                    libraryListAdapter = new LibraryListAdapter(getActivity(), libraryDataObjects, userDetailsObject);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryListAdapter);
                                    hideDialog();
                                    return null;
                                }
                            }).execute();
                        } else if (deviceStatusCode.equals("2")) {
                            libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.VISIBLE);
                            libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.GONE);
                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                        } else if (deviceStatusCode.equals("3")) {
                            libraryLayoutBinding.deactivatedDeviceQuota.deviceDeactivateLay.setVisibility(View.GONE);
                            libraryLayoutBinding.outOfDeviceQuota.deviceQuotaLay.setVisibility(View.VISIBLE);
                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setVisibility(View.GONE);
                        }
                    }
                }));*/
    }

    public void setOnClickListener() {
        SharedPreferences deviceSharedPreferences = getActivity().getSharedPreferences("deviceStatus", MODE_PRIVATE);
        deviceStatusCode = deviceSharedPreferences.getString("deviceStatusCode", "");

        libraryLayoutBinding.serachMenu.setOnClickListener(this);
        libraryLayoutBinding.btnbackLibrary.setOnClickListener(this);
        libraryLayoutBinding.gradeMenu.setOnClickListener(this);
        libraryLayoutBinding.outOfDeviceQuota.imgOutQuota.setOnClickListener(this);
        libraryLayoutBinding.deactivatedDeviceQuota.imgDeactivate.setOnClickListener(this);
    }

    public void setBookLayout(boolean bookLayout) {

        if (bookLayout) {
            if (isNetworkAvailable(getActivity())) {
                CallApiLibraryGradeViseBookList();
            } else {
                new setLocalGradeViseDataTask(new LibraryGradeAsyncResponse() {
                    @Override
                    public List<LibraryGradeObject> getLocalUserDetails(List<LibraryGradeObject> libraryObjects) {

                        List<LibraryGradeObject.Data> libraryDataObjects = new ArrayList<>();

                        for (int i = 0; i < libraryObjects.size(); i++) {
                            libraryDataObjects = libraryObjects.get(i).getData();
                        }
                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new LinearLayoutManager(getActivity()));
                        libraryGradeListAdapter = new LibraryGradeListAdapter(getActivity(), libraryDataObjects, userDetailsObject);
                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryGradeListAdapter);
                        hideDialog();
                        return null;
                    }
                }).execute();
            }

        } else {

            if (isNetworkAvailable(getActivity())) {
                CallApiLibraryBookList();
            } else {
                new setLocalDataTask(new LibraryAsyncResponse() {
                    @Override
                    public List<LibraryObject> getLocalUserDetails(List<LibraryObject> libraryObjects) {

                        List<LibraryObject.Data> libraryDataObjects = new ArrayList<>();

                        for (int i = 0; i < libraryObjects.size(); i++) {
                            libraryDataObjects = libraryObjects.get(i).getData();
                        }

                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new GridLayoutManager(getActivity(), 2));
                        libraryListAdapter = new LibraryListAdapter(getActivity(), libraryDataObjects, userDetailsObject);
                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryListAdapter);
                        hideDialog();
                        return null;
                    }
                }).execute();
            }
        }
    }

    private void CallApiLibraryBookList() {

        try {

            showDialog(getString(R.string.loading));
            disposable.add(libraryRepository.fetchBookList("")
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<LibraryObject>() {
                        @Override
                        public void onSuccess(LibraryObject libraryObject) {


                            new setOnlineDataTask(new LibraryAsyncResponse() {

                                @Override
                                public List<LibraryObject> getLocalUserDetails(List<LibraryObject> libraryObjects) {
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new GridLayoutManager(getActivity(), 2));
                                    libraryListAdapter = new LibraryListAdapter(getActivity(), libraryObject.getData(), userDetailsObject);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryListAdapter);
                                    hideDialog();

                                    return null;
                                }
                            }, libraryObject).execute();
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();
                            try {
                                HttpException error = (HttpException) e;
                                LibraryObject libraryObject = new Gson().fromJson(error.response().errorBody().string(), LibraryObject.class);
                                //showSnackBar(libraryLayoutBinding.mainLibraryLayout, libraryObject.getMessage());
                            } catch (Exception e1) {
                                //showError(e);

                                new setLocalDataTask(new LibraryAsyncResponse() {
                                    @Override
                                    public List<LibraryObject> getLocalUserDetails(List<LibraryObject> libraryObjects) {

                                        if (libraryObjects != null) {
                                            List<LibraryObject.Data> libraryDataObjects = new ArrayList<>();

                                            for (int i = 0; i < libraryObjects.size(); i++) {
                                                libraryDataObjects = libraryObjects.get(i).getData();
                                            }

                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new GridLayoutManager(getActivity(), 2));
                                            libraryListAdapter = new LibraryListAdapter(getActivity(), libraryDataObjects, userDetailsObject);
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryListAdapter);
                                        } else {
                                            showError(e);
                                        }

                                        hideDialog();
                                        return null;
                                    }
                                }).execute();
                            }
                        }
                    }));


        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public class setOnlineDataTask extends AsyncTask<Void, Void, List<LibraryObject>> {

        LibraryObject libraryObject;
        LibraryAsyncResponse delegate = null;
        List<LibraryObject> dataList = new ArrayList<>();

        public setOnlineDataTask(LibraryAsyncResponse delegate, LibraryObject libraryObject) {
            this.delegate = delegate;
            this.libraryObject = libraryObject;
        }

        @Override
        protected List<LibraryObject> doInBackground(Void... params) {

            try {

                libraryObject.setUserId(userDetailsObject.getId());
                dataList.add(libraryObject);

                LibraryObject exiestlibraryObject = libraryDatabaseRepository.getAllLibraryObjectByUserId(userId);
                if (exiestlibraryObject != null) {
                    libraryDatabaseRepository.updateAll(libraryObject.getData(), userId);
                } else {
                    libraryDatabaseRepository.insertLibraryData(libraryObject);
                }
                BookImageManager.start(getActivity(), libraryObject, userId);

            } catch (Exception e) {
                e.printStackTrace();
            }

            return dataList;
        }

        @Override
        protected void onPostExecute(List<LibraryObject> dataList) {
            delegate.getLocalUserDetails(dataList);
        }
    }

    public class setLocalDataTask extends AsyncTask<Void, Void, List<LibraryObject>> {

        LibraryAsyncResponse delegate = null;
        List<LibraryObject> libraryObjects = new ArrayList<>();
        List<LibraryObject.Data> libraryDataObjects = new ArrayList<>();

        public setLocalDataTask(LibraryAsyncResponse delegate) {
            this.delegate = delegate;
        }

        @Override
        protected void onPreExecute() {
            showDialog(getString(R.string.loading));
            super.onPreExecute();
        }

        @Override
        protected List<LibraryObject> doInBackground(Void... params) {
            try {
                if (libraryDatabaseRepository.getLibraryBookByUserId(userId) != null) {
                    if (libraryDatabaseRepository.getLibraryBookByUserId(userId).size() != 0) {
                        libraryObjects = libraryDatabaseRepository.getLibraryBookByUserId(userId);
                        for (int i = 0; i < libraryObjects.size(); i++) {
                            libraryDataObjects = libraryObjects.get(i).getData();
                            if (libraryDataObjects != null && libraryDataObjects.size() != 0) {
                                for (int j = 0; j < libraryDataObjects.size(); j++) {
                                    byte[] bitmapImage = libraryDatabaseRepository.getLibraryBookImage(userId, libraryDataObjects.get(j).getId());
                                    if (bitmapImage != null) {
                                        libraryDataObjects.get(j).setBookcoverImageBitmap(bitmapImage);
                                    }
                                }
                            }
                        }
                    }
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
            return libraryObjects;
        }

        @Override
        protected void onPostExecute(List<LibraryObject> varProgressArr) {
            delegate.getLocalUserDetails(varProgressArr);
        }
    }

    private void CallApiLibraryGradeViseBookList() {

        try {

            showDialog(getString(R.string.loading));
            disposable.add(libraryRepository.fetchBooksGradevise("")
                    .subscribeOn(Schedulers.io())
                    .observeOn(AndroidSchedulers.mainThread())
                    .subscribeWith(new DisposableSingleObserver<LibraryGradeObject>() {
                        @Override
                        public void onSuccess(LibraryGradeObject libraryGradeObject) {

                            new setOnlineGradeViseDataTask(new LibraryGradeAsyncResponse() {

                                @Override
                                public List<LibraryGradeObject> getLocalUserDetails(List<LibraryGradeObject> libraryObjects) {
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new LinearLayoutManager(getActivity()));
                                    libraryGradeListAdapter = new LibraryGradeListAdapter(getActivity(), libraryGradeObject.getData(), userDetailsObject);
                                    libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryGradeListAdapter);
                                    hideDialog();
                                    return null;
                                }
                            }, libraryGradeObject).execute();
                        }

                        @Override
                        public void onError(Throwable e) {
                            hideDialog();
                            try {
                                HttpException error = (HttpException) e;
                                LibraryGradeObject libraryGradeObject = new Gson().fromJson(error.response().errorBody().string(), LibraryGradeObject.class);
                                //showSnackBar(libraryLayoutBinding.mainLibraryLayout, libraryGradeObject.getMessage());
                                new setLocalGradeViseDataTask(new LibraryGradeAsyncResponse() {
                                    @Override
                                    public List<LibraryGradeObject> getLocalUserDetails(List<LibraryGradeObject> libraryObjects) {

                                        if (libraryObjects != null) {
                                            List<LibraryGradeObject.Data> libraryDataObjects = new ArrayList<>();

                                            for (int i = 0; i < libraryObjects.size(); i++) {
                                                libraryDataObjects = libraryObjects.get(i).getData();
                                            }
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new LinearLayoutManager(getActivity()));
                                            libraryGradeListAdapter = new LibraryGradeListAdapter(getActivity(), libraryDataObjects, userDetailsObject);
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryGradeListAdapter);
                                        } else {
                                            showError(e);
                                        }

                                        hideDialog();
                                        return null;
                                    }
                                }).execute();
                            } catch (Exception e1) {
                                // showError(e);
                                new setLocalGradeViseDataTask(new LibraryGradeAsyncResponse() {
                                    @Override
                                    public List<LibraryGradeObject> getLocalUserDetails(List<LibraryGradeObject> libraryObjects) {

                                        if (libraryObjects != null) {
                                            List<LibraryGradeObject.Data> libraryDataObjects = new ArrayList<>();

                                            for (int i = 0; i < libraryObjects.size(); i++) {
                                                libraryDataObjects = libraryObjects.get(i).getData();
                                            }
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new LinearLayoutManager(getActivity()));
                                            libraryGradeListAdapter = new LibraryGradeListAdapter(getActivity(), libraryDataObjects, userDetailsObject);
                                            libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryGradeListAdapter);
                                        } else {
                                            showError(e);
                                        }

                                        hideDialog();
                                        return null;
                                    }
                                }).execute();
                            }
                        }
                    }));

        } catch (Exception e) {
            e.printStackTrace();
        }

    }

    public class setOnlineGradeViseDataTask extends AsyncTask<Void, Void, List<LibraryGradeObject>> {

        LibraryGradeObject libraryObject;
        LibraryGradeAsyncResponse delegate = null;
        List<LibraryGradeObject> dataList = new ArrayList<>();

        public setOnlineGradeViseDataTask(LibraryGradeAsyncResponse delegate, LibraryGradeObject libraryObject) {
            this.delegate = delegate;
            this.libraryObject = libraryObject;
        }

        @Override
        protected List<LibraryGradeObject> doInBackground(Void... params) {
            try {
                libraryObject.setUserId(userDetailsObject.getId());
                dataList.add(libraryObject);

                LibraryGradeObject exiestlibraryObject = libraryDatabaseRepository.getAllLibraryGradeByUserId(userId);
                if (exiestlibraryObject != null) {
                    libraryDatabaseRepository.updateLibraryGradeBook(libraryObject.getData(), userId);
                } else {
                    libraryDatabaseRepository.insertLibraryGradeData(libraryObject);
                }

            } catch (Exception e) {
                e.printStackTrace();
            }

            return dataList;
        }

        @Override
        protected void onPostExecute(List<LibraryGradeObject> dataList) {
            delegate.getLocalUserDetails(dataList);
        }
    }

    public class setLocalGradeViseDataTask extends AsyncTask<Void, Void, List<LibraryGradeObject>> {

        LibraryGradeAsyncResponse delegate = null;
        List<LibraryGradeObject> libraryObjects = new ArrayList<>();
        List<LibraryGradeObject.Data> libraryDataObjects = new ArrayList<>();

        public setLocalGradeViseDataTask(LibraryGradeAsyncResponse delegate) {
            this.delegate = delegate;
        }

        @Override
        protected void onPreExecute() {
            showDialog(getString(R.string.loading));
            super.onPreExecute();
        }

        @Override
        protected List<LibraryGradeObject> doInBackground(Void... params) {
            try {
                if (libraryDatabaseRepository.getAllLibraryGradeBook(userId) != null) {
                    if (libraryDatabaseRepository.getLibraryBookByUserId(userId).size() != 0) {
                        libraryObjects = libraryDatabaseRepository.getAllLibraryGradeBook(userId);
                        for (int i = 0; i < libraryObjects.size(); i++) {
                            libraryDataObjects = libraryObjects.get(i).getData();
                            if (libraryDataObjects != null && libraryDataObjects.size() != 0) {
                                for (int j = 0; j < libraryDataObjects.size(); j++) {

                                    ArrayList<LibraryGradeObject.Books> booksArrayList = new ArrayList<>();
                                    for (int k = 0; k < booksArrayList.size(); k++) {
                                        byte[] bitmapImage = libraryDatabaseRepository.getLibraryGradeBookImage(userId, libraryDataObjects.get(j).getId());
                                        if (bitmapImage != null) {
                                            libraryDataObjects.get(j).getBooks().get(k).setBookGradecoverImageBitmap(bitmapImage);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
            return libraryObjects;
        }

        @Override
        protected void onPostExecute(List<LibraryGradeObject> varProgressArr) {
            delegate.getLocalUserDetails(varProgressArr);
        }
    }

    private void CallApiLibrarySearch(String query, boolean bookLayout) {

        if (bookLayout) {
            try {
                showDialog(getString(R.string.loading));

                if (isNetworkAvailable(getActivity())) {
                    disposable.add(libraryRepository.fetchBooksGradevise(query)
                            .subscribeOn(Schedulers.io())
                            .observeOn(AndroidSchedulers.mainThread())
                            .subscribeWith(new DisposableSingleObserver<LibraryGradeObject>() {
                                @Override
                                public void onSuccess(LibraryGradeObject libraryGradeObject) {

                                    if (libraryGradeObject.getData() != null) {
                                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new LinearLayoutManager(getActivity()));
                                        libraryGradeListAdapter = new LibraryGradeListAdapter(getActivity(), libraryGradeObject.getData(), userDetailsObject);
                                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryGradeListAdapter);

                                    } else {
                                        Toast.makeText(getActivity(), R.string.error_no_book_found, Toast.LENGTH_SHORT).show();
                                    }
                                    hideDialog();
                                }


                                @Override
                                public void onError(Throwable e) {
                                    hideDialog();
                                    try {
                                        HttpException error = (HttpException) e;
                                        LibraryGradeObject libraryGradeObject = new Gson().fromJson(error.response().errorBody().string(), LibraryGradeObject.class);
                                        showSnackBar(libraryLayoutBinding.mainLibraryLayout, libraryGradeObject.getMessage());
                                    } catch (Exception e1) {
                                        //showError(e);

                                        if (libraryGradeListAdapter != null) {
                                            if (libraryGradeListAdapter.getItemCount() != 0) {
                                                libraryGradeListAdapter.getFilter().filter(query);
                                            }
                                        } else {
                                            showError(e);
                                        }
                                        hideDialog();
                                    }
                                }
                            }));
                } else {
                    libraryGradeListAdapter.getFilter().filter(query);
                    hideDialog();
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
        } else {
            try {
                showDialog(getString(R.string.loading));

                if (isNetworkAvailable(getActivity())) {
                    disposable.add(libraryRepository.fetchBookList(query)
                            .subscribeOn(Schedulers.io())
                            .observeOn(AndroidSchedulers.mainThread())
                            .subscribeWith(new DisposableSingleObserver<LibraryObject>() {
                                @Override
                                public void onSuccess(LibraryObject libraryObject) {

                                    if (libraryObject.getData() != null) {
                                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setHasFixedSize(true);
                                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setLayoutManager(new GridLayoutManager(getActivity(), 2));
                                        libraryListAdapter = new LibraryListAdapter(getActivity(), libraryObject.getData(), userDetailsObject);
                                        libraryLayoutBinding.rcVerticalLayout.rcVertical.setAdapter(libraryListAdapter);
                                    } else {
                                        Toast.makeText(getActivity(), R.string.error_no_book_found, Toast.LENGTH_SHORT).show();
                                    }
                                    hideDialog();
                                }


                                @Override
                                public void onError(Throwable e) {
                                    hideDialog();
                                    try {
                                        HttpException error = (HttpException) e;
                                        LibraryObject libraryObject = new Gson().fromJson(error.response().errorBody().string(), LibraryObject.class);
                                        //showSnackBar(libraryLayoutBinding.mainLibraryLayout, libraryObject.getMessage());
                                    } catch (Exception e1) {
                                        //showError(e);

                                        if (libraryListAdapter != null) {
                                            if (libraryListAdapter.getItemCount() != 0) {
                                                libraryListAdapter.getFilter().filter(query);
                                            }
                                        } else {
                                            showError(e);
                                        }
                                        hideDialog();
                                    }
                                }
                            }));
                } else {
                    libraryListAdapter.getFilter().filter(query);
                    hideDialog();
                }

            } catch (Exception e) {
                e.printStackTrace();
            }
        }
    }

    public interface LibraryGradeAsyncResponse {
        List<LibraryGradeObject> getLocalUserDetails(List<LibraryGradeObject> courseListl);
    }

    public interface LibraryAsyncResponse {
        List<LibraryObject> getLocalUserDetails(List<LibraryObject> courseListl);
    }

    @SuppressLint("RestrictedApi")
    @Override
    public void onClick(View view) {
        switch (view.getId()) {
            case R.id.serachMenu:
                if (deviceStatusCode.equals("0")) {
                    libraryLayoutBinding.serachMenu.setVisibility(View.GONE);
                    libraryLayoutBinding.libraryText.setVisibility(View.GONE);
                    libraryLayoutBinding.booksearchview.setVisibility(View.VISIBLE);
                    libraryLayoutBinding.booksearchview.onActionViewExpanded();
                }
                break;
            case R.id.btnbackLibrary:
                if (libraryLayoutBinding.booksearchview.getVisibility() == View.VISIBLE) {
                    libraryLayoutBinding.serachMenu.setVisibility(View.VISIBLE);
                    libraryLayoutBinding.libraryText.setVisibility(View.VISIBLE);
                    libraryLayoutBinding.booksearchview.setVisibility(View.GONE);
                } else {
                    backInterface.backfragment();
                }
                break;
            case R.id.gradeMenu:
                if (deviceStatusCode.equals("0")) {
                    if (bookLayout) {
                        bookLayout = false;
                    } else {
                        bookLayout = true;
                    }
                    setBookLayout(bookLayout);
                }
                break;
            case R.id.imgOutQuota:
            case R.id.imgDeactivate:
                if (getActivity() != null)
                    if (isNetworkAvailable(getActivity()))
                        startActivity(new Intent(getActivity(), LoginDevicesActivity.class));
                    else showNetworkAlert(getActivity());
                break;
        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case android.R.id.home:
                getActivity().finish();
                return true;
            default:
                return super.onOptionsItemSelected(item);
        }
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        if (observer != null) {
            MainDashBoardActivity.courseIsAdd = false;
            MainDashBoardActivity.coursePageNoArray.removeObserver(observer);
        }

        if (chapterObserver != null) {
            ChapterActivity.pageNo1.removeObserver(chapterObserver);
        }
        super.onDestroy();
    }
}

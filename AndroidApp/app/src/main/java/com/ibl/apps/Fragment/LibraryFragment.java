package com.ibl.apps.Fragment;

import android.annotation.SuppressLint;
import android.databinding.DataBindingUtil;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v7.widget.GridLayoutManager;
import android.support.v7.widget.LinearLayoutManager;
import android.text.TextUtils;
import android.view.LayoutInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.SearchView;
import android.widget.Toast;

import com.google.gson.Gson;
import com.ibl.apps.Adapter.LibraryGradeListAdapter;
import com.ibl.apps.Adapter.LibraryListAdapter;
import com.ibl.apps.Base.BaseFragment;
import com.ibl.apps.Interface.BackInterface;
import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.Model.LibraryObject;
import com.ibl.apps.RoomDatabase.database.AppDatabase;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.Service.BookImageManager;
import com.ibl.apps.Utils.Const;
import com.ibl.apps.Utils.LoadMoreData.RecyclerViewLoadMoreScroll;
import com.ibl.apps.Utils.PrefUtils;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.LibraryLayoutBinding;

import java.util.ArrayList;
import java.util.List;

import io.reactivex.android.schedulers.AndroidSchedulers;
import io.reactivex.disposables.CompositeDisposable;
import io.reactivex.observers.DisposableSingleObserver;
import io.reactivex.schedulers.Schedulers;
import retrofit2.HttpException;

import static com.ibl.apps.Base.BaseActivity.apiService;


public class LibraryFragment extends BaseFragment implements View.OnClickListener {

    LibraryLayoutBinding libraryLayoutBinding;
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
    private RecyclerViewLoadMoreScroll scrollListener;

    boolean isLoad = true;
    int pageNumber = 1;
    String perpagerecord = "10";
    String userRoleName = "";
    boolean bookLayout = false;
    LibraryListAdapter libraryListAdapter;
    LibraryGradeListAdapter libraryGradeListAdapter;
    String AddtionalLibrary, AddtionalDiscussions, AddtionalAssignment = "";
    boolean AddtionalLibraryBoolean = true;
    BackInterface backInterface;

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
        return libraryLayoutBinding.getRoot();
    }

    @Override
    protected void setUp(View view) {
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
        backInterface = (BackInterface) getActivity();

        setOnClickListener();
    }

    public void setOnClickListener() {
        libraryLayoutBinding.serachMenu.setOnClickListener(this);
        libraryLayoutBinding.btnbackLibrary.setOnClickListener(this);
        libraryLayoutBinding.gradeMenu.setOnClickListener(this);
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
            disposable.add(apiService.fetchBookList("")
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

                LibraryObject exiestlibraryObject = AppDatabase.getAppDatabase(getActivity()).libraryDao().getAllLibraryObject(userId);
                if (exiestlibraryObject != null) {
                    AppDatabase.getAppDatabase(getActivity()).libraryDao().updateAll(libraryObject.getData(), userId);
                } else {
                    AppDatabase.getAppDatabase(getActivity()).libraryDao().insertAll(libraryObject);
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
                if (AppDatabase.getAppDatabase(getActivity()).libraryDao().getAlllibraryBook(userId) != null) {
                    if (AppDatabase.getAppDatabase(getActivity()).libraryDao().getAlllibraryBook(userId).size() != 0) {
                        libraryObjects = AppDatabase.getAppDatabase(getActivity()).libraryDao().getAlllibraryBook(userId);
                        for (int i = 0; i < libraryObjects.size(); i++) {
                            libraryDataObjects = libraryObjects.get(i).getData();
                            if (libraryDataObjects != null && libraryDataObjects.size() != 0) {
                                for (int j = 0; j < libraryDataObjects.size(); j++) {
                                    byte[] bitmapImage = AppDatabase.getAppDatabase(NoonApplication.getContext()).libraryDao().getBookImage(userId, libraryDataObjects.get(j).getId());
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
            disposable.add(apiService.fetchBooksGradevise("")
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

                LibraryGradeObject exiestlibraryObject = AppDatabase.getAppDatabase(getActivity()).libraryGradeDao().getAllLibraryGradeObject(userId);
                if (exiestlibraryObject != null) {
                    AppDatabase.getAppDatabase(getActivity()).libraryGradeDao().updateGradeBookAll(libraryObject.getData(), userId);
                } else {
                    AppDatabase.getAppDatabase(getActivity()).libraryGradeDao().insertAll(libraryObject);
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
                if (AppDatabase.getAppDatabase(getActivity()).libraryGradeDao().getAlllibraryGradeBook(userId) != null) {
                    if (AppDatabase.getAppDatabase(getActivity()).libraryDao().getAlllibraryBook(userId).size() != 0) {
                        libraryObjects = AppDatabase.getAppDatabase(getActivity()).libraryGradeDao().getAlllibraryGradeBook(userId);
                        for (int i = 0; i < libraryObjects.size(); i++) {
                            libraryDataObjects = libraryObjects.get(i).getData();
                            if (libraryDataObjects != null && libraryDataObjects.size() != 0) {
                                for (int j = 0; j < libraryDataObjects.size(); j++) {

                                    ArrayList<LibraryGradeObject.Books> booksArrayList = new ArrayList<>();
                                    for (int k = 0; k < booksArrayList.size(); k++) {
                                        byte[] bitmapImage = AppDatabase.getAppDatabase(NoonApplication.getContext()).libraryGradeDao().getBookImage(userId, libraryDataObjects.get(j).getId());
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
                    disposable.add(apiService.fetchBooksGradevise(query)
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
                    disposable.add(apiService.fetchBookList(query)
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
                libraryLayoutBinding.serachMenu.setVisibility(View.GONE);
                libraryLayoutBinding.libraryText.setVisibility(View.GONE);
                libraryLayoutBinding.booksearchview.setVisibility(View.VISIBLE);
                libraryLayoutBinding.booksearchview.onActionViewExpanded();
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

                if (bookLayout) {
                    bookLayout = false;
                } else {
                    bookLayout = true;
                }
                setBookLayout(bookLayout);
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
}

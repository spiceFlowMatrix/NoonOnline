//
// Source code recreated from a .class file by IntelliJ IDEA
// (powered by Fernflower decompiler)
//

package com.ibl.apps.Utils.CustomView;

import android.app.Dialog;
import android.graphics.Color;
import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.v7.app.AppCompatDialogFragment;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.SearchView;
import android.support.v7.widget.SearchView.OnQueryTextListener;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.TextView;

import com.abdeveloper.library.R.id;
import com.abdeveloper.library.R.layout;
import com.abdeveloper.library.RecyclerViewEmptySupport;
import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.noon.R;

import java.util.ArrayList;
import java.util.Iterator;
import java.util.Objects;

import static com.ibl.apps.Fragment.ProgressReportFragment.selectedIdsForCallback;

public class MultiSelectSpinner extends AppCompatDialogFragment implements OnQueryTextListener, OnClickListener {

    public ArrayList<ParentSpinnerModel.Data> mainListOfAdapter = new ArrayList<>();
    private MutliSelectSpinnerAdapter mutliSelectSpinnerAdapter;
    private String title;
    private float titleSize = 25.0F;
    private String positiveText = "DONE";
    private String negativeText = "CANCEL";
    private TextView dialogTitle;
    private TextView dialogSubmit;
    private TextView dialogCancel;
    private ArrayList<ParentSpinnerModel.Data> previouslySelectedIdsList = new ArrayList<>();
    //    private ArrayList<ParentSpinnerModel.FillesData> tempPreviouslySelectedIdsList = new ArrayList();
    private ArrayList<ParentSpinnerModel.Data> tempMainListOfAdapter = new ArrayList<>();
    private MultiSelectSpinner.SubmitCallbackListener submitCallbackListener;
    private int maxSelectionLimit = 0;
    private int minSelectionLimit = 1;

    public MultiSelectSpinner() {
    }

    @NonNull
    public Dialog onCreateDialog(Bundle savedInstanceState) {
        Dialog dialog = new Dialog(Objects.requireNonNull(this.getActivity()));
        Objects.requireNonNull(dialog.getWindow()).requestFeature(1);
        dialog.getWindow().setFlags(32, 1024);
        dialog.setContentView(layout.custom_multi_select);
        RecyclerViewEmptySupport mrecyclerView = dialog.findViewById(id.recycler_view);
        SearchView searchView = dialog.findViewById(id.search_view);
        this.dialogTitle = dialog.findViewById(id.title);
        this.dialogSubmit = dialog.findViewById(id.done);
        this.dialogCancel = dialog.findViewById(id.cancel);
        mrecyclerView.setEmptyView(dialog.findViewById(id.list_empty1));
        LinearLayoutManager layoutManager = new LinearLayoutManager(this.getActivity(), 1, false);
        mrecyclerView.setLayoutManager(layoutManager);
        this.dialogSubmit.setTextColor(Color.parseColor("#30CDDF"));
        this.dialogCancel.setTextColor(Color.parseColor("#30CDDF"));
        this.dialogSubmit.setOnClickListener(this);
        this.dialogCancel.setOnClickListener(this);
        this.settingValues();
        //this.mainListOfAdapter = this.setCheckedIDS(this.mainListOfAdapter, this.previouslySelectedIdsList);
        this.mutliSelectSpinnerAdapter = new MutliSelectSpinnerAdapter(this.mainListOfAdapter, this.getContext());
        mrecyclerView.setAdapter(this.mutliSelectSpinnerAdapter);
        searchView.setOnQueryTextListener(this);
        searchView.onActionViewExpanded();
        searchView.clearFocus();
        dialog.setCancelable(false);
        dialog.setCanceledOnTouchOutside(false);
        return dialog;
    }

    public MultiSelectSpinner title(String title) {
        this.title = title;
        return this;
    }

    public MultiSelectSpinner titleSize(float titleSize) {
        this.titleSize = titleSize;
        return this;
    }

    public MultiSelectSpinner positiveText(@NonNull String message) {
        this.positiveText = message;
        return this;
    }

    public MultiSelectSpinner negativeText(@NonNull String message) {
        this.negativeText = message;
        return this;
    }

    public MultiSelectSpinner preSelectIDsList(ArrayList<ParentSpinnerModel.Data> list) {
        this.previouslySelectedIdsList = list;
//        this.tempPreviouslySelectedIdsList = new ArrayList(this.previouslySelectedIdsList);
        return this;
    }

    public MultiSelectSpinner multiSelectList(ArrayList<ParentSpinnerModel.Data> list) {
        this.mainListOfAdapter = list;
        this.tempMainListOfAdapter = new ArrayList(this.mainListOfAdapter);
        if (this.maxSelectionLimit == 0) {
            this.maxSelectionLimit = list.size();
        }

        return this;
    }

    public MultiSelectSpinner setMaxSelectionLimit(int limit) {
        this.maxSelectionLimit = limit;
        return this;
    }

    public MultiSelectSpinner setMinSelectionLimit(int limit) {
        this.minSelectionLimit = limit;
        return this;
    }

    public MultiSelectSpinner onSubmit(@NonNull MultiSelectSpinner.SubmitCallbackListener callback) {
        this.submitCallbackListener = callback;
        return this;
    }

    private void settingValues() {
        this.dialogTitle.setText(this.title);
        this.dialogTitle.setTextSize(2, this.titleSize);
        this.dialogSubmit.setText(this.positiveText.toUpperCase());
        this.dialogCancel.setText(this.negativeText.toUpperCase());
    }

    /*private ArrayList<ParentSpinnerModel.FillesData> setCheckedIDS(ArrayList<ParentSpinnerModel.FillesData> multiselectdata, ArrayList<ParentSpinnerModel.FillesData> listOfIdsSelected) {
        for(int i = 0; i < multiselectdata.size(); ++i) {

            for(int j = 0; j < listOfIdsSelected.size(); ++j) {
                if (listOfIdsSelected.get(j).getId() == (multiselectdata.get(i)).getId()) {
                    (multiselectdata.get(i)).setSelected(true);
                }
            }
        }

        return multiselectdata;
    }*/

    private ArrayList<ParentSpinnerModel.Data> filter(ArrayList<ParentSpinnerModel.Data> models, String query) {
        query = query.toLowerCase();
        ArrayList<ParentSpinnerModel.Data> filteredModelList = new ArrayList<>();
        ParentSpinnerModel.Data data = new ParentSpinnerModel.Data();
        data.setUsername(Objects.requireNonNull(getString(R.string.all_users)));
        data.setId(0L);
        filteredModelList.add(0, data);
        if (query.equals("") | query.isEmpty()) {
            filteredModelList.addAll(models);
            return filteredModelList;
        } else {
            for (ParentSpinnerModel.Data model : models) {
                if (model != null && model.getUsername() != null && !model.getUsername().isEmpty()) {
                    String name = model.getUsername().toLowerCase();
                    if (name.contains(query)) {
                        filteredModelList.add(model);
                    }
                }
            }

            return filteredModelList;
        }
    }

    public boolean onQueryTextSubmit(String query) {
        return false;
    }

    public boolean onQueryTextChange(String newText) {
//        selectedIdsForCallback = this.previouslySelectedIdsList;
//        this.mainListOfAdapter = this.setCheckedIDS(this.mainListOfAdapter, selectedIdsForCallback);
        if (newText != null) {
            ArrayList<ParentSpinnerModel.Data> filteredlist = this.filter(this.mainListOfAdapter, newText);
            this.mutliSelectSpinnerAdapter.setData(filteredlist, newText.toLowerCase(), this.mutliSelectSpinnerAdapter);
            return false;
        }
        return false;
    }

    public void onClick(View view) {
        if (view.getId() == id.done) {
            ArrayList<ParentSpinnerModel.Data> callBackListOfIds = selectedIdsForCallback;
            String youCan;
            String options;
            String option;
            String message;
            if (this.submitCallbackListener != null) {
                this.submitCallbackListener.onSelected(callBackListOfIds, this.getSelectNameList(), this.getSelectedDataString());
            }

            this.dismiss();
        }
        if (view.getId() == id.cancel) {
            if (this.submitCallbackListener != null) {
//                selectedIdsForCallback.addAll(this.tempPreviouslySelectedIdsList);
                this.submitCallbackListener.onCancel();
            }

            this.dismiss();
        }

    }

    private String getSelectedDataString() {
        String data = "";

        for (int i = 0; i < this.tempMainListOfAdapter.size(); ++i) {
            if (this.checkForSelection((this.tempMainListOfAdapter.get(i)))) {
                data = data + ", " + (this.tempMainListOfAdapter.get(i)).getUsername();
            }
        }

        if (data.length() > 0) {
            return data.substring(1);
        } else {
            return "";
        }
    }

    private ArrayList<String> getSelectNameList() {
        ArrayList<String> names = new ArrayList<>();

        for (int i = 0; i < this.tempMainListOfAdapter.size(); ++i) {
            if (this.checkForSelection((this.tempMainListOfAdapter.get(i)))) {
                names.add((this.tempMainListOfAdapter.get(i)).getUsername());
            }
        }

        return names;
    }

    private boolean checkForSelection(ParentSpinnerModel.Data id) {
        for (int i = 0; i < selectedIdsForCallback.size(); ++i) {
            if (id.equals(selectedIdsForCallback.get(i))) {
                return true;
            }
        }

        return false;
    }

    public interface SubmitCallbackListener {
        void onSelected(ArrayList<ParentSpinnerModel.Data> var1, ArrayList<String> var2, String var3);

        void onCancel();
    }
}

//
// Source code recreated from a .class file by IntelliJ IDEA
// (powered by Fernflower decompiler)
//

package com.ibl.apps.util.CustomView;

import android.content.Context;
import android.content.res.ColorStateList;
import android.text.SpannableString;
import android.text.style.TextAppearanceSpan;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.CompoundButton;
import android.widget.RelativeLayout;
import android.widget.TextView;

import androidx.annotation.NonNull;
import androidx.appcompat.widget.AppCompatCheckBox;
import androidx.core.content.ContextCompat;
import androidx.recyclerview.widget.RecyclerView.Adapter;
import androidx.recyclerview.widget.RecyclerView.ViewHolder;

import com.ibl.apps.Model.parent.ParentSpinnerModel;
import com.ibl.apps.noon.NoonApplication;
import com.ibl.apps.noon.R;

import java.util.ArrayList;

import static com.ibl.apps.Fragment.ProgressReportFragment.selectedIdsForCallback;

class MutliSelectSpinnerAdapter extends Adapter<MutliSelectSpinnerAdapter.MultiSelectDialogViewHolder> {
    private ArrayList<ParentSpinnerModel.Data> mDataSet = new ArrayList();
    private String mSearchQuery = "";
    private Context mContext;
    private boolean isAllSelect = false;

    MutliSelectSpinnerAdapter(ArrayList<ParentSpinnerModel.Data> dataSet, Context context) {
        this.mDataSet = dataSet;
        this.mContext = context;
    }

    @NonNull
    public MutliSelectSpinnerAdapter.MultiSelectDialogViewHolder onCreateViewHolder(@NonNull ViewGroup parent, int viewType) {
        View view = LayoutInflater.from(parent.getContext()).inflate(R.layout.include_spinner_checkbox_item, parent, false);
        return new MutliSelectSpinnerAdapter.MultiSelectDialogViewHolder(view);
    }

    public void onBindViewHolder(@NonNull final MutliSelectSpinnerAdapter.MultiSelectDialogViewHolder holder, int position) {
        if (position != 0) {
            if (!this.mSearchQuery.equals("") && this.mSearchQuery.length() > 1) {
                this.setHighlightedText(position, holder.dialog_name_item);
            } else {
                if (this.mDataSet.get(position) != null && this.mDataSet.get(position).getUsername() != null && !this.mDataSet.get(position).getUsername().isEmpty()) {
                    holder.dialog_name_item.setText((this.mDataSet.get(position)).getUsername());
                } else {
                    holder.dialog_name_item.setText((this.mDataSet.get(position)).getFullName());
                }
            }
        } else {
            if (this.mDataSet.get(position) != null && this.mDataSet.get(position).getUsername() != null && !this.mDataSet.get(position).getUsername().isEmpty()) {
                holder.dialog_name_item.setText((this.mDataSet.get(position)).getUsername());
            } else {
                holder.dialog_name_item.setText(this.mContext.getResources().getString(R.string.username));
            }
        }

        /*if (selectedIdsForCallback.size() > 0) {
            for (int i = 0; i < selectedIdsForCallback.size(); i++) {
                if (selectedIdsForCallback.get(i).getUsername().equals(mDataSet.get(position).getUsername())) {
                    holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                    holder.dialog_item_checkbox.setChecked(true);
                }else {
                    holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                    holder.dialog_item_checkbox.setChecked(false);
                }
            }
        }*/

        if (position == 0) {
            if (mDataSet.get(position).getUsername().contains(NoonApplication.getContext().getResources().getString(R.string.all_users))) {
                if (mDataSet.size() - 1 == selectedIdsForCallback.size()) {
                    holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                    holder.dialog_item_checkbox.setChecked(true);
                } else {
                    holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                    holder.dialog_item_checkbox.setChecked(false);
                }
            } else {
                if (this.checkForSelection(this.mDataSet.get(position))) {
                    holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                    holder.dialog_item_checkbox.setChecked(true);
                } else {
                    holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                    holder.dialog_item_checkbox.setChecked(false);
                }
            }

        } else {
            if (this.checkForSelection(this.mDataSet.get(position))) {
                holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                holder.dialog_item_checkbox.setChecked(true);
            } else {
                holder.dialog_item_checkbox.setOnCheckedChangeListener(null);
                holder.dialog_item_checkbox.setChecked(false);
            }
        }


//        if (isAllSelect) {
//            holder.dialog_item_checkbox.setChecked(true);
//        } else {
//            if (this.checkForSelection(this.mDataSet.get(position))) {
//                holder.dialog_item_checkbox.setChecked(true);
//            } else {
//                holder.dialog_item_checkbox.setChecked(false);
//            }
//        }


        holder.main_container.setOnClickListener(new View.OnClickListener() {
            public void onClick(View view) {
                if (!holder.dialog_item_checkbox.isChecked()) {
                    holder.dialog_item_checkbox.setChecked(true);
                    // ((ParentSpinnerModel.FillesData)MutliSelectSpinnerAdapter.this.mDataSet.get(holder.getAdapterPosition())).setSelected(true);
                    MutliSelectSpinnerAdapter.this.notifyItemChanged(holder.getAdapterPosition());
                } else {
                    holder.dialog_item_checkbox.setChecked(false);
//                    ((ParentSpinnerModel.FillesData)MutliSelectSpinnerAdapter.this.mDataSet.get(holder.getAdapterPosition())).setSelected(false);
                    MutliSelectSpinnerAdapter.this.notifyItemChanged(holder.getAdapterPosition());
                }

            }
        });
        /*if (selectedIdsForCallback.isEmpty()) {
            for (int i = 1; i < mDataSet.size(); i++) {
                selectedIdsForCallback.add(mDataSet.get(i));
                holder.dialog_item_checkbox.setChecked(true);
            }
        } else {
            for (int i = 1; i < mDataSet.size(); i++) {
                for (int j = 0; j < selectedIdsForCallback.size(); j++) {
                    if (!mDataSet.get(i).getUsername().equals(selectedIdsForCallback.get(j).getUsername())&&mDataSet.get(i).getId()!=0L) {
                        selectedIdsForCallback.add(mDataSet.get(i));
                        holder.dialog_item_checkbox.setChecked(true);
                    }
                }

            }
        }*/

        if (position == 0) {
            if (mDataSet.get(position).getUsername().contains(NoonApplication.getContext().getResources().getString(R.string.all_users))) {
                holder.dialog_item_checkbox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton compoundButton, boolean b) {

                        if (b) {
                            selectedIdsForCallback.clear();
                            for (int i = 1; i < mDataSet.size(); i++) {
                                selectedIdsForCallback.add(mDataSet.get(i));
                                holder.dialog_item_checkbox.setChecked(true);
                            }
                            notifyDataSetChanged();
                        } else {
                            selectedIdsForCallback.clear();
                            notifyDataSetChanged();
                        }
                    }
                });
            } else {
                holder.dialog_item_checkbox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                    @Override
                    public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                        if (b) {
                            if (mDataSet.get(position).getId() != 0L) {
                                selectedIdsForCallback.add(mDataSet.get(position));
                                holder.dialog_item_checkbox.setChecked(true);
                            }
                            notifyDataSetChanged();
                        } else {
                            MutliSelectSpinnerAdapter.this.removeFromSelection(mDataSet.get(position));
                            holder.dialog_item_checkbox.setChecked(false);
                            notifyDataSetChanged();
                        }
                    }
                });
            }

        } else {

            holder.dialog_item_checkbox.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
                @Override
                public void onCheckedChanged(CompoundButton compoundButton, boolean b) {
                    if (b) {
                        if (mDataSet.get(position).getId() != 0L) {
                            selectedIdsForCallback.add(mDataSet.get(position));
                            holder.dialog_item_checkbox.setChecked(true);
                        }
                        notifyDataSetChanged();
                    } else {
                        MutliSelectSpinnerAdapter.this.removeFromSelection(mDataSet.get(position));
                        holder.dialog_item_checkbox.setChecked(false);
                        notifyDataSetChanged();
                    }
                }
            });
        }
    }

    private void setHighlightedText(int position, TextView textview) {
        String name = ((ParentSpinnerModel.Data) this.mDataSet.get(position)).getUsername();
        SpannableString str = new SpannableString(name);
        int endLength = name.toLowerCase().indexOf(this.mSearchQuery) + this.mSearchQuery.length();
        ColorStateList highlightedColor = new ColorStateList(new int[][]{new int[0]}, new int[]{ContextCompat.getColor(this.mContext, R.color.colorPrimary)});
        TextAppearanceSpan textAppearanceSpan = new TextAppearanceSpan((String) null, 0, -1, highlightedColor, (ColorStateList) null);
        str.setSpan(textAppearanceSpan, name.toLowerCase().indexOf(this.mSearchQuery), endLength, 33);
        textview.setText(str);
    }

    private void removeFromSelection(ParentSpinnerModel.Data data) {
        selectedIdsForCallback.remove(data);
    }

    private boolean checkForSelection(ParentSpinnerModel.Data id) {
        for (int i = 0; i < selectedIdsForCallback.size(); ++i) {
            if (id.getUsername() != null && selectedIdsForCallback.get(i).getUsername() != null) {
                if (id.getUsername().equals(selectedIdsForCallback.get(i).getUsername())) {
                    return true;
                }
            } else {
                if (id.getFullName() != null && selectedIdsForCallback.get(i).getFullName() != null) {
                    if (id.getFullName().equals(selectedIdsForCallback.get(i).getFullName())) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public int getItemCount() {
        return this.mDataSet.size();
    }

    void setData(ArrayList<ParentSpinnerModel.Data> data, String query, MutliSelectSpinnerAdapter mutliSelectSpinnerAdapter) {
        this.mDataSet = data;
        this.mSearchQuery = query;
        mutliSelectSpinnerAdapter.notifyDataSetChanged();
    }

    class MultiSelectDialogViewHolder extends ViewHolder {
        private TextView dialog_name_item;
        private AppCompatCheckBox dialog_item_checkbox;
        private RelativeLayout main_container;

        MultiSelectDialogViewHolder(View view) {
            super(view);
            this.dialog_name_item = (TextView) view.findViewById(R.id.text);
            this.dialog_item_checkbox = (AppCompatCheckBox) view.findViewById(R.id.checkbox);
            this.main_container = (RelativeLayout) view.findViewById(R.id.main_container);
        }
    }
}

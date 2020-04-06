package com.ibl.apps.Adapter;

import android.content.Context;
import androidx.databinding.DataBindingUtil;
import androidx.recyclerview.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.ViewGroup;
import android.widget.Filter;
import android.widget.Filterable;

import com.ibl.apps.Model.LibraryGradeObject;
import com.ibl.apps.RoomDatabase.entity.UserDetails;
import com.ibl.apps.noon.R;
import com.ibl.apps.noon.databinding.LibraryGradeListLayoutBinding;

import java.util.ArrayList;
import java.util.List;

/**
 * Created on 28/09/17 by iblinfotech.
 */

public class LibraryGradeListAdapter extends RecyclerView.Adapter<LibraryGradeListAdapter.MyViewHolder> implements Filterable {

    List<LibraryGradeObject.Data> list;
    List<LibraryGradeObject.Data> ListFiltered;
    Context ctx;
    UserDetails userDetailsObject;

    public class MyViewHolder extends RecyclerView.ViewHolder {
        LibraryGradeListLayoutBinding libraryGradeListLayoutBinding;

        public MyViewHolder(LibraryGradeListLayoutBinding libraryGradeListLayoutBinding) {
            super(libraryGradeListLayoutBinding.getRoot());
            this.libraryGradeListLayoutBinding = libraryGradeListLayoutBinding;
        }
    }

    public LibraryGradeListAdapter(Context ctx, List<LibraryGradeObject.Data> list, UserDetails userDetailsObject) {
        this.list = list;
        this.ListFiltered = list;
        this.ctx = ctx;
        this.userDetailsObject = userDetailsObject;
    }

    @Override
    public MyViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        LibraryGradeListLayoutBinding itemViewBinding = DataBindingUtil.inflate(LayoutInflater.from(parent.getContext()), R.layout.library_grade_list_layout, parent, false);
        return new MyViewHolder(itemViewBinding);
    }

    @Override
    public void onBindViewHolder(MyViewHolder holder, int position) {
        LibraryGradeObject.Data model = ListFiltered.get(position);
        holder.libraryGradeListLayoutBinding.txtgradeName.setText(model.getGradename());
        loadData(holder, model, 0);
    }

    private void loadData(MyViewHolder holder, LibraryGradeObject.Data model, int expandFlag) {
        List<LibraryGradeObject.Books> libraryBookArrayList = new ArrayList<>();
        libraryBookArrayList = model.getBooks();

        holder.libraryGradeListLayoutBinding.rcHorizontalLayout.rcVertical.setHasFixedSize(true);
        LibraryGradeInnerListAdapter adp = new LibraryGradeInnerListAdapter(ctx, libraryBookArrayList, userDetailsObject);
        holder.libraryGradeListLayoutBinding.rcHorizontalLayout.rcVertical.setAdapter(adp);
    }

    @Override
    public int getItemCount() {
        return ListFiltered.size();
    }

    @Override
    public int getItemViewType(int position) {
        return position;
    }


    @Override
    public Filter getFilter() {
        return new Filter() {
            @Override
            protected FilterResults performFiltering(CharSequence charSequence) {
                String charString = charSequence.toString();
                if (charString.isEmpty()) {
                    ListFiltered = list;
                } else {
                    ArrayList<LibraryGradeObject.Data> filteredList = new ArrayList<>();
                    for (LibraryGradeObject.Data row : list) {
                        ArrayList<LibraryGradeObject.Books> innerBooksArrayList = new ArrayList<>();
                        for (LibraryGradeObject.Books booksrow : row.getBooks()) {
                            if (booksrow.getTitle().toLowerCase().contains(charString.toLowerCase()) ||
                                    booksrow.getAuthor().toLowerCase().contains(charString.toLowerCase()) ||
                                    booksrow.getSubject().toLowerCase().contains(charString.toLowerCase()) ||
                                    booksrow.getDescription().toLowerCase().contains(charString.toLowerCase()) ||
                                    booksrow.getBookpublisher().toLowerCase().contains(charString.toLowerCase())) {
                                innerBooksArrayList.add(booksrow);

                            }
                        }
                        if (innerBooksArrayList.size() != 0) {
                            LibraryGradeObject.Data newEditeRow = new LibraryGradeObject.Data();
                            newEditeRow.setBooks(innerBooksArrayList);
                            newEditeRow.setGradename(row.getGradename());
                            newEditeRow.setId(row.getId());
                            filteredList.add(newEditeRow);
                        }
                    }
                    ListFiltered = filteredList;
                }

                FilterResults filterResults = new FilterResults();
                filterResults.values = ListFiltered;
                return filterResults;
            }

            @Override
            protected void publishResults(CharSequence charSequence, FilterResults filterResults) {
                ListFiltered = (ArrayList<LibraryGradeObject.Data>) filterResults.values;
                notifyDataSetChanged();
            }
        };
    }
}
